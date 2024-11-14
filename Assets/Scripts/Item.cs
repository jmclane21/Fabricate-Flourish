using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    public Vector3Int placeRange = new Vector3Int(3, 3, 3);
    public bool stackable = true;
}

public enum ItemType
{
    Log,
    Rock,
    Plant
}
