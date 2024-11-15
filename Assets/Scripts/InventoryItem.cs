using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour
{
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;

    public Image image;
    public TMPro.TMP_Text countText;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
    }
}
