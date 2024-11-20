using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    const KeyCode PLACE_BLOCK = KeyCode.Mouse1;
    const KeyCode DESTROY_BLOCK = KeyCode.Mouse0;
    const KeyCode EXIT = KeyCode.Tab;
    const KeyCode TRANSFORM_BLOCK = KeyCode.LeftShift;

    enum buildMode{
        PREVIEW,
        NONE,
        TRANSFORM
    }

    buildMode mode = buildMode.NONE;

    [HideInInspector] public Item itemToPlace;
    [HideInInspector] GameObject previewObject;
    public Camera mainCamera;

    // Method to spawn objects
    void SpawnObject(Vector3 spawnPosition)
    {
        itemToPlace = InventoryManager.Instance.getSelectedItem(true);
        GameObject spawnedItem = Instantiate(itemToPlace.prefabObject, spawnPosition, Quaternion.identity);
    }

    void EnterPreviewMode()
    {
        if (Input.GetKeyDown(PLACE_BLOCK))
        {
            if (InventoryManager.Instance.selectedSlot >= 0)
            {
                mode = buildMode.PREVIEW;
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
                        PreviewObject(hit.point);
                    }

                }
            }
        }
    }

    void ExitPreviewMode()
    {
        if (Input.GetKeyDown(EXIT)){
            itemToPlace = null;
            Destroy(previewObject);
            previewObject = null;
            mode = buildMode.NONE;
        }

    }

    void PreviewObject(Vector3 spawnPosition)
    {
        //grabs obj without taking from inventory
        itemToPlace = InventoryManager.Instance.getSelectedItem(false);

        previewObject = Instantiate(
            itemToPlace.prefabObject,
            spawnPosition, Quaternion.identity,
            //currently preview obj does not snap to ground/surfaces
            mainCamera.transform);
    }

    void BuildObject()
    {
        if (Input.GetKeyDown(PLACE_BLOCK))
        {
            if (InventoryManager.Instance.selectedSlot >= 0)
            {
                //removes obj from inventory when Actually placed
                InventoryManager.Instance.getSelectedItem(true);

                //decouples preview obj from player, places fully in scene
                previewObject.transform.parent = null;
                previewObject = null;
                mode = buildMode.NONE;
            }
        }
    }

    void EnterTransformMode()
    {
        if (Input.GetKeyDown(TRANSFORM_BLOCK))
        {
            Debug.Log("entering transform mode");
            //should have some sort of UI indication to player
            mode = buildMode.TRANSFORM;
        }
    }

    void ExitTransformMode()
    {
        if (Input.GetKeyDown(TRANSFORM_BLOCK))
        {
            Debug.Log("exiting transform mode");
            mode = buildMode.PREVIEW;
        }
    }

    void TransformObject()
    {
        //lock camera
        //take mouse input
        //take scroll wheel input
        //have some sort of "reset to default" key
    }

    void Update()
    {
        // Handle entering preview mode
        switch (mode)
        {
            case buildMode.NONE:
                EnterPreviewMode();
                return;

            case buildMode.PREVIEW:
                ExitPreviewMode();
                EnterTransformMode();
                BuildObject();
                return;
            case buildMode.TRANSFORM:
                ExitTransformMode();
                TransformObject();
                return;
        }
        
        // Handle exit preview, scaling, and rotation
    }
}
