using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageManager : MonoBehaviour
{
    public GameObject[] foliagePrefabs;  // Array of foliage prefabs to spawn
    public float spawnRadius = 20f;      // The maximum distance from the player to spawn foliage
    public int maxAttempts = 30;         // Maximum attempts to find a valid spawn location

    private Transform playerTransform;

    private void Start()
    {
        // Find the player object
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Ensure the player has the 'Player' tag.");
        }

        if (foliagePrefabs.Length == 0)
        {
            Debug.LogError("No foliage prefabs assigned to FoliageManager.");
        }
    }

    public void SpawnFoliageNearPlayer()
    {
        if (playerTransform == null) return;

        Vector3 spawnPosition = Vector3.zero;
        bool foundPosition = false;

        // Attempt to find a valid position for foliage
        for (int i = 0; i < maxAttempts; i++)
        {
            if (TryPlaceFoliage(out spawnPosition))
            {
                foundPosition = true;
                break;
            }
        }

        if (!foundPosition)
        {
            spawnPosition = playerTransform.position;  // Fallback to player's position
            Debug.LogWarning("Fallback: Spawning foliage at player position.");
        }

        // Randomly select a foliage prefab from the array
        int randomIndex = Random.Range(0, foliagePrefabs.Length);
        GameObject selectedPrefab = foliagePrefabs[randomIndex];

        // Instantiate the selected foliage prefab at the spawn position
        Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
    }

    public bool TryPlaceFoliage(out Vector3 spawnPosition)
    {
        spawnPosition = playerTransform.position + new Vector3(
            Random.Range(-spawnRadius, spawnRadius),
            0,
            Random.Range(-spawnRadius, spawnRadius)
        );

        // Debug visualization: Draw a line to show the raycast attempts in the scene view
        Debug.DrawLine(spawnPosition + Vector3.up * 10, spawnPosition - Vector3.up * 10, Color.red, 2f);

        // Check for valid ground position
        RaycastHit hit;
        if (Physics.Raycast(spawnPosition + Vector3.up * 10, Vector3.down, out hit, 20f))
        {
            if (hit.collider != null && hit.collider.CompareTag("Ground"))  // Replace "Ground" with your ground tag
            {
                spawnPosition.y = hit.point.y;  // Adjust spawn position to ground level
                return true;
            }
        }

        return false;
    }
}
