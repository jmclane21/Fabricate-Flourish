using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ObjectManager class to handle object spawning and manipulation
public class ObjectManager : MonoBehaviour
{
    public GameObject rockPrefab;      // The rock prefab to spawn
    public Camera mainCamera;          // Reference to the main camera
    private List<GameObject> spawnedRocks = new List<GameObject>();  // List to keep track of spawned rocks

    // Method to spawn objects
    public void SpawnObject(Vector3 spawnPosition)
    {
        GameObject spawnedRock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
        spawnedRocks.Add(spawnedRock);
    }

    // Method to handle raycast and object spawning
    public void HandleSpawnOnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Spawn object at hit point
                SpawnObject(hit.point);
            }
        }
    }

    // Method to delete selected objects
    private void DeleteObjects()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            foreach (GameObject obj in spawnedRocks)
            {
                Destroy(obj);
            }
            spawnedRocks.Clear();
        }
    }

    // Method to scale all spawned objects up or down
    private void ScaleObjects()
    {
        if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            foreach (GameObject obj in spawnedRocks)
            {
                obj.transform.localScale *= 1.1f; // Increase size by 10%
            }
        }
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            foreach (GameObject obj in spawnedRocks)
            {
                obj.transform.localScale *= 0.9f; // Decrease size by 10%
            }
        }
    }

    // Method to rotate all spawned objects
    private void RotateObjects()
    {
        if (Input.GetKey(KeyCode.R))
        {
            foreach (GameObject obj in spawnedRocks)
            {
                obj.transform.Rotate(Vector3.up, 45 * Time.deltaTime); // Rotate around Y-axis
            }
        }
    }

    void Update()
    {
        // Handle spawning on click
        HandleSpawnOnClick();
        
        // Handle deletion, scaling, and rotation with key presses
        DeleteObjects();
        ScaleObjects();
        RotateObjects();
    }
}
