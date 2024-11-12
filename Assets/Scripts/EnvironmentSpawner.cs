using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [Header("Grass Settings")]
    public GameObject[] grassPrefabs;       // Array of different grass prefabs
    public Transform player;                // Reference to the player's transform
    public float grassSpawnRadius = 150f;   // Area around the player to spawn grass
    public int grassDensity = 100;          // Number of grass objects per spawn area
    public Vector2 grassScaleVariance = new Vector2(0.5f, 1.5f); // Scale variance for grass

    private List<GameObject> grassPool = new List<GameObject>(); // Pool for grass objects

    void Start()
    {
        SpawnInitialGrass();  // Generate grass when the scene loads
    }

    // Spawns grass within the defined radius around the player
    private void SpawnInitialGrass()
    {
        for (int i = 0; i < grassDensity; i++)
        {
            Vector3 randomPosition = GetRandomPositionAroundPlayer();
            GameObject grass = Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Length)]);

            // Apply position, scale, and rotation variance
            grass.transform.position = randomPosition;
            float randomScale = Random.Range(grassScaleVariance.x, grassScaleVariance.y);
            grass.transform.localScale = Vector3.one * randomScale;
            grass.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

            grassPool.Add(grass);  // Optionally add grass to the pool for future use
        }
    }

    // Returns a random position within the defined grass spawn radius around the player
    private Vector3 GetRandomPositionAroundPlayer()
    {
        Vector2 randomOffset = Random.insideUnitCircle * grassSpawnRadius;
        return new Vector3(player.position.x + randomOffset.x, 0, player.position.z + randomOffset.y);  // Y = 0 for ground placement
    }
}

