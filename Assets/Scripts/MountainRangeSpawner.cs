using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// uses object pooling and forms of random generation to create a mountain range around the player
public class MountainRangeSpawner : MonoBehaviour
{
    public GameObject[] mountainPrefabs;  // Array of different mountain range prefabs
    public Transform player;              // Reference to the player's transform
    public float spawnRadius = 100f;      // Distance around the player where mountains will spawn
    public float checkDistance = 10f;     // Minimum distance before triggering new spawns
    public Vector2 scaleVariance = new Vector2(0.8f, 1.2f); // Variance in scale of spawned mountains
    public Vector2 rotationVariance = new Vector2(0, 360);  // Variance in Y-axis rotation

    private HashSet<Vector2> spawnedLocations = new HashSet<Vector2>();  // Track where mountains have been spawned
    private List<GameObject> mountainPool = new List<GameObject>();      // Pool for mountain objects
    public int poolSize = 10;       // Number of objects to pool

    public float clusterRadius = 20f;  // Radius within which each cluster spawns mountains
public int mountainsPerCluster = 3;  // Number of mountains per cluster
public int clustersToSpawn = 3;      // Number of clusters to spawn around the player
public float initialSpawnRadius = 200f;  // Larger spawn radius for scene loading



    void Start()
    {
        InitializeObjectPool();
        Vector2 playerPos2D = new Vector2(player.position.x, player.position.z);

        // Temporarily set a larger spawn radius for initial load
        float originalSpawnRadius = spawnRadius;
        spawnRadius = initialSpawnRadius;  // Use a larger radius for initial generation
        SpawnMountainsAroundPlayer(playerPos2D);  // Pregenerate mountains in a larger area
        spawnRadius = originalSpawnRadius;  // Reset to regular spawn radius

    }

    void Update()
    {
    }

    // Initializes the object pool by creating inactive mountain objects
    private void InitializeObjectPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject randomPrefab = mountainPrefabs[Random.Range(0, mountainPrefabs.Length)];
            GameObject mountain = Instantiate(randomPrefab);
            mountain.SetActive(false);  // Start with all objects inactive
            mountainPool.Add(mountain);
        }
    }

    // Method to get a pooled object, or create one if the pool is empty
    private GameObject GetPooledObject()
    {
        foreach (GameObject mountain in mountainPool)
        {
            if (!mountain.activeInHierarchy)
            {
                return mountain;  // Return an inactive object from the pool
            }
        }
        // If no inactive objects are available, instantiate a new one and add it to the pool
        GameObject newMountain = Instantiate(mountainPrefabs[Random.Range(0, mountainPrefabs.Length)]);
        newMountain.SetActive(false);
        mountainPool.Add(newMountain);
        return newMountain;
    }

    // Determines if new mountains should be spawned based on player's position
    private bool ShouldSpawnNewMountains(Vector2 playerPos2D)
    {
        foreach (var location in spawnedLocations)
        {
            if (Vector2.Distance(playerPos2D, location) < checkDistance)
            {
                return false;
            }
        }
        return true;
    }

    // Spawns mountains around the player, using pooled objects
    private void SpawnMountainsAroundPlayer(Vector2 playerPos2D)
    {
         for (int cluster = 0; cluster < clustersToSpawn; cluster++)  // Loop to create clusters
    {
        Vector3 clusterCenter = GetRandomPositionNearPlayer(playerPos2D);  // Center of the cluster
        
        for (int i = 0; i < mountainsPerCluster; i++)  // Loop to spawn mountains within a cluster
        {
            Vector3 randomPosition = GetRandomPositionInCluster(clusterCenter);
            GameObject mountain = GetPooledObject();  // Get a pooled mountain

            // Activate the mountain and set its position
            mountain.SetActive(true);
            mountain.transform.position = randomPosition;

            // Apply random scale and rotation variance
            float randomScale = Random.Range(scaleVariance.x, scaleVariance.y);
            mountain.transform.localScale = Vector3.one * randomScale;
            float randomRotationY = Random.Range(rotationVariance.x, rotationVariance.y);
            mountain.transform.rotation = Quaternion.Euler(0, randomRotationY, 0);

            // Add the location to the spawned list to avoid respawning there
            spawnedLocations.Add(new Vector2(randomPosition.x, randomPosition.z));
        }
    }
    }

    // Deactivates the mountains after a certain distance or condition (if needed)
    public void DeactivateMountain(GameObject mountain)
    {
        mountain.SetActive(false);
    }

    // Get a random position around the player within the defined radius
    private Vector3 GetRandomPositionNearPlayer(Vector2 playerPos2D)
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        return new Vector3(playerPos2D.x + randomOffset.x, 0, playerPos2D.y + randomOffset.y);  // Y = 0 for ground placement
    }

    // Spawns mountains in clusters around the player, using pooled objects

// Get a random position within a smaller radius around a specified center point
private Vector3 GetRandomPositionInCluster(Vector3 center)
{
    Vector2 randomOffset = Random.insideUnitCircle * clusterRadius;
    return new Vector3(center.x + randomOffset.x, 0, center.z + randomOffset.y);  // Y = 0 for ground placement
}
}
