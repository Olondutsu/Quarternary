using UnityEngine;
using UnityEngine.UI;

public class Slot: MonoBehaviour
{
    public GameObject emptyVisual;
    public Page suppliesPage;
    public ItemData slotItem;
    public ItemsManager itemManager;

    void Start()
    {

    }

    void OnSlotClick(ItemData slotItem)
    {
        itemManager.OnUse(slotItem);
    }

}

