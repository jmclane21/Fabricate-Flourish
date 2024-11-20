using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFoliage : MonoBehaviour
{
    private FoliageManager foliageManager;
    private InventoryManager inventoryManager;

    private void Start()
    {
        foliageManager = FindObjectOfType<FoliageManager>();
        inventoryManager = InventoryManager.Instance;

        if (foliageManager == null)
        {
            Debug.LogError("FoliageManager not found in the scene.");
        }

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager not found in the scene.");
        }
    }

    private void Update()
    {
        // Right-click to spawn foliage (right-click is MouseButton 1)
        if (Input.GetMouseButtonDown(1))  // 1 is the right mouse button
        {
            // Check if there are items in the inventory before spawning foliage
            if (HasItemsInInventory())
            {
                if (foliageManager != null)
                {
                    // Spawn foliage near the player
                    foliageManager.SpawnFoliageNearPlayer();
                }
            }
            else
            {
                Debug.LogWarning("No items in inventory to spawn foliage.");
            }
        }

        // Left-click for inventory actions (can be expanded as needed)
        if (Input.GetMouseButtonDown(0))  // 0 is the left mouse button
        {
            // Optionally, handle other inventory-related actions, like using the selected item
            UseSelectedItem();
        }
    }

    private bool HasItemsInInventory()
    {
        // Iterate through inventory slots and check if any slot has items
        foreach (var slot in inventoryManager.inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.count > 0)
            {
                return true;  // There are items in the inventory
            }
        }
        return false;  // No items in the inventory
    }

    private void UseSelectedItem()
    {
        // Use the item from the selected inventory slot
        Item selectedItem = inventoryManager.getSelectedItem(true); // Pass 'true' to use the item
        if (selectedItem != null)
        {
            Debug.Log($"Using item: {selectedItem.name}");
        }
    }
}
