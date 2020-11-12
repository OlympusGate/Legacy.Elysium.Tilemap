using Elysium.AI.Pathfinding;
using Elysium.Timers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnManager : MonoBehaviour
{
    // ----------------------- DEBUG ONLY ~> REMOVE LATER -----------------------
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { Spawn(); }
    }
    // --------------------------------------------------------------------------

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Tilemap spawnTilemap;
    [SerializeField] private Tilemap pathTilemap;

    private Dictionary<Vector3Int, Vector3> SpawnTiles { get; set; }
    private Dictionary<Vector3Int, Vector3> TargetTiles { get; set; }
    private Dictionary<Vector3Int, Vector3> PathTiles { get; set; }

    private void Awake()
    {
        SpawnTiles = new Dictionary<Vector3Int, Vector3>();
        TargetTiles = new Dictionary<Vector3Int, Vector3>();
        PathTiles = new Dictionary<Vector3Int, Vector3>();
        SetupSpawnTilemap();
        SetupPathTilemap();
    }

    private void SetupPathTilemap()
    {
        foreach (var position in pathTilemap.cellBounds.allPositionsWithin)
        {
            if (pathTilemap.HasTile(position))
            {
                var cell = pathTilemap.GetTile(position);

                if (cell is PathTile)
                {
                    var center = pathTilemap.GetCellCenterWorld(position);
                    PathTiles.Add(position, center);
                }
            }
        }
    }

    private void SetupSpawnTilemap()
    {
        foreach (var position in spawnTilemap.cellBounds.allPositionsWithin)
        {
            if (spawnTilemap.HasTile(position))
            {
                var cell = spawnTilemap.GetTile(position);

                if (cell is SpawnTile)
                {
                    var center = spawnTilemap.GetCellCenterWorld(position);
                    SpawnTiles.Add(position, center);
                }
                else if (cell is TargetTile)
                {
                    var center = spawnTilemap.GetCellCenterWorld(position);
                    TargetTiles.Add(position, center);
                }
            }
        }
    }

    private void Spawn()
    {
        var path = CalculateEnemyPath(SpawnTiles.First().Value, TargetTiles.First().Value);
        var enemy = Instantiate(enemyPrefab, SpawnTiles.First().Value, transform.rotation).GetComponent<AI_Movement>();
        enemy.Init(path);
    }

    private Vector3[] CalculateEnemyPath(Vector3 _origin, Vector3 _target)
    {
        Pathfinding pathfinding = new Pathfinding();

        foreach (Vector3 tile in PathTiles.Values)
        {
            var n = GetNeighbours(tile);
            pathfinding.AddNode(tile, n);                    
        }

        var path = pathfinding.FindPath(_origin, _target);
        Vector3[] result = new Vector3[path.Count];

        for (int i = 0; i < path.Count; i++)
        {
            result[i] = path[i].position;
        }

        return result;
    }

    private List<Vector3> GetNeighbours(Vector3 _tile)
    {
        List<Vector3> neighbours = new List<Vector3>();

        foreach (var possibleNeighbour in PathTiles.Values)
        {
            if (possibleNeighbour == _tile) { continue; }
            if (Vector3.Distance(_tile, possibleNeighbour) == 1)
            {
                neighbours.Add(possibleNeighbour);
            }
        }

        return neighbours;
    }
}
