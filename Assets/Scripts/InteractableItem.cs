using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    public Item item; // The item data to be associated with this GameObject
    public bool isCollected = false; // To track if the item has already been picked up

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is in range and presses the "E" key
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && !isCollected)
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        // Add the item to the player's inventory
        ItemDemo itemDemo = FindObjectOfType<ItemDemo>();
        if (itemDemo != null && itemDemo.inventoryManager != null)
        {
            bool addedToInventory = itemDemo.inventoryManager.AddItem(item);
            if (addedToInventory)
            {
                Debug.Log($"{item.name} added to inventory.");
                // isCollected = true; // Mark as collected to prevent future pickups
                // gameObject.SetActive(false); // Hide the item from the scene
            }
            else
            {
                Debug.Log("Inventory is full! Could not pick up the item.");
            }
        }
    }
}
