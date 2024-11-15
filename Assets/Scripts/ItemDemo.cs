using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDemo : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (result == true)
        {
            Debug.Log("Item added");
        } else
        {
            Debug.Log("Item not added, inventory full");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PickupItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PickupItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            PickupItem(2);
        }
    }
}
