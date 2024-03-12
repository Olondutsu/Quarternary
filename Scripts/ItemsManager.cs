using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{

    public Transform[] foodVisualPositions;
    public Transform[] drinkVisualPositions;
    public GameObject foodVisualPrefab;
    public GameObject drinkVisualPrefab;
    public GameObject healKitVisual;

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
        int foodCount = 0;
        int drinkCount = 0;
        int rifleCount = 0;
        int foodTransformCount = 0;
        int drinkTransformCount = 0;
        // Crer une genre de liste de transform avec tous les items existants, ajouter a tous un sprite
        foreach(ItemData item in selectedBase.itemsInBase)
        {
            if(item.isFood)
            {
                foodCount++;
            }
            if(item.isDrink)
            {
                drinkCount++;
            }
            if(item.isHealKit)
            {
                healKitVisual.SetActive(true);
            }
            if(item.name == "Rifle")
            {  
                rifleCount++;
            }
        }

        foreach(Transform transform in foodVisualPositions)
        {
            foodTransformCount++;
        }

        foreach(Transform transform in drinkVisualPositions)
        {
            drinkTransformCount++;
        }

        if(foodCount > foodTransformCount)
        {
            foodCount = foodTransformCount;
        }

        if(drinkCount > drinkTransformCount)
        {
            drinkCount = drinkTransformCount;
        }

        for(int i = 0; i < foodCount; i++)
        {
            GameObject visual = Instantiate(foodVisualPrefab, foodVisualPositions[i].position, Quaternion.identity);
            visual.transform.parent = foodVisualPositions[i];
        }

        for(int i = 0; i < drinkCount; i++)
        {
            GameObject visual = Instantiate(drinkVisualPrefab, drinkVisualPositions[i].position, Quaternion.identity);
            visual.transform.parent = drinkVisualPositions[i];
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