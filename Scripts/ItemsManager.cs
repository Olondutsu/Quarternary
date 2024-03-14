using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{

    public List<ItemData>displayedItems = new List<ItemData>();
    public Transform[] foodVisualPositions;
    public Transform[] drinkVisualPositions;
    public Transform[] rifleVisualPositions;
    public GameObject foodVisualPrefab;
    public GameObject drinkVisualPrefab;
    public GameObject rifleVisualPrefab;
    public GameObject healKitVisual;
    public GameObject axeVisual;
    public GameObject shovelVisual;
    public GameObject radioVisual;

    public int healKit;
    public int food;
    public int drink;
    public int money;

    public Base selectedBase;
    public TeamManager teamManager;

    public void AddItem(Base aBase, ItemData addItem, int amount)
    { 
        Debug.Log("AddItem appelé pour " + addItem + "Amount : " + amount);
        Base.InventoryItem existingItem = aBase.itemsInBase.Find(item => item.itemData == addItem);

        if (existingItem != null)
        {
            // Si l'élément existe déjà, augmentez simplement sa quantité
            existingItem.quantity += amount;
        }
        else
        {
            aBase.itemsInBase.Add(new Base.InventoryItem(addItem, amount));
        }
        DisplayItemsVisual();
    }

    public void RemoveItem(Base aBase, ItemData rmvItem, int amount)
    {
        Base.InventoryItem existingItem = aBase.itemsInBase.Find(item => item.itemData == rmvItem);

        if(existingItem.quantity > 1)
        {
            existingItem.quantity -= amount;
        }
        else
        {
            aBase.itemsInBase.Remove(existingItem);
        }
        DisplayItemsVisual();
    }

    public void DisplayItemsVisual()
    {
        // Base.InventoryItem existingItem = selectedBase.itemsInBase.Find(item => item.itemData == addItem);

        int foodTransformCount = 0;
        int drinkTransformCount = 0;
        int rifleTransformCount = 0;
        
        
        foreach(Transform transform in foodVisualPositions)
        {
            foodTransformCount++;
        }

        foreach(Transform transform in drinkVisualPositions)
        {
            drinkTransformCount++;
        }

        foreach(Transform transform in rifleVisualPositions)
        {
            rifleTransformCount++;
        }

        // foreach(Base.InventoryItem inventoryItem in selectedBase.itemsInBase)
        // {
            for(int y = 0; y < selectedBase.itemsInBase.Count; y++)
            {

            Base.InventoryItem inventoryItem = selectedBase.itemsInBase[y];
            Debug.Log("recuperation des inventoryItem " + inventoryItem + inventoryItem.itemData);
            ItemData item = inventoryItem.itemData;

            if(item.name == "Rifle")
            {
                for(int i = 0; inventoryItem.quantity > i; i++)
                {   
                    Debug.Log("for int i = à ; i < inventoryItem.quantity" + item.name);
                    if(inventoryItem.quantity < rifleTransformCount)
                    {
                        if(rifleVisualPositions[i].transform.childCount == 0)
                        {
                            GameObject visual = Instantiate(rifleVisualPrefab, rifleVisualPositions[i].position, Quaternion.identity);
                            visual.transform.parent = rifleVisualPositions[i]; 
                        }

                    }
                }
            }
            if(item.name == "Food")
            {
                for(int i = 0; i < inventoryItem.quantity; i++)
                {
                     Debug.Log("for int i = à ; i < inventoryItem.quantity" + item.name);
                    if(inventoryItem.quantity < foodTransformCount)
                    {
                        if(foodVisualPositions[i].transform.childCount == 0)
                        {
                            GameObject visual = Instantiate(foodVisualPrefab, foodVisualPositions[i].position, Quaternion.identity);
                            visual.transform.parent = foodVisualPositions[i];
                        }
                    }
                }
            }
            
            if(item.name == "Water")
            {
                for(int i = 0; i < inventoryItem.quantity; i++)
                {
                    Debug.Log("for int i = à ; i < inventoryItem.quantity" + item.name);
                    if(inventoryItem.quantity < drinkTransformCount)
                    {
                        if(drinkVisualPositions[i].transform.childCount == 0)
                        {
                            GameObject visual = Instantiate(drinkVisualPrefab, drinkVisualPositions[i].position, Quaternion.identity);
                            visual.transform.parent = drinkVisualPositions[i];
                        }
                    }
                }
            }

            if(item.isHealKit)
            {
                healKitVisual.SetActive(true);
            }

            if(item.name == "Shovel")
            {  
                shovelVisual.SetActive(true);
            }

            if(item.name == "Axe")
            {  
                axeVisual.SetActive(true);
            }
            if(item.name == "Radio")
            {  
                radioVisual.SetActive(true);
            }
            }
        // }
    }

    
    public void ItemCount()
    {
        foreach(Base.InventoryItem item in selectedBase.itemsInBase)
        {
            if(item.itemData.isFood)
            {
                food++;
            }

            if(item.itemData.isDrink)
            {
               drink++; 
            }
            
            if(item.itemData.isHealKit)
            {
                healKit++;
            }
        }
    }

    // ajout d'un  Member ?,
    public void OnUse(Member teamMember, ItemData usedItem)
    {
        Base.InventoryItem existingItem = selectedBase.itemsInBase.Find(item => item.itemData == usedItem);

        if(usedItem.isFood)
        {
            teamManager.FeedMember(teamMember, teamManager.feedRate);
            RemoveItem(selectedBase, usedItem, 1);
            ItemCount();
        }

        if(usedItem.isDrink)
        { 
            teamManager.UnthirstMember(teamMember, teamManager.drinkRate);
            RemoveItem(selectedBase, usedItem, 1);
            ItemCount();
        }

        if(usedItem.isHealKit)
        {
            teamManager.HealMember(teamMember);
            RemoveItem(selectedBase, usedItem, 1);
            ItemCount();
        }
    }
}