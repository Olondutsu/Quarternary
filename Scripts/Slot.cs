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
    }

    void Start()
    {
        itemManager = FindObjectOfType<ItemsManager>();
        teamManager = FindObjectOfType<TeamManager>();
    }

    public void OnSlotClick()
    {
        if(slotItem != null)
        {
            if(slotMember != null)
            {
                itemManager.OnUse(slotMember, slotItem);
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
                teamManager.OnSelectionClick(slotMember);
            }
        }
    }
}

