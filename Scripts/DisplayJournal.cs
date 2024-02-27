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

    public int eventIndex = 0;
    public int needsIndex = 0;
    public int supplieIndex = 0;
    public int mapIndex = 0;



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
                    go.SetActive(true);
                    Debug.Log("if(selectedBase.pagePrefab.Count > 0");
                }
            }

            if(journalIndex == 0)
            {
                AddIndex();
            }
            
            journalDisplayed = !journalDisplayed;
            journal.SetActive(journalDisplayed);
            Debug.Log("OnOpenClickOP");
        }
    }

    //To instantiate pages/ REFONTE DU SYSTEME INITIAL NON UTILISE JUSQUA PRESENT
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
        }

        if(selectedBase != null)
        {
            //Supplie Page
            GameObject goSupplie;
            goSupplie = Instantiate(pagePrefab, (parentPages.transform)) as GameObject;
            goSupplie.transform.SetParent(parentPages.transform);

            Page goSuppliePage = goSupplie.GetComponent<Page>();


            goSuppliePage.isSuppliesPage = true;

            selectedBase.pagePrefab.Add(goSupplie);
            suppliesPage = goSuppliePage;

            supplieIndex++;

            //NeedPage
            GameObject goNeed;

            goNeed = Instantiate(pagePrefab, (parentPages.transform)) as GameObject;
            goNeed.transform.SetParent(parentPages.transform);

            Page goNeedPage = goNeed.GetComponent<Page>();

            goNeedPage.isNeedsPage = true;
            needsPage = goNeedPage;

            selectedBase.pagePrefab.Add(goNeed);
        }
        foreach(GameObject go in selectedBase.pagePrefab)
        {
            Page goPage = go.GetComponent<Page>();

            goPage.eventGenerator = FindObjectOfType<EventGenerator>();
            goPage.displayJournal = FindObjectOfType<DisplayJournal>();
        }

        TeamPopulate();
        PopulatePages();
    }

    public void OnNextPage(Page page)
    {
        Debug.Log("OnNextPage Appelé");

        List<Page> eventPages = new List<Page>();

        int eventPageIndex = 0;

        if(page.isEventPage && journalDisplayed)
        {
            Debug.Log("if(page.isEventPage && journalDisplayed)");
            
            eventPages.Add(page);

            // il faut faire évoluer ça car on ajoute à cahque fois l'affichage de la prochaine page plutôt que sur le button de la page d'avant
            // en gros là dans le else on met qu'on active la needsPage puis dans la needsPage qu'on active la suppliepage etc..
            //voir directement la prochaien dans l'index car noramlement ils sont ajoutés à la liste dans le bon ordre
            //Donc on setactive dans la liste genre pagesPrefab[currentIndex] en commençant l'index à 0
            int index = 0;

            // index++;
            int indexCheck = index + 1;

            if(selectedBase.pagePrefab.Count >= indexCheck)
            {
                Debug.Log("if(selectedBase.pagePrefab.Count > index)");

                GameObject go = selectedBase.pagePrefab[index];
                Page goPage = go.GetComponent<Page>();

                

                go.SetActive(true);

                if(page == eventPages[eventPageIndex])
                {
                    Debug.Log("If page = eventPage[eventPageIndex]");
                    page.gameObject.SetActive(true);
                    page.CheckButtons();
                    eventPageIndex++;
                    AddIndex();
                    index++;
                }

                else
                {
                    Debug.Log("else page != eventPage[eventPageIndex]");
                    page.gameObject.SetActive(false);
                }

                if(page.isNeedsPage)
                {
                    Debug.Log("If Page is NeedsPage");
                    
                    if(journalIndex == eventIndex + 1)
                    {
                        Debug.Log("If (journalIndex == eventIndex + 1)");
                        page.gameObject.SetActive(true);
                        page.CheckButtons();
                        AddIndex();
                        index++;
                    }
                    else
                    {

                        Debug.Log("If (journalIndex!== eventIndex (" + eventIndex + ") + 1)");
                        Debug.Log("Journal Index  " + journalIndex +  "  " +  eventIndex + "+1");
                        page.gameObject.SetActive(false);

                    }
                }

                if(page.isSuppliesPage)
                {
                    Debug.Log(" if(page.isSuppliesPage)");

                    if(journalIndex == eventIndex + needsIndex + 1)
                    {
                        Debug.Log("if(journalIndex == eventIndex (" + eventIndex + "+ needsIndex (" +  needsIndex +  ") + 1)");
                        page.parentGameObject.SetActive(true);
                        page.gameObject.SetActive(true);
                        page.CheckButtons();
                        AddIndex();
                        index++;
                    }
                    else
                    {
                        Debug.Log("else du if(journalIndex == eventIndex (" + eventIndex + "+ needsIndex (" +  needsIndex +  ") + 1");
                        page.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.Log("Else de if(selectedBase.pagePrefab.Count > indexCheck)");
            }

            // if(page = eventPages[eventPageIndex])
            // {
            //     Debug.Log("If page = eventPage[eventPageIndex]");
            //     page.gameObject.SetActive(true);
            //     page.CheckButtons();
            //     eventPageIndex++;
            //     AddIndex();
            // }

            // else
            // {
            //     Debug.Log("else page != eventPage[eventPageIndex]");
            //     page.gameObject.SetActive(false);
            // }
        // }

        
        // if(page.isNeedsPage)
        // {
        //     Debug.Log("If Page is NeedsPage");
        //     if(journalIndex == eventIndex + 1)
        //     {
        //         Debug.Log("If (journalIndex == eventIndex + 1)");
        //         page.gameObject.SetActive(true);
        //         page.CheckButtons();
        //         AddIndex();
        //     }

        //     else
        //     {
        //         Debug.Log("If (journalIndex!== eventIndex (" + eventIndex + ") + 1)");
        //         Debug.Log("Journal Index  " + journalIndex +  "  " +  eventIndex + "+1");
        //         page.gameObject.SetActive(false);
                
        //     }
        // }
    
        // if(page.isSuppliesPage)
        // {
        //     Debug.Log(" if(page.isSuppliesPage)");

        //     if(journalIndex == eventIndex + needsIndex + 1)
        //     {
        //         Debug.Log("if(journalIndex == eventIndex (" + eventIndex + "+ needsIndex (" +  needsIndex +  ") + 1)");
        //         page.parentGameObject.SetActive(true);
        //         page.gameObject.SetActive(true);
        //         page.CheckButtons();
        //         AddIndex();
        //     }
        //     else
        //     {
        //         Debug.Log("else du if(journalIndex == eventIndex (" + eventIndex + "+ needsIndex (" +  needsIndex +  ") + 1");
        //         page.gameObject.SetActive(false);
        //     }
        // }
        }
    }

    public void OnPreviousClick()
    {
        foreach(GameObject go in selectedBase.pagePrefab)
        {
            Page goPage = go.GetComponent<Page>();

            List<Page> eventPages = new List<Page>();

            int eventPageIndex = eventPages.Count;
    

            if(goPage.isEventPage && journalDisplayed)
            {
                if(goPage = eventPages[eventPageIndex])
                {
                    go.SetActive(false);
                    eventPageIndex--;
                    RemoveIndex();
                    
                }
            }
            
            if(goPage.isNeedsPage)
            {
                if(journalIndex == eventIndex + 1)
                {
                    go.SetActive(true);
                    RemoveIndex();
                    goPage.parentGameObject.SetActive(false);
                }
            }

            if(goPage.isSuppliesPage)
            {
                if(journalIndex == eventIndex + needsIndex + 1)
                {
                    goPage.parentGameObject.SetActive(true);
                    go.SetActive(true);
                    RemoveIndex();
                }
            }

        }
    }

    // public void OnNextPageButtonClick()
    // {
    //     if(eventGenerator.currentEvent != null)
    //     {
    //         if(eventGenerator.currentEvent.isEnd && !eventGenerator.currentEvent.completed)
    //         {
    //             eventGenerator.currentEvent.completed = true;
    //             eventGenerator.MemberHandler();
    //         }
    //     }

    //     if(journalDisplayed)
    //     {
    //         Debug.Log("journalDisplayed");
    //         AddIndex();
    //     }

    //     if(journalIndex == 1)  
    //     {
    //         if(eventGenerator.currentEvent != null)
    //         {
    //             Debug.Log("journalIndex == 1");
    //             eventPage.pageVisual.SetActive(true);
    //             eventPage.CheckButtons();
    //             AddIndex();
    //         }
    //         else
    //         {
    //             AddIndex();
    //         }
    //     }

    //     if(journalIndex == eventIndex + 1)
    //     {
    //         Debug.Log("journalIndex == 2");
    //         needsPage.pageVisual.SetActive(true);
    //         needsPage.CheckButtons();
    //         AddIndex();
    //     }

    //     if(journalIndex == eventIndex + needsIndex + 1)
    //     {
    //         suppliesPage.pageVisual.SetActive(true);
    //         suppliesPage.CheckButtons();
    //         AddIndex();
    //     }

    //     if(journalIndex == eventIndex + needsIndex + supplieIndex + 1)
    //     {
    //         Debug.Log("journalIndex == 4");
    //         mapPage.pageVisual.SetActive(true);
    //         mapPage.CheckButtons();
    //     }

    //     if(journalIndex == eventIndex + needsIndex + supplieIndex + mapIndex + 1 && readytoNextDay)
    //     {
    //         ApplyActions();
    //         Debug.Log("journalDisplayed && teamInfosPageDisplayed && suppliesPageDisplayed && readytoNextDay");
    //     }
    // }
    
    public void AddIndex()
    {
        Debug.Log("AddIndex appelé, journal Index = " + journalIndex);
        if(pastIndex == journalIndex)
        {
            Debug.Log("if(pastIndex == journalIndex) on ajoute un a l'index");
            journalIndex++;
        }
    }
    public void RemoveIndex()
    {
        if(pastIndex == journalIndex)
        {
            journalIndex--;
        }
    }

    public void NewDay()
    {
        Debug.Log("NewDayCalled");
        eventGenerator.RandomizeEvent();
        // journalDisplayed = true;
        // PopulatePages();
    }

    public void ResetJournal()
    {
        if(selectedBase.journalLoaded)
        {

            readytoNextDay = false;
            journalDisplayed = false;
            teamInfosPageDisplayed = false;
            suppliesPageDisplayed = false;
            eventDisplayed = false;
            journalIndex = 0;
            pastIndex = 0;

        foreach(GameObject pagePrefab in selectedBase.pagePrefab)
        {
            pagePrefab.SetActive(false);
            selectedBase.pagePrefab.Remove(pagePrefab);
        }

        foreach(Page page in selectedBase.pagesForBase)
        {
            page.pageHead.text = "\n";
            page.pageBody.text =  "\n";
            selectedBase.pagesForBase.Remove(page);
        }

        InitializePages();
        }

        else
        {
            Debug.Log("Wtf le journal n'est pas Loaded et on appelle le reset Journal, trouvez moi ce criminel");
            InitializePages();
        }
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
        int eventIndex = 0;

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
                        eventIndex++;
                        
                        teamManager.selectedBase.pagesForBase.Add(goPage);
                    }
                }
                else
                {
                    Debug.Log("go != selectedBase.pagePrefab[eventIndex]");
                }
            }

        }
        selectedBase.journalLoaded = true;
    }
    public void ApplyActions()
    {
        TeamDepopulate();
        TeamPopulate();
    }
}

