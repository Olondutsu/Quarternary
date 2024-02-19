using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Slot: MonoBehaviour
{
    public Sprite emptyVisual;
    public Page suppliesPage;
    public ItemData slotItem;
    public ItemsManager itemManager;
    public Member slotMember;
    public bool isHealKitSlot;
    public bool isFoodSlot;
    public bool isDrinkSlot;
    public bool isCharacterSlot;
    public Image image;


    void Start()
    {

    }

    void OnSlotClick(ItemData slotItem)
    {
        itemManager.OnUse(slotItem);
    }

}

