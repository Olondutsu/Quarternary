using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using System.Collections.Generic;

public class DisplayJournal : MonoBehaviour
{
    public EventGenerator eventGenerator;
    public TeamManager teamManager;
    public TimeManager timeManager;
    public ItemsManager itemsManager;
    public MapManager mapManager;

    public GameObject journal;
    private Event rnEvent;

    public ItemData foodItemData;
    public ItemData drinkItemData;
    public ItemData healKitItemData;

    private Slot isHealSlot;
    private Slot isFoodSlot;
    private Slot isDrinkSlot;
    private Slot isCharacterSlot;

    public GameObject parentGameObject;
    public GameObject memberPrefab;
    public GameObject travelButtons;
    public GameObject parentPages;
    public GameObject pagePrefab;


    public Page eventPage;
    public Page needsPage;
    public Page suppliesPage;
    public Page mapPage;

    public Base selectedBase;

    public int maxCharPerPage = 250;
    public int pastIndex = 0;
    public int journalIndex = 0;
    public bool eventDisplayed = false;
    public bool journalDisplayed = false;
    public bool teamInfosPageDisplayed = false;
    public bool suppliesPageDisplayed = false;
    public bool readytoNextDay = false;


    void Start()
    {
        
        NewDay();
        // TeamPopulate();
        // PopulatePages();
    }

    public void ConfirmTravelClick()
    {
        travelButtons.SetActive(false);
        mapManager.OnConfirmTravel();
    }

    public void CancelTravelClick()
    {
        travelButtons.SetActive(false);
    }

    public void OnOpenClick()
    {

        if (selectedBase != null)
        {

            eventGenerator.RandomizeEvent();
            journalDisplayed = !journalDisplayed;
            eventPage.pageVisual.SetActive(true);
            journal.SetActive(journalDisplayed);
            
            foreach (Transform child in parentGameObject.transform)
            {
            PopulatePages();
            break;
            }
            
        }
    }

    //To instantiate pages/ REFONTE DU SYSTEME INITIAL NON UTILISE JUSQUA PRESENT
    public void InitializePages()
    {
        int eventIndex = 0;
        int needsIndex = 0;
        int supplieIndex = 0;
        int mapIndex = 0;

        // creer ou mettre à jour une liste;
        
        if(eventGenerator.currentEvent != null)
        {
            if(eventGenerator.currentEvent.description.Length >= maxCharPerPage)
            {
                int amountOfPages = 0;

                amountOfPages = eventGenerator.currentEvent.description.Length/maxCharPerPage;

                for(int i = 0; i > amountOfPages; i++ )
                {
                    GameObject go;
                    go = Instantiate(pagePrefab, (parentPages.transform)) as GameObject;
                    go.transform.SetParent(parentPages.transform);

                    Page goPage = go.GetComponent<Page>();
                    goPage.isEventPage = true;
                    eventIndex++;
                }
            }
        }
    }

    public void OnNextPageButtonClick()
    {
        if(eventGenerator.currentEvent != null)
        {
            if(eventGenerator.currentEvent.isEnd && !eventGenerator.currentEvent.completed)
            {
                eventGenerator.currentEvent.completed = true;
                eventGenerator.MemberHandler();
            }
        }

        // int eventIndex;
        // int needsIndex;
        // int supplieIndex;
        // int mapIndex;

        if(journalDisplayed)
        {
            Debug.Log("journalDisplayed");
            AddIndex();
        }

        if(journalIndex == 1)  
        {
            if(eventGenerator.currentEvent != null)
            {
                Debug.Log("journalIndex == 1");
                eventPage.pageVisual.SetActive(true);
                eventPage.CheckButtons();
                AddIndex();
            }
            else
            {
                AddIndex();
            }
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

        if(journalIndex == 4)
        {
            Debug.Log("journalIndex == 4");
            mapPage.pageVisual.SetActive(true);
            mapPage.CheckButtons();
        }

        if(journalIndex == 4 && readytoNextDay)
        {
            ApplyActions();
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
        // journalDisplayed = true;
        // PopulatePages();
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

        suppliesPage.pageHead.text = "\n";
        suppliesPage.pageBody.text =  "\n";

        needsPage.pageHead.text = "\n";
        needsPage.pageBody.text =  "\n";

        eventPage.pageHead.text = "\n";
        eventPage.pageBody.text =  "\n";

        if(!selectedBase.journalLoaded)
        {
            PopulatePages();
        }
    }

    public void PopulatePages()
    {   
        PopulateMapPage();
        PopulateEventPage();
        PopulateNeedsPage();
        PopulateSuppliesPage();
    }

     public void TeamPopulate()
    {
        List<Slot> slotList = new List<Slot>();

        int xOffset = 0;

        foreach(ItemData item in selectedBase.itemsInBase)
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

        foreach(Member member in selectedBase.membersInBase)
        {
            GameObject go;

            xOffset += 3;
            go = Instantiate(memberPrefab, (parentGameObject.transform)) as GameObject;

            go.transform.SetParent(parentGameObject.transform);

        }

        foreach(Transform child in parentGameObject.transform)
        {
            Debug.Log(child);

            Debug.Log("ForEach child de TeamPopulate, le premier forEach d'enfants");

            foreach(Transform slotParents in child)
            {
                Debug.Log("ForEach child de TeamPopulate, le second forEach d'enfants");
                Debug.Log(slotParents);
                foreach(Transform slotChild in slotParents)
                {
                    Debug.Log("ForEach child de TeamPopulate, le troisième foreach d'enfants");
                    Debug.Log(slotChild);
                    Slot slotScript = slotChild.GetComponent<Slot>();

                    if(slotScript != null)
                    {
                        slotList.Add(slotScript);
                        Debug.Log("Add " + slotScript + " To my list");
                    }
                }
            }
        }

        int slotInt = -1;

        foreach(Slot slot in slotList)
        {
            if(slot != null)
            {
                if (slot.isCharacterSlot)
                {
                    slotInt++;
                    Debug.Log("slotInt  = " + slotInt);

                    for(int i = 0; selectedBase.membersInBase.Count > i; i++ )
                    {
                            Debug.Log("ma condition chelou" + i );
                            Member membrous = selectedBase.membersInBase[slotInt];
                            Debug.Log("Membrous wesh : " + membrous);
                    }

                    foreach(Member member in selectedBase.membersInBase)
                    {
                        Debug.Log(member + "chaque member au milieu de mon truc ");
                        slot.image.sprite = member.journalVisual;
                        slot.slotMember = member;
                    }
                        Member slotMemb = selectedBase.membersInBase[slotInt];
                        
                        slot.image.sprite = slotMemb.journalVisual;
                        slot.slotMember = slotMemb;

                        Debug.Log("Member : " + slotMemb + " slot = " + slot + slotMemb);
                }

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
        }
                
        selectedBase.journalLoaded = true;
    }

    public void TeamDepopulate()
    {

        foreach(Transform child in parentGameObject.transform)
        {
            Destroy(child.gameObject);

            foreach(Transform slotParent in child)
            {
                Destroy(slotParent.gameObject);

                foreach(Transform slotChild in slotParent)
                {                      
                    Destroy(slotChild.gameObject);
                }
            }
        }
    }

    void PopulateMapPage()
    {   
        mapPage.CheckButtons();
        mapPage.pageHead.text = "WORLDMAP \n" + "DAY" + timeManager.currentDay;
        // Afficher les cases, les membres de la team aussi, faire en sortee que tout soit cliquable mais ça se fera sur le map Manager
    }

    void PopulateSuppliesPage() 
    {
        suppliesPage.CheckButtons();

        suppliesPage.pageHead.text = "DAY " + timeManager.currentDay + ":\n";

    }

    void PopulateNeedsPage() 
    {
        needsPage.CheckButtons();
        needsPage.pageHead.text = "DAY " + timeManager.currentDay + ":\n";
        needsPage.pageBody.text = "";

        foreach(Member member in selectedBase.membersInBase)
        {
            
            if(selectedBase.membersInBase != null){

                // Add pronuns for each member
                needsPage.pageBody.text += member.name;

                // FOOD
                if(member.hunger >= 5){needsPage.pageBody.text += " is fully feed,";}
                if(member.hunger < 5 && member.hunger >= 3){needsPage.pageBody.text += " he would not mind to eat something,";}
                if(member.hunger < 3 && member.hunger >1){needsPage.pageBody.text += " he begin to be hungry,";}
                if(member.hunger == 1){needsPage.pageBody.text += " he will die if he don't eat,";}
                if(member.hunger == 0){needsPage.pageBody.text += " he died from hunger,";}

                // DRINK
                if(member.thirst > 5){needsPage.pageBody.text += " is full of water,";}
                if(member.thirst < 5 && member.hunger > 3){needsPage.pageBody.text += " he would not mind to drink a little something,";}
                if(member.thirst < 3 && member.hunger >1){needsPage.pageBody.text +=  " he begin to be really thirsty,";}
                if(member.thirst == 1){needsPage.pageBody.text += " he will die if he don't drink,";}
                if(member.thirst == 0){needsPage.pageBody.text += " he died from thirst,";}
                
                // PHYSICALHEALTH
                if(member.physicalHealth == 2){needsPage.pageBody.text += " he is in perfect health,";}
                if(member.physicalHealth == 1){needsPage.pageBody.text += " he begin to be in a critical health,";}
                if(member.physicalHealth == 0){needsPage.pageBody.text += " he died from his physical health problems,";}

                // MENTALHEALTH
                if(member.mentalHealth == 2){needsPage.pageBody.text += " he is in perfect health,";}
                if(member.mentalHealth == 1){needsPage.pageBody.text += " he begin to be in a critical mental health problems,";}
                if(member.mentalHealth == 0){needsPage.pageBody.text += " he died due to mental illness,";}
            }
        }
    }

    void PopulateEventPage()
    {
        // eventGenerator.currentEvent.completed = true;
        // eventGenerator.pastEvent = eventGenerator.currentEvent;
        eventPage.pageHead.text = "DAY " + timeManager.currentDay + ":\n";
        
        rnEvent = eventGenerator.currentEvent;
        eventPage.pageBody.text += rnEvent.description;
    }

    public void ApplyActions()
    {
        TeamDepopulate();
        TeamPopulate();
    }
}

