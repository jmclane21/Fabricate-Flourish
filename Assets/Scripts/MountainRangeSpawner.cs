using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class MountainRangeSpawner : MonoBehaviour
{
    public GameObject[] mountainPrefabs;
    public Transform player;
    public float spawnRadius = 100f;
    public float checkDistance = 10f;
    public Vector2 scaleVariance = new Vector2(0.8f, 1.2f);
    public Vector2 rotationVariance = new Vector2(0, 360);

    private HashSet<Vector2> spawnedLocations = new HashSet<Vector2>();
    private List<GameObject> mountainPool = new List<GameObject>();
    public int poolSize = 10;
    public float clusterRadius = 20f;
    public int mountainsPerCluster = 3;
    public int clustersToSpawn = 3;
    public float initialSpawnRadius = 200f;

    void Start()
    {
        InitializeObjectPool();
        Vector2 playerPos2D = new Vector2(player.position.x, player.position.z);

        float originalSpawnRadius = spawnRadius;
        spawnRadius = initialSpawnRadius;
        SpawnMountainsAroundPlayer(playerPos2D);
        spawnRadius = originalSpawnRadius;
    }

    private void InitializeObjectPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject randomPrefab = mountainPrefabs[Random.Range(0, mountainPrefabs.Length)];
            GameObject mountain = Instantiate(randomPrefab);
            mountain.SetActive(false);
            mountainPool.Add(mountain);
        }
    }

    private GameObject GetPooledObject()
    {
        foreach (GameObject mountain in mountainPool)
        {
            if (!mountain.activeInHierarchy)
            {
                return mountain;
            }
        }
        GameObject newMountain = Instantiate(mountainPrefabs[Random.Range(0, mountainPrefabs.Length)]);
        newMountain.SetActive(false);
        mountainPool.Add(newMountain);
        return newMountain;
    }

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

    private void SpawnMountainsAroundPlayer(Vector2 playerPos2D)
    {
        for (int cluster = 0; cluster < clustersToSpawn; cluster++)
        {
            Vector3 clusterCenter = GetRandomPositionNearPlayer(playerPos2D);

            for (int i = 0; i < mountainsPerCluster; i++)
            {
                Vector3 randomPosition = GetRandomPositionInCluster(clusterCenter);
                GameObject mountain = GetPooledObject();

                mountain.SetActive(true);
                mountain.transform.position = randomPosition;

                float randomScale = Random.Range(scaleVariance.x, scaleVariance.y);
                mountain.transform.localScale = Vector3.one * randomScale;
                float randomRotationY = Random.Range(rotationVariance.x, rotationVariance.y);
                mountain.transform.rotation = Quaternion.Euler(0, randomRotationY, 0);

                CopyComponents(mountainPrefabs[Random.Range(0, mountainPrefabs.Length)], mountain);

                spawnedLocations.Add(new Vector2(randomPosition.x, randomPosition.z));
            }
        }
    }

    private void CopyComponents(GameObject source, GameObject target)
    {
        Component[] components = source.GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component == null || component is Transform)
                continue;

            // Add the component to the target GameObject
            Component copy = target.AddComponent(component.GetType());

            // Copy fields from the source component to the copied component
            FieldInfo[] fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                if (field.IsStatic)
                    continue;

                field.SetValue(copy, field.GetValue(component));
            }
        }
    }

    public void DeactivateMountain(GameObject mountain)
    {
        mountain.SetActive(false);
    }

    private Vector3 GetRandomPositionNearPlayer(Vector2 playerPos2D)
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        return new Vector3(playerPos2D.x + randomOffset.x, 0, playerPos2D.y + randomOffset.y);
    }

    private Vector3 GetRandomPositionInCluster(Vector3 center)
    {
        Vector2 randomOffset = Random.insideUnitCircle * clusterRadius;
        return new Vector3(center.x + randomOffset.x, 0, center.z + randomOffset.y);
    }
}
