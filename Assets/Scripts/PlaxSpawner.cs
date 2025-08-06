using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Repeats background tiles based on the furthest obstacle position in the scene.
/// Ensures there's always background behind and slightly ahead of the visible level.
/// </summary>
public class BackgroundRepeaterByObstacles : MonoBehaviour
{
    [Header("Background Settings")]
    public GameObject backgroundTilePrefab; // Prefab for one segment of background
    public float tileWidth = 49.75f;        // Horizontal space each background tile covers
    public int bufferTilesAhead = 1;        // Number of tiles to spawn ahead of furthest obstacle
    public float tileY = 0f;                // Fixed Y-position for background layer

    private Queue<GameObject> spawnedTiles = new Queue<GameObject>(); // Track all spawned tiles
    private float lastSpawnedX = 0f;        // X position of the last tile spawned

    void Start()
    {
        // Start with one tile at the origin
        SpawnTileAtX(0f);
    }

    void Update()
    {
        // Determine how far ahead to spawn background tiles
        float targetX = ProceduralSpawner.FarthestX + bufferTilesAhead * tileWidth;

        // Continue spawning tiles until we’ve covered the area up to targetX
        while (lastSpawnedX < targetX)
        {
            lastSpawnedX += tileWidth;
            SpawnTileAtX(lastSpawnedX);
        }
    }

    /// <summary>
    /// Spawns a single background tile at a given X position.
    /// </summary>
    /// <param name="x">World X position where the tile will be placed</param>
    void SpawnTileAtX(float x)
    {
        Vector3 spawnPos = new Vector3(x, tileY, 0f); // Position on background layer
        GameObject tile = Instantiate(backgroundTilePrefab, spawnPos, Quaternion.identity, transform);

        // Ensure consistent scale if tiles were manually resized
        tile.transform.localScale = new Vector3(5.375f, 5.375f, 1f);

        spawnedTiles.Enqueue(tile); // Track tile in queue (could later be used to destroy old tiles)
    }
}
