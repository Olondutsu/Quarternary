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
    public TeamManager teamManager;

    public void AddItem(ItemData addItem)
    {
        inventoryItems.Add(addItem);
    }

    public void RemoveItem(ItemData rmvItem)
    {
        inventoryItems.Remove(rmvItem);
    }
    public void InitializeItems()
    {
        foreach(ItemData item in inventoryItems)
        {
            // Histoire de dire que les Items dans les Int sont les meme que ceux dans inventoryItems;
        }
    }

    public void DisplayItemsVisual()
    {
        foreach(ItemData item in inventoryItems)
        {
            //
        }
    }
    
    public void OnUse(ItemData usedItem)
    {
        foreach(Member teamMember in teamManager.teamMembers)
        {
            foreach(ItemData item in inventoryItems)
            {
                if(item.isFood)
                {
                    teamManager.FeedMember(teamMember, teamManager.feedRate);
                }

                if(item.isDrink)
                { 
                    teamManager.UnthirstMember(teamMember, teamManager.drinkRate);
                }

                if(item.isHealKit)
                {
                    teamManager.HealMember(teamMember);
                }
            }
        }
    }
}
