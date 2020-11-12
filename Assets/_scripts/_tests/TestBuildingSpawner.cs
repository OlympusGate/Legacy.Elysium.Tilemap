using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Elysium.Tilemaps
{
    public class TestBuildingSpawner : MonoBehaviour
    {
        public TestBuildingObject[] buildings;

        private BuildingTilemap smartTilemap;

        private void Awake() => smartTilemap = GetComponent<BuildingTilemap>();        

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (buildings.Length < 1)
                {
                    Debug.LogError("no buildings to instantiate!");
                    return;
                }

                // GET CLICKED POSITION
                var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z = 0;

                // CONSTRUCT BUILDING
                var r = Random.Range(0, buildings.Length);
                smartTilemap.ConstructBuilding(worldPosition, buildings[r]);
            }

            if (Input.GetMouseButtonDown(1))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;

                var ctx = smartTilemap.GetBuilding(pos);
                if (ctx != null) { ctx.Repair(); }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;

                smartTilemap.DemolishBuilding(pos);
            }
        }

        private void UpdateColor(Vector3Int _position, Color _color)
        {
            if (smartTilemap.GetBuilding(_position) == null)
            {
                smartTilemap.Tilemap.SetTileFlags(_position, TileFlags.None);
                smartTilemap.Tilemap.SetColor(_position, Color.clear);
                smartTilemap.Tilemap.SetTileFlags(_position, TileFlags.LockColor);
            }
            else
            {
                smartTilemap.Tilemap.SetTileFlags(_position, TileFlags.None);
                smartTilemap.Tilemap.SetColor(_position, _color);
                smartTilemap.Tilemap.SetTileFlags(_position, TileFlags.LockColor);
            }
        }
    }
}