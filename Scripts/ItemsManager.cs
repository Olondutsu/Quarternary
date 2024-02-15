using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{

    public int healKit;
    public int food;
    public int drink;
    public int money;
    public List<ItemData> inventoryItems = new List<ItemData>();

    public void AddItem(ItemData addItem)
    {
        inventoryItems.Add(addItem);
    }

    public void RemoveItem(ItemData rmvItem)
    {
        inventoryItems.Remove(rmvItem);
    }

    public void DisplayItemsVisual()
    {
        foreach(ItemData item in inventoryItems)
        {
            item.visual.SetActive(true);
        }
    }
    
}
