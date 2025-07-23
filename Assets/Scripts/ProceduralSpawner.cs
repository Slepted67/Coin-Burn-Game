using System.Collections.Generic;
using UnityEngine;

public class ProceduralSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject startPlatform;       // Your start platform prefab
    public GameObject[] flatVariants;      // Your flat ground pieces
    public GameObject[] obstacleVariants;  // Your obstacles

    [Header("Spawn Settings")]
    public float spawnDistance = 20f;
    public float minSpacing = 10f;
    public float maxSpacing = 15f;

    [Header("Memory Settings")]
    public int memorySize = 3;
    private Queue<int> recentObstacleIndices = new Queue<int>();

    private Transform lastEndPoint;
    private bool hasStarted = false;

    void Start()
    {
        // STEP 1: Spawn Start Platform at origin
        GameObject start = Instantiate(startPlatform, Vector3.zero, Quaternion.identity);
        lastEndPoint = start.transform.Find("EndPoint");

        // STEP 2: Spawn initial flat land immediately after
        GameObject firstFlat = Instantiate(GetRandomFlat(), lastEndPoint.position, Quaternion.identity);
        ProcessOptionalCoins(firstFlat);
        lastEndPoint = firstFlat.transform.Find("EndPoint");

        hasStarted = true;
    }
    
    void SpawnNextSegment()
    {
        // 1. Choose a non-recent obstacle variant
        int index = GetRandomObstacleIndex();

        // 2. Spawn obstacle and process optional coins in it
        GameObject obstacle = Instantiate(obstacleVariants[index], lastEndPoint.position, Quaternion.identity);
        ProcessOptionalCoins(obstacle);

        Transform obstacleEnd = obstacle.transform.Find("EndPoint");

        // 3. Spawn flat segment after obstacle and process its optional coins
        GameObject flat = Instantiate(GetRandomFlat(), obstacleEnd.position, Quaternion.identity);
        ProcessOptionalCoins(flat);

        lastEndPoint = flat.transform.Find("EndPoint");
    }


    void Update()
    {
        if (hasStarted && player != null && Vector3.Distance(player.position, lastEndPoint.position) < spawnDistance)
        {
            SpawnNextSegment();
        }
    }

    void ProcessOptionalCoins(GameObject segment)
    {
        OptionalCoin[] optionalCoins = segment.GetComponentsInChildren<OptionalCoin>();

        foreach (OptionalCoin coin in optionalCoins)
        {
            if (Random.value > coin.spawnChance)
            {
                coin.gameObject.SetActive(false);
            }
        }
    }

    int GetRandomObstacleIndex()
    {
        int index;
        int attempts = 0;
        do
        {
            index = Random.Range(0, obstacleVariants.Length);
            attempts++;
        } while (recentObstacleIndices.Contains(index) && attempts < 10);

        // Remember last X indices
        recentObstacleIndices.Enqueue(index);
        if (recentObstacleIndices.Count > memorySize)
            recentObstacleIndices.Dequeue();

        return index;
    }

    GameObject GetRandomFlat()
    {
        return flatVariants[Random.Range(0, flatVariants.Length)];
    }
}
