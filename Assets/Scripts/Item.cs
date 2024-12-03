using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public Sprite image;             // Icon for the item in the inventory
    public ItemType type;            // Type of item (e.g., Log, Rock, Plant)
    public GameObject prefabObject;  // Prefab for this item (in-world representation)
    public Vector3Int placeRange = new Vector3Int(3, 3, 3); // Range where the item can be placed
    public int stackCount = 1;       // Max stack count for this item
}

public enum ItemType
{
    Log,
    Rock,
    Plant
}
