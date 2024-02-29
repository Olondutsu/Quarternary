using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using System.Collections.Generic;
using UnityEditor;

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

    List<Slot> slotList = new List<Slot>();


    public GameObject parentGameObject;
    public GameObject memberPrefab;
    public GameObject travelButtons;
    public GameObject parentPages;
    public GameObject pagePrefab;

    public Page needsPage;
    public Page suppliesPage;
    public Page mapPage;
    public Base selectedBase;

    public int maxCharPerPage = 250;
    public int pastIndex = 0;
    public int journalIndex = 0;
    public bool journalDisplayed = false;

    private int index = 0;
    public int eventIndex = 0;
    public int needsIndex = 0;
    public int supplieIndex = 0;
    public int mapIndex = 0;



    void Start()
    {
        
        NewDay();

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
            if(selectedBase.pagePrefab.Count == 0)
            {
                Debug.Log("if pagePrefab count = 0");
                eventGenerator.RandomizeEvent();
                InitializePages();
            }

            if(selectedBase.pagePrefab != null)
            {
                Debug.Log("if(selectedBase.pagePrefab != null)");
                if(selectedBase.pagePrefab.Count > 0)
                {
                    GameObject go = selectedBase.pagePrefab[0];
                    // Page goPage = go.GetComponent<Page>();

                    go.SetActive(true);
                    // goPage.CheckButtons();
                
                    Debug.Log("if(selectedBase.pagePrefab.Count > 0");
                }
            }
            
            journalDisplayed = !journalDisplayed;
            journal.SetActive(journalDisplayed);
            Debug.Log("OnOpenClickOP");
        }
    }

    public void InitializePages()
    {
        // creer ou mettre à jour une liste;
        
        if(eventGenerator.currentEvent != null)
        {

            Debug.Log("EventGenerator.currentEVent != null ");
            if(eventGenerator.currentEvent.description.Length >= maxCharPerPage)
            {
                Debug.Log("if(eventGenerator.currentEvent.description.Length >= maxCharPerPage)");
                int amountOfPages = 0;

                amountOfPages = eventGenerator.currentEvent.description.Length/maxCharPerPage;

                for(int i = 0; i > amountOfPages; i++ )
                {
                    Debug.Log("for(int i = 0; i > amountOfPages; i++ ) c'est la qu'on crée les GO");
                    GameObject goEvent;
                    goEvent = Instantiate(pagePrefab, (parentPages.transform)) as GameObject;
                    goEvent.transform.SetParent(parentPages.transform);

                    Page goEventPage = goEvent.GetComponent<Page>();
                    
                    goEventPage.isEventPage = true;

                    selectedBase.pagePrefab.Add(goEvent);
                    eventIndex++;
                }
            }
            else
            {
                    GameObject goEvent;
                    goEvent = Instantiate(pagePrefab, (parentPages.transform)) as GameObject;
                    goEvent.transform.SetParent(parentPages.transform);

                    Page goEventPage = goEvent.GetComponent<Page>();
                    
                    goEventPage.isEventPage = true;

                    selectedBase.pagePrefab.Add(goEvent);
                    eventIndex++;
            }

            if(eventGenerator.currentEvent.hasImDis)
            {
                    GameObject goEvent;
                    goEvent = Instantiate(pagePrefab, (parentPages.transform)) as GameObject;
                    goEvent.transform.SetParent(parentPages.transform);

                    Page goEventPage = goEvent.GetComponent<Page>();
                    
                    goEventPage.isEventPage = true;

                    selectedBase.pagePrefab.Add(goEvent);

                    eventIndex++;
            }
        }

        if(selectedBase != null)
        {
            //NeedPage
            GameObject goNeed;

            goNeed = Instantiate(pagePrefab, (parentPages.transform)) as GameObject;
            goNeed.transform.SetParent(parentPages.transform);

            Page goNeedPage = goNeed.GetComponent<Page>();

            goNeedPage.isNeedsPage = true;
            needsPage = goNeedPage;

            selectedBase.pagePrefab.Add(goNeed);

        }
            //Supplie Page
            GameObject goSupplie;
            goSupplie = Instantiate(pagePrefab, (parentPages.transform)) as GameObject;
            goSupplie.transform.SetParent(parentPages.transform);

            Page goSuppliePage = goSupplie.GetComponent<Page>();


            goSuppliePage.isSuppliesPage = true;

            selectedBase.pagePrefab.Add(goSupplie);
            suppliesPage = goSuppliePage;

            supplieIndex++;


        foreach(GameObject go in selectedBase.pagePrefab)
        {
            Page goPage = go.GetComponent<Page>();

            goPage.eventGenerator = FindObjectOfType<EventGenerator>();
            goPage.displayJournal = FindObjectOfType<DisplayJournal>();

            goPage.CheckButtons();
        }

        TeamPopulate();
        PopulatePages();
    }

    public void OnNextPage()
    {
        int indexCheck = index + 1;

        Debug.Log("OnNextPage Appelé");

        List<Page> eventPages = new List<Page>();
        
        GameObject go = selectedBase.pagePrefab[index];
        Page page = go.GetComponent<Page>();

        if(selectedBase.pagePrefab.Count > indexCheck)
        {
            Debug.Log("if(selectedBase.pagePrefab.Count > indexCheck)");
            
            index ++;

            GameObject goNext = selectedBase.pagePrefab[indexCheck];
            Page goPageNext = goNext.GetComponent<Page>(); 

            if(page.isSuppliesPage)
            {
                Debug.Log("IsSupplyPage");
                page.parentGameObject.SetActive(true);
            }
            else
            {
                Debug.Log("else is not SupplyPage");
                page.parentGameObject.SetActive(false);
            }
            if(page.isEventPage)
            {
                if(eventGenerator.currentEvent.hasImDis)
                {
                    eventGenerator.currentEvent.completed = true;
                }
            }
            
            if(goPageNext != null)
            {
                Debug.Log("GoPageNext != Null Les GO sont les suivants go " + go + "goNext" + goNext + "Index : " + index );
                goNext.SetActive(true);
                goPageNext.CheckButtons();
                

            }
            else
            {
                Debug.Log("GoPageNext = Null");
            }
        }
        else
        {
            foreach(GameObject gor in selectedBase.pagePrefab)
            {
                gor.SetActive(false);
            }
            Debug.Log("Ready for Next day");

            timeManager.NextDay();
            // en commentaire pour calmer les logs
            // ApplyActions();
            
        }    
    }

    public void OnEndDay()
    {

    }

    public void OnPreviousClick()
    {
        int pastIndex = index - 1;

        GameObject go = selectedBase.pagePrefab[index];

        Page page = go.GetComponent<Page>();

        go.SetActive(false);
        index--;
    }

    public void NewDay()
    {
        Debug.Log("NewDayCalled");
        eventGenerator.RandomizeEvent();
        index = 0;
        ResetJournal();
    }

    public void ResetJournal()
    {
        if(selectedBase != null)
        {
            if(selectedBase.journalLoaded)
            {
                Debug.Log("if(selectedBase.journalLoaded)");
                Debug.Log("selectedBase.pagePrefab.Count =  " + selectedBase.pagePrefab.Count );

                while (selectedBase.pagePrefab.Count > 0)
                {
                    Debug.Log("Pour chaque pagePrefab dans ..");
                    GameObject pagePrefab = selectedBase.pagePrefab[0];
                    selectedBase.pagePrefab.RemoveAt(0);
                    Destroy(pagePrefab);
                }

                while (selectedBase.pagesForBase.Count > 0)
                {
                    Debug.Log("Pour chaque pageforBases dans ..");
                    Page page = selectedBase.pagesForBase[0];
                    selectedBase.pagesForBase.RemoveAt(0);
                    Destroy(page.gameObject);
                }
            
                // InitializePages();
            }

            else
            {
                Debug.Log("Wtf le journal n'est pas Loaded et on appelle le reset Journal, trouvez moi ce criminel");
                // InitializePages();
            }
        }
    }

     public void TeamPopulate()
    {
        

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
            go = Instantiate(memberPrefab, (suppliesPage.parentGameObject.transform)) as GameObject;

            go.transform.SetParent(suppliesPage.parentGameObject.transform);

        }

        foreach(Transform child in suppliesPage.parentGameObject.transform)
        {
            Debug.Log(child);

            foreach(Transform slotParents in child)
            {
                foreach(Transform slotChild in slotParents)
                {
                    Slot slotScript = slotChild.GetComponent<Slot>();

                    if(slotScript != null)
                    {
                        slotList.Add(slotScript);
                    }
                }
            }
        }

        int slotInt = 0;

        foreach(Slot slot in slotList)
        {
            
            if(slot != null)
            {
                Debug.Log("SlotInt" + slotInt);
                if (slot.isCharacterSlot)
                {
                        Debug.Log("Slot " + slot + " isCharacterSlot"); 
                        Member slotMemb = selectedBase.membersInBase[slotInt];
                        Debug.Log("Character Slot character " + slotMemb);
                        
                        slot.image.sprite = slotMemb.journalVisual;

                        slot.slotMember = slotMemb;

                        int slotChecker = slotInt + 1;

                        if(selectedBase.membersInBase.Count > slotChecker)
                        {
                            Debug.Log("if(selectedBase.membersInBase.Count > slotChecker)");
                            slotInt++;
                        }
                        else
                        {
                             Debug.Log("else if(selectedBase.membersInBase.Count < slotChecker)"); 
                        }
                }

                if (slot.isHealKitSlot)
                {
                    slot.slotItem = healKitItemData;
                    isHealSlot = slot;
                    Debug.Log("Slot " + slot + " isHealKitSlot"); 

                    if(itemsManager.healKit >= 1)
                    {
                        slot.image.sprite= healKitItemData.journalVisualAvailable;
                        
                        Member slotMemb = selectedBase.membersInBase[slotInt];
                        Debug.Log("HealKit SLot Memb = " + slotMemb); 

                        slot.slotMember = slotMemb;
                    }
                    else
                    {
                        slot.image.sprite = healKitItemData.journalVisualUnAvailable;
                        Debug.Log(" else il ny a pas d'healkit ?"); 
                    }
                }

                if (slot.isFoodSlot)
                {
                    slot.slotItem = foodItemData;
                    isFoodSlot = slot;
                    Debug.Log("Slot " + slot + " isFoodSlot"); 
                    if(itemsManager.food >= 1)
                    {
                        slot.image.sprite = foodItemData.journalVisualAvailable;   

                        Member slotMemb = selectedBase.membersInBase[slotInt];
                        Debug.Log("FoodSLot Memb = " + slotMemb); 
                        
                        slot.slotMember = slotMemb;
                    }
                    else
                    {
                        slot.image.sprite = foodItemData.journalVisualUnAvailable;
                    }
                }
                            
                if (slot.isDrinkSlot)
                {
                    slot.slotItem = drinkItemData;
                    isDrinkSlot = slot;
                    Debug.Log("Slot " + slot + " isDrinkSlot"); 
                    if(itemsManager.drink >= 1)
                    {
                       
                        slot.image.sprite = drinkItemData.journalVisualAvailable;
                                                
                        Member slotMemb = selectedBase.membersInBase[slotInt];
                        Debug.Log("DrinkLot Memb = " + slotMemb); 
                        slot.slotMember = slotMemb;
                    }

                    else
                    {
                        slot.image.sprite = drinkItemData.journalVisualUnAvailable;
                    }
                }
            }
        }
                
    }

    public void TeamDepopulate()
    {

        foreach(Transform child in suppliesPage.parentGameObject.transform)
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

    public void PopulatePages()
    {
        // int eventIndex = 0;

        foreach(GameObject go in selectedBase.pagePrefab)
        {
            Page goPage = go.GetComponent<Page>();
            
            if(goPage.isMapPage)
            {
                goPage.CheckButtons();
                goPage.pageHead.text = "WORLDMAP \n" + "DAY" + timeManager.currentDay;

                teamManager.selectedBase.pagesForBase.Add(goPage);
            }
            if(goPage.isSuppliesPage)
            {
                    goPage.CheckButtons();

                    goPage.pageHead.text = "DAY " + timeManager.currentDay + ":\n";
                    teamManager.selectedBase.pagesForBase.Add(suppliesPage);
            }
            if(goPage.isNeedsPage)
            {
                goPage.CheckButtons();
                goPage.pageHead.text = "DAY " + timeManager.currentDay + ":\n";
                goPage.pageBody.text = "";

                foreach(Member member in selectedBase.membersInBase)
                {
                    
                    if(selectedBase.membersInBase != null){

                        // Add pronuns for each member
                        goPage.pageBody.text += member.name;

                        // FOOD
                        if(member.hunger >= 5){goPage.pageBody.text += " is fully feed,";}
                        if(member.hunger < 5 && member.hunger >= 3){goPage.pageBody.text += " he would not mind to eat something,";}
                        if(member.hunger < 3 && member.hunger >1){goPage.pageBody.text += " he begin to be hungry,";}
                        if(member.hunger == 1){goPage.pageBody.text += " he will die if he don't eat,";}
                        if(member.hunger == 0){goPage.pageBody.text += " he died from hunger,";}

                        // DRINK
                        if(member.thirst > 5){goPage.pageBody.text += " is full of water,";}
                        if(member.thirst < 5 && member.hunger > 3){goPage.pageBody.text += " he would not mind to drink a little something,";}
                        if(member.thirst < 3 && member.hunger >1){goPage.pageBody.text +=  " he begin to be really thirsty,";}
                        if(member.thirst == 1){goPage.pageBody.text += " he will die if he don't drink,";}
                        if(member.thirst == 0){goPage.pageBody.text += " he died from thirst,";}
                        
                        // PHYSICALHEALTH
                        if(member.physicalHealth == 2){goPage.pageBody.text += " he is in perfect health,";}
                        if(member.physicalHealth == 1){goPage.pageBody.text += " he begin to be in a critical health,";}
                        if(member.physicalHealth == 0){goPage.pageBody.text += " he died from his physical health problems,";}

                        // MENTALHEALTH
                        if(member.mentalHealth == 2){goPage.pageBody.text += " he is in perfect health,";}
                        if(member.mentalHealth == 1){goPage.pageBody.text += " he begin to be in a critical mental health problems,";}
                        if(member.mentalHealth == 0){goPage.pageBody.text += " he died due to mental illness,";}
                    }
                }
                teamManager.selectedBase.pagesForBase.Add(goPage);
            }

            if(goPage.isEventPage)
            {
                rnEvent = eventGenerator.currentEvent;
                if(rnEvent.hasImDis)
                {
                    int indexCheck = index + 1 ; 
                    GameObject goNext = selectedBase.pagePrefab[indexCheck];
                    Page goPageNext = goNext.GetComponent<Page>(); 

                    if(goPageNext.isEventPage)
                    {
                        if(rnEvent.boolChoice)
                        {
                            if(rnEvent.yesChoice)
                            {
                                int aBool = UnityEngine.Random.Range(0, rnEvent.yesOutcomeEvents.Length);
                                Event imDisEvent = rnEvent.yesOutcomeEvents[aBool];
                                goPageNext.pageBody.text = imDisEvent.description;
                            }
                            if(rnEvent.noChoice)
                            {
                                int aBool = UnityEngine.Random.Range(0, rnEvent.yesOutcomeEvents.Length);
                                Event imDisEvent = rnEvent.noOutcomeEvents[aBool];
                                goPageNext.pageBody.text = imDisEvent.description;
                            }
                        }
                        else
                        {
                                int aBool = UnityEngine.Random.Range(0, rnEvent.yesOutcomeEvents.Length);
                                Event imDisEvent = rnEvent.outcomeEvents[aBool];
                                goPageNext.pageBody.text = imDisEvent.description;
                        }
                    }
                }

                if(go == selectedBase.pagePrefab[eventIndex])
                {
                    Debug.Log("go == selectedBase.pagePrefab[eventIndex]");
                    goPage.pageHead.text = "DAY " + timeManager.currentDay + ":\n";

                    if(rnEvent.description.Length <= maxCharPerPage)
                    {
                        goPage.pageBody.text += rnEvent.description;

                        teamManager.selectedBase.pagesForBase.Add(goPage);
                    }
                    else
                    {
                        int startCharIndex = eventIndex * maxCharPerPage;
                        int remainingChars = rnEvent.description.Length - startCharIndex;
                        int charsToAdd = Mathf.Min(remainingChars, maxCharPerPage);

                        goPage.pageBody.text += rnEvent.description.Substring(startCharIndex, charsToAdd);
                        
                        teamManager.selectedBase.pagesForBase.Add(goPage);
                    }


                }
                else
                {
                    Debug.Log("go == selectedBase.pagePrefab[eventIndex], donc a priori c'est la derniere page d'events");
                }
            }

        }
        selectedBase.journalLoaded = true;
    }
    public void ApplyActions()
    {
        foreach(Slot slot in slotList)
        {
            if(slot.slotItem != null)
            {
                ItemData item = slot.slotItem;
                Member member = slot.slotMember;

                if(item.selected)
                {
                    itemsManager.OnUse(slot.slotMember, item);
                }
            }
        }
        if(selectedBase.selectedMembers != null)
        {
            foreach(Member member in selectedBase.selectedMembers)
            {
                selectedBase.membersInBase.Remove(member);
                selectedBase.membersInTravel.Add(member);

            }
        }
        if(!eventGenerator.currentEvent.hasImDis)
        {
            eventGenerator.EventEnabler();
            
        }

        TeamDepopulate();
        TeamPopulate();
    }
}

