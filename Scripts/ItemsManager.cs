using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{

    public int healKit;
    public int food;
    public int drink;
    public int money;

    public Base selectedBase;
    public TeamManager teamManager;

    public void AddItem(Base aBase, ItemData addItem, int amount)
    { 
        for(int i = 1; i < amount; i++)
        {
            aBase.itemsInBase.Add(addItem);
        }
        
        ItemCount();
    }

    public void RemoveItem(Base aBase, ItemData rmvItem, int amount)
    {
        aBase.itemsInBase.Remove(rmvItem);
        ItemCount();
    }

    public void DisplayItemsVisual()
    {
        foreach(ItemData item in selectedBase.itemsInBase)
        {
            item.gameVisual.SetActive(true);
        }
    }
    
    public void ItemCount()
    {
        foreach(ItemData item in selectedBase.itemsInBase)
        {
            if(item.isFood)
            {
                food++;
            }

            if(item.isDrink)
            {
               drink++; 
            }
            
            if(item.isHealKit)
            {
                healKit++;
            }
        }
    }

    // ajout d'un  Member ?,
    public void OnUse(Member teamMember, ItemData usedItem)
    {
        if(usedItem.isFood)
        {
            teamManager.FeedMember(teamMember, teamManager.feedRate);
            selectedBase.itemsInBase.Remove(usedItem);
            ItemCount();
        }

        if(usedItem.isDrink)
        { 
            teamManager.UnthirstMember(teamMember, teamManager.drinkRate);
            selectedBase.itemsInBase.Remove(usedItem);
            ItemCount();
        }

        if(usedItem.isHealKit)
        {
            teamManager.HealMember(teamMember);
            selectedBase.itemsInBase.Remove(usedItem);
            ItemCount();
        }
    }
}