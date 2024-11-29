using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    const KeyCode PLACE_BLOCK = KeyCode.Mouse1;
    const KeyCode DESTROY_BLOCK = KeyCode.Mouse0;
    const KeyCode EXIT = KeyCode.Tab;
    const KeyCode TRANSFORM_BLOCK = KeyCode.LeftShift;

    enum buildMode
    {
        PREVIEW,
        NONE,
        TRANSFORM
    }

    buildMode mode = buildMode.NONE;

    [HideInInspector] public Item itemToPlace;
    [HideInInspector] GameObject previewObject;
    public Material previewMaterial;
    public Camera mainCamera;

    public float gridSize = 1f; // Size of the snapping grid

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
                    float distance = gameObject.transform.position.magnitude - hit.point.magnitude;
                    if (distance < itemToPlace.placeRange.magnitude)
                    {
                        PreviewObject(hit.point, hit.normal);
                    }
                }
            }
        }
    }

    void ExitPreviewMode()
    {
        if (Input.GetKeyDown(EXIT))
        {
            itemToPlace = null;
            Destroy(previewObject);
            previewObject = null;
            mode = buildMode.NONE;
        }
    }

    void setPreviewObjectTransparent()
    {
        Renderer renderer;
        renderer = previewObject.GetComponent<Renderer>();

        Material[] materials = new Material[renderer.materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = previewMaterial;
        }
        renderer.materials = materials;
    }

    void PreviewObject(Vector3 spawnPosition, Vector3 surfaceNormal)
    {
        itemToPlace = InventoryManager.Instance.getSelectedItem(false);

        if (previewObject == null)
        {
            previewObject = Instantiate(itemToPlace.prefabObject, spawnPosition, Quaternion.identity, mainCamera.transform);
        }
        previewObject.GetComponent<BoxCollider>().enabled = false;
        setPreviewObjectTransparent();
        

        // Snap the preview object to the grid or surface
        Vector3 snappedPosition = GetSnappedPosition(spawnPosition, surfaceNormal);
        previewObject.transform.position = snappedPosition;
        previewObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
    }

    void BuildObject()
    {

        if (Input.GetKeyDown(PLACE_BLOCK) && previewObject != null)
        {
            Debug.Log("Right-click detected, placing object at position: " + previewObject.transform.position);
            Debug.Log("Rotation: " + previewObject.transform.rotation.eulerAngles);

            if (InventoryManager.Instance.selectedSlot >= 0)
            {
                InventoryManager.Instance.getSelectedItem(true);

                GameObject placedObject = Instantiate(itemToPlace.prefabObject, previewObject.transform.position, previewObject.transform.rotation);
                placedObject.transform.localScale = previewObject.transform.localScale;
                Destroy(previewObject);
                previewObject = null;
                mode = buildMode.NONE;
            }
        }

    }

    Vector3 GetSnappedPosition(Vector3 position, Vector3 normal)
    {
        // Snap to grid
        Vector3 gridPosition = new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            Mathf.Round(position.z / gridSize) * gridSize
        );

        // Optional: Add logic to align to surface normal if needed
        // For grid snapping, this line can be omitted.
        return gridPosition;
    }

    void EnterTransformMode()
    {
        if (Input.GetKeyDown(TRANSFORM_BLOCK))
        {
            Debug.Log("entering transform mode");
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
    if (previewObject == null) return;

    // Rotation step in degrees
    float rotationStep = 15f; // Adjust the value for finer or coarser rotation steps

    // Check for the 'R' key to rotate the log upright
    if (Input.GetKeyDown(KeyCode.R))
    {
        // Make the log stand upright (rotating to 90 degrees around the x-axis)
        previewObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        Debug.Log("Log rotated to stand upright.");
    }

    // rotate object with q/e around z axis
    if (Input.GetKeyDown(KeyCode.Q)) // Right-click and drag to rotate
    {
        previewObject.transform.Rotate(new Vector3(0, 0, rotationStep));
    }
    if (Input.GetKeyDown(KeyCode.E)) // Right-click and drag to rotate
    {
        previewObject.transform.Rotate(new Vector3(0, 0, -rotationStep));
    }

        // Optional: Scale object with scroll wheel
        float scaleSpeed = 0.1f; // Adjust for sensitivity
    float scroll = Input.GetAxis("Mouse ScrollWheel");
    if (scroll != 0f)
    {
        Vector3 currentScale = previewObject.transform.localScale;
        Vector3 newScale = currentScale + Vector3.one * scroll * scaleSpeed;

        // Prevent the scale from going too small or too large
        newScale = Vector3.Max(newScale, Vector3.one * 0.1f); // Minimum scale
        newScale = Vector3.Min(newScale, Vector3.one * 5f);   // Maximum scale

        previewObject.transform.localScale = newScale;
    }
}




    void Update()
    {
        switch (mode)
        {
            case buildMode.NONE:
                EnterPreviewMode();
                return;

            case buildMode.PREVIEW:
                ExitPreviewMode();
                EnterTransformMode();

                // Continuously update the preview position
                if (previewObject != null)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    Ray ray = mainCamera.ScreenPointToRay(mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 snappedPosition = GetSnappedPosition(hit.point, hit.normal);
                        previewObject.transform.position = snappedPosition;
                        previewObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    }
                }

                BuildObject();
                return;

            case buildMode.TRANSFORM:
                ExitTransformMode();
                TransformObject();
                BuildObject();
                return;
        }
    }
}
