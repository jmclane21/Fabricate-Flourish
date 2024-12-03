using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDemo : MonoBehaviour
{
    public InventoryManager inventoryManager;

    // Tracks items within range
    private Dictionary<int, Item> nearbyItems = new Dictionary<int, Item>();
    private Dictionary<int, GameObject> itemObjects = new Dictionary<int, GameObject>();
    private int currentItemId = 0;

    public void PickupItem(int id)
    {
        if (nearbyItems.ContainsKey(id))
        {
            Item item = nearbyItems[id];
            bool result = inventoryManager.AddItem(item);

            if (result)
            {
                Debug.Log($"Item added to inventory: {item.name}");
                GameObject itemObject = itemObjects[id];
                nearbyItems.Remove(id);
                itemObjects.Remove(id);

                // // Optional: Disable further interaction
                // Collider itemCollider = itemObject.GetComponent<Collider>();
                // if (itemCollider != null)
                // {
                //     itemCollider.enabled = false;
                // }
                // Renderer renderer = itemObject.GetComponent<Renderer>();
                // if (renderer != null)
                // {
                //     renderer.material.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent
                // }
            }
            else
            {
                Debug.Log("Item not added, inventory full");
            }
        }
        else
        {
            Debug.LogWarning("No item to pick up with this ID!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Debug.Log($"E key pressed, currentItemId: {currentItemId}");
            if (currentItemId > 0)
            {
                PickupItem(currentItemId);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called with {other.gameObject.name}");
        InteractableItem interactable = other.GetComponent<InteractableItem>();
        if (interactable != null && !interactable.isCollected)
        {
            currentItemId++;
            Debug.Log($"Collider detected: {other.gameObject.name}, assigned ID: {currentItemId}");
            nearbyItems[currentItemId] = interactable.item;
            itemObjects[currentItemId] = other.gameObject;
        }
        else
        {
            Debug.Log($"OnTriggerEnter: {other.gameObject.name} is not interactable.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var kvp in itemObjects)
        {
            if (kvp.Value == other.gameObject)
            {
                Debug.Log($"OnTriggerExit called for {other.gameObject.name}, removing ID: {kvp.Key}");
                nearbyItems.Remove(kvp.Key);
                itemObjects.Remove(kvp.Key);
                break;
            }
        }
    }
}
