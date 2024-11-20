using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public Item itemToPlace;
    public Camera mainCamera;          // Reference to the main camera

    // Method to spawn objects
    public void SpawnObject(Vector3 spawnPosition)
    {
        itemToPlace = InventoryManager.Instance.getSelectedItem(true);
        GameObject spawnedRock = Instantiate(itemToPlace.prefabObject, spawnPosition, Quaternion.identity);
    }

    // Method to handle raycast and object spawning
    public void HandleSpawnOnClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (InventoryManager.Instance.selectedSlot >= 0)
            {
                itemToPlace = InventoryManager.Instance.getSelectedItem(false);
                Vector3 mousePosition = Input.mousePosition;
                Ray ray = mainCamera.ScreenPointToRay(mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Spawn object at hit point if within range
                    float distance = gameObject.transform.position.magnitude - hit.point.magnitude;
                    if (distance < itemToPlace.placeRange.magnitude)
                    {
                        SpawnObject(hit.point);
                    }

                }
            }
        }
    }

    void Update()
    {
        // Handle entering preview mode
        HandleSpawnOnClick();

        // Handle exit preview, scaling, and rotation
    }
}
