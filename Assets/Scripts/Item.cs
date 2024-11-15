using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject
{
    public Sprite image;
    public ItemType type;
    public GameObject prefabObject;
    public Vector3Int placeRange = new Vector3Int(3, 3, 3);
    public int stackCount = 1;
}

public enum ItemType
{
    Log,
    Rock,
    Plant
}
