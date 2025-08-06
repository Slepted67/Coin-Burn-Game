using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles procedural generation of gameplay segments (obstacles + flat platforms).
/// Spawns new segments based on player's distance from the last segment endpoint.
/// </summary>
public class ProceduralSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;                // Reference to the player object
    public GameObject startPlatform;        // Initial platform to spawn at (0,0)
    public GameObject[] flatVariants;       // Flat platform variants (no obstacles)
    public GameObject[] obstacleVariants;   // Obstacle segment variants

    [Header("Spawn Settings")]
    public float spawnDistance = 20f;       // How close the player must be to trigger next segment spawn
    public float minSpacing = 10f;          // Not used currently, but may be for spacing control
    public float maxSpacing = 15f;          // Not used currently
    public static float FarthestX { get; private set; } = 0f; // Track farthest point generated (used by background)

    [Header("Memory Settings")]
    public int memorySize = 3;              // Prevent recently-used obstacles from repeating
    private Queue<int> recentObstacleIndices = new Queue<int>();

    private Transform lastEndPoint;         // Position of the last segment's end
    private bool hasStarted = false;        // Flag to delay spawning until initialization is complete

    void Start()
    {
        // STEP 1: Spawn the start platform at the world origin
        GameObject start = Instantiate(startPlatform, Vector3.zero, Quaternion.identity);
        lastEndPoint = start.transform.Find("EndPoint");

        // STEP 2: Spawn the first flat platform after the start
        GameObject firstFlat = Instantiate(GetRandomFlat(), lastEndPoint.position, Quaternion.identity);
        ProcessOptionalCoins(firstFlat);
        lastEndPoint = firstFlat.transform.Find("EndPoint");

        // Update the farthest X position
        FarthestX = Mathf.Max(FarthestX, lastEndPoint.position.x);
        hasStarted = true;
    }

    void Update()
    {
        // Spawn new segments if player is close to the last segment’s end
        if (hasStarted && player != null && Vector3.Distance(player.position, lastEndPoint.position) < spawnDistance)
        {
            SpawnNextSegment();
        }
    }

    /// <summary>
    /// Spawns an obstacle segment followed by a flat segment.
    /// Also updates the lastEndPoint and farthest X-position.
    /// </summary>
    void SpawnNextSegment()
    {
        // 1. Pick a non-recent obstacle
        int index = GetRandomObstacleIndex();

        // 2. Spawn the obstacle segment
        GameObject obstacle = Instantiate(obstacleVariants[index], lastEndPoint.position, Quaternion.identity);
        ProcessOptionalCoins(obstacle);

        // 3. Get the obstacle's EndPoint position
        Transform obstacleEnd = obstacle.transform.Find("EndPoint");

        // 4. Spawn a flat segment after the obstacle
        GameObject flat = Instantiate(GetRandomFlat(), obstacleEnd.position, Quaternion.identity);
        ProcessOptionalCoins(flat);

        // 5. Update lastEndPoint and farthestX
        lastEndPoint = flat.transform.Find("EndPoint");
        FarthestX = Mathf.Max(FarthestX, lastEndPoint.position.x);
    }

    /// <summary>
    /// Disables optional coins randomly based on their individual spawn chances.
    /// </summary>
    /// <param name="segment">The platform segment containing optional coins</param>
    void ProcessOptionalCoins(GameObject segment)
    {
        OptionalCoin[] optionalCoins = segment.GetComponentsInChildren<OptionalCoin>();

        foreach (OptionalCoin coin in optionalCoins)
        {
            if (Random.value > coin.spawnChance)
            {
                coin.gameObject.SetActive(false); // Hide the coin
            }
        }
    }

    /// <summary>
    /// Selects a random obstacle index not recently used.
    /// </summary>
    /// <returns>A random (but non-recent) obstacle index</returns>
    int GetRandomObstacleIndex()
    {
        int index;
        int attempts = 0;

        // Try to avoid recently used obstacle variants
        do
        {
            index = Random.Range(0, obstacleVariants.Length);
            attempts++;
        } while (recentObstacleIndices.Contains(index) && attempts < 10);

        // Store used index in memory to avoid repetition
        recentObstacleIndices.Enqueue(index);
        if (recentObstacleIndices.Count > memorySize)
            recentObstacleIndices.Dequeue();

        return index;
    }

    /// <summary>
    /// Picks a random flat platform from available variants.
    /// </summary>
    GameObject GetRandomFlat()
    {
        return flatVariants[Random.Range(0, flatVariants.Length)];
    }
}