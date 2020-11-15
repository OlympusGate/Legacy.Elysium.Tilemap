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

    private Tilemap tilemap;

    private List<Vector3> pathTiles;
    private List<Vector3> spawnTiles;
    private List<Vector3> targetTile;

    private void Awake() => tilemap = GetComponent<Tilemap>();

    private void Start() => BindValidTiles();

    private void BindValidTiles()
    {
        TileValidator tileValidator = new TileValidator(tilemap);
        pathTiles = tileValidator.GetWorldPositions(new string[] { "PathTile", "SpawnTile", "TargetTile" });
        spawnTiles = tileValidator.GetWorldPositions(new string[] { "SpawnTile" });
        targetTile = tileValidator.GetWorldPositions(new string[] { "TargetTile" });
    }    

    private void Spawn()
    {
        var origin = spawnTiles.First();
        var destination = targetTile.First();
        var path = CalculateEnemyPath(origin, destination);
        var enemy = Instantiate(enemyPrefab, origin, transform.rotation).GetComponent<AI_Movement>();
        enemy.Init(path);
    }

    private Vector3[] CalculateEnemyPath(Vector3 _origin, Vector3 _target)
    {
        Pathfinding pathfinding = new Pathfinding();

        foreach (Vector3 tile in pathTiles)
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

        foreach (var possibleNeighbour in pathTiles)
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
