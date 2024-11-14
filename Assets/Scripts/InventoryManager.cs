using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    public GameObject itemSlotBlueprint;

    public void AddItem(Item item)
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            ItemSlot slot = itemSlots[i];
            Debug.Log(slot);
            Item itemInSlot = slot.item;
            if(itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return;
            }
        }
    }

    void SpawnNewItem(Item item, ItemSlot slot)
    {
        GameObject newItem = Instantiate(itemSlotBlueprint, slot.transform);
        ItemSlot itemSlot = newItem.GetComponent<ItemSlot>();
        itemSlot.InitialiseItem(item);
    }
}
