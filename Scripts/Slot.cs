using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Slot: MonoBehaviour
{
    public Sprite emptyVisual;
    public Page suppliesPage;
    public ItemData slotItem;

    public GameObject gameManager;
    public ItemsManager itemManager;
    public TeamManager teamManager;
    public MapManager mapManager;

    public Member slotMember;
    public Base slotBase;
    public bool isHealKitSlot;
    public bool isFoodSlot;
    public bool isDrinkSlot;
    public bool isCharacterSlot;
    public bool isPickUp;
    public Image image;

    void Awake()
    {
        itemManager = FindObjectOfType<ItemsManager>();
        teamManager = FindObjectOfType<TeamManager>();
        mapManager = FindObjectOfType<MapManager>();
    }

    void Start()
    {
        itemManager = FindObjectOfType<ItemsManager>();
        teamManager = FindObjectOfType<TeamManager>();
        mapManager = FindObjectOfType<MapManager>();
    }

    public void OnSlotClick()
    {
        for(int i = 0; i < mapManager.selectionSlots.Count; i++)
        {
            if(this == mapManager.selectionSlots[i])
            {
                Debug.Log("Oura on a trouvé l ebon mais alors quel est le pb ?   " + slotMember);
                
                teamManager.OnSelectionClick(slotMember);
            }
        }

        if(slotItem != null)
        {
            if(slotMember != null)
            {
                Base.InventoryItem existingItem = teamManager.selectedBase.itemsInBase.Find(item => item.itemData == slotItem);

                if(teamManager.selectedBase.itemsInBase.Contains(existingItem))
                {
                    itemManager.OnUse(slotMember, slotItem);
                }
                else
                {
                    Debug.Log("Votre inventaire actuel ne contient pas " + slotItem);
                }
                
            }
        }

        if(slotBase != null)
        {
            // teamManager.OnLeaderClick();
        }

        if(slotMember != null)
        {
            
            if(slotMember.isPickUp)
            {

                // Base teamBase = transform.GetComponent<Base>();
                if(teamManager.selectedBase != null)
                {
                    Debug.Log("teamManager.selected Base est pas null");
                    teamManager.AddMember(teamManager.selectedBase, slotMember);
                    teamManager.OnPickUpClick();
                }
                else
                {
                    Debug.Log("teamManager.selected Base est null");
                }

            }
            else
            {
                // teamManager.OnSelectionClick(slotMember);
            }
        }
    }
}

