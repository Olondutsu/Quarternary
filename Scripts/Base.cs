using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    MapCase baseCase;
    public List<Member> membersInBase = new List<Member>(); 
    public List<Member> membersInTravel = new List<Member>();
    public List<Member> selectedMembers = new List<Member>();
    
    public ItemData[] itemData;
    public List<InventoryItem> itemsInBase = new List<InventoryItem>();
    public List<Page> pagesForBase = new List<Page>();
    public List<GameObject> pagePrefab = new List<GameObject>();

    public Member squadLeader;
    public MapCase clickedCase;
    public bool journalLoaded;
    public bool isSelected;
    public bool available;
    public bool displayed = false;
    public bool isFull = false;

    [System.Serializable]
    public class InventoryItem
    {
        public ItemData itemData;
        public int quantity;

        public InventoryItem(ItemData itemData, int quantity)
        {
            this.itemData = itemData;
            this.quantity = quantity;
        }
    }
}