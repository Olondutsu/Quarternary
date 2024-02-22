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
            itemManager.OnUse(slotItem);
        }

        if(slotBase != null)
        {
            // teamManager.OnLeaderClick();
        }

        if(slotMember != null)
        {
            if(slotMember.isPickUp)
            {
                foreach(Member proposedMember in teamManager.proposedMembers)
                {
                    foreach(Base aBase in teamManager.bases)
                    {
                        
                    }
                    foreach(Transform child in teamManager.parentDisplayBases.transform)
                    {
                        Base teamBase = child.GetComponent<Base>();

                        teamManager.AddMember(teamBase, proposedMember);
                        teamManager.RandomizeLeader();
                        break;
                    }
                }
            }
            else
            {
                teamManager.OnSelectionClick(slotMember);
            }
        }
    }
}

