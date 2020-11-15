using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileValidator : MonoBehaviour
{
    public struct TileDetails
    {
        public List<Vector3> worldPositions;
        public List<Vector3Int> tilePositions;
    }

    [SerializeField] private List<Tile> validTiles;
    
    public Dictionary<string, TileDetails> ValidTiles { get; set; }

    private Tilemap tilemap;

    public List<Vector3> GetWorldPositions(string[] _key)
    {
        if (ValidTiles == null) { Debug.LogError("Valid Tiles not initalized yet."); return null; }

        List<Vector3> results = new List<Vector3>();

        for (int i = 0; i < _key.Length; i++)
        {
            results.AddRange(ValidTiles[_key[i]].worldPositions);
        }

        return results;
    }

    public List<Vector3Int> GetCellPositions(string[] _key)
    {
        if (ValidTiles == null) { Debug.LogError("Valid Tiles not initalized yet."); return null; }

        List<Vector3Int> results = new List<Vector3Int>();

        for (int i = 0; i < _key.Length; i++)
        {
            results.AddRange(ValidTiles[_key[i]].tilePositions);
        }

        return results;
    }

    private void Awake()
    { 
        tilemap = GetComponent<Tilemap>();
        SetupTilemap();
    }    

    private void SetupTilemap()
    {
        ValidTiles = new Dictionary<string, TileDetails>();

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile(position))
            {
                var cell = tilemap.GetTile(position);

                for (int i = 0; i < validTiles.Count; i++)
                {
                    if (validTiles[i] == null) { continue; }

                    if (cell.GetType() == validTiles[i].GetType())
                    {
                        var center = tilemap.GetCellCenterWorld(position);
                        AddElement(cell.GetType().ToString(), center, position);
                    }
                }                
            }
        }
    }

    private void AddElement(string _key, Vector3 _worldValue, Vector3Int _tileValue)
    {
        if (!ValidTiles.ContainsKey(_key))
        {
            ValidTiles.Add(_key, new TileDetails
            {
                worldPositions = new List<Vector3>() { _worldValue },
                tilePositions = new List<Vector3Int>() { _tileValue },
            });
            return;
        }

        ValidTiles[_key].worldPositions.Add(_worldValue);
        ValidTiles[_key].tilePositions.Add(_tileValue);
    }
}
