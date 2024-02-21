using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Animations;

public class DisplayJournal : MonoBehaviour
{
    public EventGenerator eventGenerator;
    public TeamManager teamManager;
    public TimeManager timeManager;
    public ItemsManager itemsManager;
    public MapManager mapManager;

    public GameObject journal;
    public Event rnEvent;

    public ItemData foodItemData;
    public ItemData drinkItemData;
    public ItemData healKitItemData;

    public Slot isHealSlot;
    public Slot isFoodSlot;
    public Slot isDrinkSlot;
    public Slot isCharacterSlot;

    public GameObject parentGameObject;
    public GameObject memberPrefab;

    public Page eventPage;
    public Page needsPage;
    public Page suppliesPage;
    public Page mapPage;

    public int pastIndex = 0;
    public int journalIndex = 0;
    public bool eventDisplayed =false;
    public bool journalDisplayed = false;
    public bool teamInfosPageDisplayed = false;
    public bool suppliesPageDisplayed = false;
    public bool readytoNextDay = false;


    void Start()
    {
        NewDay();
        TeamPopulate();

        // PopulatePages();
    }

    public void OnOpenClick()
    {
        journalDisplayed = !journalDisplayed;
        eventPage.pageVisual.SetActive(true);
        journal.SetActive(journalDisplayed);
    }

    public void OnNextPageButtonClick()
    {
        if(journalDisplayed)
        {
            Debug.Log("journalDisplayed");
            AddIndex();
        }

        if(journalIndex == 1)  
        {
            Debug.Log("journalIndex == 1");
            eventPage.pageVisual.SetActive(true);
            eventPage.CheckButtons();
            AddIndex();
        }

        if(journalIndex == 2)
        {
            Debug.Log("journalIndex == 2");
            needsPage.pageVisual.SetActive(true);
            needsPage.CheckButtons();
            AddIndex();
        }

        if(journalIndex == 3)
        {
            Debug.Log("journalIndex == 3");
            suppliesPage.pageVisual.SetActive(true);
            suppliesPage.CheckButtons();
            AddIndex();
        }

        if(journalIndex == 3 && readytoNextDay)
        {
            Debug.Log("journalDisplayed && teamInfosPageDisplayed && suppliesPageDisplayed && readytoNextDay");
        }
    }
    
    public void AddIndex()
    {
        if(pastIndex == journalIndex)
        {
            journalIndex++;
        }
    }

    public void NewDay()
    {
        Debug.Log("NewDayCalled");
        journalDisplayed = true;
        PopulatePages();
    }

    public void ResetJournal()
    {
        readytoNextDay = false;
        journalDisplayed = false;
        teamInfosPageDisplayed = false;
        suppliesPageDisplayed = false;
        eventDisplayed = false;
        journalIndex = 0;
        pastIndex = 0;

        suppliesPage.pageVisual.SetActive(false);
        needsPage.pageVisual.SetActive(false);
        eventPage.pageVisual.SetActive(false);

        Debug.Log("Reset du Journal");
        suppliesPage.pageHead.text = "\n";
        suppliesPage.pageBody.text =  "\n";

        needsPage.pageHead.text = "\n";
        needsPage.pageBody.text =  "\n";

        eventPage.pageHead.text = "\n";
        eventPage.pageBody.text =  "\n";

        timeManager.NextDay();
        NewDay();
    }

    public void PopulatePages()
    {
        Debug.Log("PagePopulated");
        
        PopulateEventPage();
        PopulateNeedsPage();
        PopulateSuppliesPage();
    }

    void TeamPopulate()
    {
        int xOffset = 0;

        foreach(ItemData item in itemsManager.inventoryItems)
        {
            if(item.isSupplie)
            {
                if(item.isFood)
                {
                    foodItemData = item;
                }
                if(item.isDrink)
                {
                    drinkItemData = item;
                }
                if(item.isHealKit)
                {
                    healKitItemData = item;
                }
            }
        }
        foreach(Member member in teamManager.teamMembers)
        {
            GameObject go;
            xOffset += 3;
            go = Instantiate(memberPrefab, (parentGameObject.transform)) as GameObject;
            // xOffset += 3;
            // Vector3 spawnPosition = parentGameObject.transform.position + new Vector3(xOffset, 0, 0); 
            // go = Instantiate(memberPrefab, spawnPosition, Quaternion.identity, parentGameObject.transform) as GameObject; 

            // go.transform.parent = parentGameObject.transform;
            go.transform.SetParent(parentGameObject.transform);

            foreach(Transform child in parentGameObject.transform)
            {
                foreach(Transform slotParents in child)
                {
                    foreach(Transform slotChild in slotParents)
                    {
                        Slot slotScript = slotChild.GetComponent<Slot>();
                        
                        if(slotScript != null)
                        {
                            if (slotScript.isCharacterSlot)
                            {
                                isCharacterSlot = slotScript;
                                isCharacterSlot.image.sprite = member.journalVisual;
                                slotScript.slotMember = member;
                            }
                        }
                        else
                        {
                        }
                    }
                }
            }
        }
           
        foreach (Transform child in parentGameObject.transform)
        {
            foreach(Transform slotParent in child)
            {
                foreach(Transform slotChild in slotParent)
                {
                    Slot slot = slotChild.GetComponent<Slot>();
                    if (slot != null)
                    {
                        if (slot.isHealKitSlot)
                        {
                            isHealSlot = slot;
                            if(itemsManager.healKit >= 1)
                            {
                                slot.image.sprite= healKitItemData.journalVisualAvailable;
                            }
                            else
                            {
                                slot.image.sprite = healKitItemData.journalVisualUnAvailable;
                            }
                        }

                        if (slot.isFoodSlot)
                        {
                            isFoodSlot = slot;
                            if(itemsManager.food >= 1)
                            {
                                slot.image.sprite = foodItemData.journalVisualAvailable;
                            }
                            else
                            {
                                slot.image.sprite = foodItemData.journalVisualUnAvailable;
                            }
                        }
                                    
                        if (slot.isDrinkSlot)
                        {
                            isDrinkSlot = slot;
                            if(itemsManager.drink >= 1)
                            {
                                slot.image.sprite = drinkItemData.journalVisualAvailable;
                            }

                            else
                            {
                                slot.image.sprite = drinkItemData.journalVisualUnAvailable;
                            }
                        }
                    }  
                    else
                    {
                    }
                }
            }
        }
        // PreHeatSuppliePage();
    }

    void PopulateMapPage()
    {
        // Afficher les cases, les membres de la team aussi, faire en sortee que tout soit cliquable mais ça se fera sur le map Manager
    }

    void AddSupplieSlot(Member member)
    {

    }

    void PopulateSuppliesPage() 
    {
        suppliesPage.CheckButtons();

        suppliesPage.pageHead.text += "DAY " + timeManager.currentDay + ":\n";

    }

    void PopulateNeedsPage() 
    {
        needsPage.CheckButtons();
        needsPage.pageHead.text += "DAY " + timeManager.currentDay + ":\n";

        foreach(Member member in teamManager.teamMembers)
        {
            if(member.isInTeam)
            {
                // FOOD
                if(member.hunger >= 5)
                {
                    needsPage.pageBody.text += member.name + "is fully feed.";
                }
                if(member.hunger < 5 && member.hunger >= 3)
                {
                    needsPage.pageBody.text += member.name + "would not mind to eat something.";
                }
                if(member.hunger < 3 && member.hunger >1)
                {
                    needsPage.pageBody.text += member.name + "begin to be hungry.";
                }
                if(member.hunger == 1)
                {
                    needsPage.pageBody.text += member.name + "will die if he don't eat.";
                }
                
                if(member.hunger == 0)
                {
                    needsPage.pageBody.text += member.name + "died from hunger.";
                }

                // DRINK
                if(member.thirst > 5)
                {
                    needsPage.pageBody.text += member.name + "is full of water.";
                }
                if(member.thirst < 5 && member.hunger > 3)
                {
                    needsPage.pageBody.text += member.name + "would not mind to drink a little something.";
                }
                if(member.thirst < 3 && member.hunger >1)
                {
                    needsPage.pageBody.text += member.name + "begin to be really thirsty.";
                }
                if(member.thirst == 1)
                {
                    needsPage.pageBody.text += member.name + "will die if he don't drink.";
                }
                if(member.thirst == 0)
                {
                    needsPage.pageBody.text += member.name + "died from thirst.";
                }
                
                // PHYSICALHEALTH
                if(member.physicalHealth == 2)
                {
                    needsPage.pageBody.text += member.name + "is in perfect health";
                }
                if(member.physicalHealth == 1)
                {
                    needsPage.pageBody.text += member.name + "begin to be in a critical health";
                }
                if(member.physicalHealth == 0)
                {
                    needsPage.pageBody.text += member.name + "died from his physical health problems";
                }

                // MENTALHEALTH
                if(member.mentalHealth == 2)
                {
                    needsPage.pageBody.text += member.name + "is in perfect health";
                }
                if(member.mentalHealth == 1)
                {
                    needsPage.pageBody.text += member.name + "begin to be in a critical mental health problems";
                }
                if(member.mentalHealth == 0)
                {
                    needsPage.pageBody.text += member.name + "died due to mental illness";
                }
            }
        }
    }

    void PopulateEventPage()
    {
        // eventGenerator.EventEnabler();
        eventGenerator.RandomizeEvent();
        // retrait du Page.CheckButton()

        // eventGenerator.currentEvent.completed = true;
        // eventGenerator.pastEvent = eventGenerator.currentEvent;
        eventPage.pageHead.text += "DAY " + timeManager.currentDay + ":\n";
        
        rnEvent = eventGenerator.currentEvent;
        eventPage.pageBody.text += rnEvent.description;
    }

    public void ApplyAction()
    {
        
    }
}

