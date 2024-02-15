using UnityEngine;
using UnityEngine.UI;

public class DisplayJournal : MonoBehaviour
{
    public EventGenerator eventGenerator;
    public TeamManager teamManager;
    public TimeManager timeManager;
    public ItemsManager itemsManager;
    public Member member;

    public Page eventPage;
    public Page needsPage;
    public Page suppliesPage;


    private bool eventDisplayed =false;
    private bool eventBegan = false;
    private bool journalDisplayed = false;
    private bool teamInfosPageDisplayed = false;
    private bool suppliesPageDisplayed = false;


    void Start()
    {
        journalDisplayed = true;
        PopulatePages();
    }


    public void OnNextPageButtonClick()
    {
        if(journalDisplayed)
        {
            eventPage.pageVisual.SetActive(true);
            eventDisplayed = true;
        }

        if(eventDisplayed)
        {
            needsPage.pageVisual.SetActive(true);
            teamInfosPageDisplayed = true;
        }

        if(teamInfosPageDisplayed)
        {
            suppliesPage.pageVisual.SetActive(true);
            suppliesPageDisplayed = true;
        }

        if(journalDisplayed && teamInfosPageDisplayed && suppliesPageDisplayed)
        {
            
            timeManager.NextDay();
            ResetJournal();
        }
    }
    
    public void NewDay()
    {
        journalDisplayed = true;
        PopulatePages();
    }

    public void ResetJournal()
    {
        journalDisplayed = false;
        teamInfosPageDisplayed = false;
        suppliesPageDisplayed = false;
        eventDisplayed = false;
        
        suppliesPage.pageHead.text = "\n";
        suppliesPage.pageBody.text =  "\n";

        needsPage.pageHead.text = "\n";
        needsPage.pageBody.text =  "\n";

        eventPage.pageHead.text = "\n";
        eventPage.pageBody.text =  "\n";

        // a déplacer dans nextday du timeManager
        

        NewDay();
    }

    public void PopulatePages()
    {
        PopulateEventPage();
        PopulateNeedsPage();
        PopulateSuppliesPage();
    
    }

    void PopulateSuppliesPage() 
    {
        suppliesPage.CheckButtons();

        suppliesPage.pageHead.text += timeManager.currentDay + " :\n";

        foreach(Member member in teamManager.teamMembers)
        {
        // A mettre en enfant dun truc qui s'alligne ?
        member.journalVisual.SetActive(true);

            foreach(ItemData item in itemsManager.inventoryItems)
            {
                if(item.isSupplie)
                {

                suppliesPage.slot.emptyVisual = item.journalVisualAvailable;

                suppliesPage.pageBody.text += "Heal Kit : " + itemsManager.healKit + "\n";
                suppliesPage.pageBody.text += "Food : " + itemsManager.food + "\n";
                suppliesPage.pageBody.text += "Drink : " + itemsManager.drink + "\n";
                suppliesPage.pageBody.text += "Money : " + itemsManager.money + "\n";
                suppliesPage.pageBody.text += "\n";
                }
            }
        }

        
    }

    void PopulateNeedsPage() 
    {
        needsPage.CheckButtons();

        needsPage.pageHead.text += timeManager.currentDay + " :\n";

        foreach(Member member in teamManager.teamMembers)
        {
            if(member.isInTeam)
            {
                // FOOD
                if(member.hunger > 5)
                {
                    needsPage.pageBody.text = member.name + "is fully feed.";
                }
                if(member.hunger < 5 && member.hunger > 3)
                {
                    needsPage.pageBody.text = member.name + "would not mind to eat something.";
                }
                if(member.hunger < 3 && member.hunger >1)
                {
                    needsPage.pageBody.text = member.name + "begin to be hungry.";
                }
                if(member.hunger == 1)
                {
                    needsPage.pageBody.text = member.name + "will die if he don't eat.";
                }
                
                if(member.hunger == 0)
                {
                    needsPage.pageBody.text = member.name + "died from hunger.";
                }

                // DRINK
                if(member.thirst > 5)
                {
                    needsPage.pageBody.text = member.name + "is full of water.";
                }
                if(member.thirst < 5 && member.hunger > 3)
                {
                    needsPage.pageBody.text = member.name + "would not mind to drink a little something.";
                }
                if(member.thirst < 3 && member.hunger >1)
                {
                    needsPage.pageBody.text = member.name + "begin to be really thirsty.";
                }
                if(member.thirst == 1)
                {
                    needsPage.pageBody.text = member.name + "will die if he don't drink.";
                }
                if(member.thirst == 0)
                {
                    needsPage.pageBody.text = member.name + "died from thirst.";
                }
                
                // PHYSICALHEALTH
                if(member.physicalHealth == 2)
                {
                    needsPage.pageBody.text = member.name + "is in perfect health";
                }
                if(member.physicalHealth == 1)
                {
                    needsPage.pageBody.text = member.name + "begin to be in a critical health";
                }
                if(member.physicalHealth == 0)
                {
                    needsPage.pageBody.text = member.name + "died from his physical health problems";
                }

                // MENTALHEALTH
                if(member.mentalHealth == 2)
                {
                    needsPage.pageBody.text = member.name + "is in perfect health";
                }
                if(member.mentalHealth == 1)
                {
                    needsPage.pageBody.text = member.name + "begin to be in a critical mental health problems";
                }
                if(member.mentalHealth == 0)
                {
                    needsPage.pageBody.text = member.name + "died due to mental illness";
                }
            }
        }

        teamInfosPageDisplayed = true;
    }
    void PopulateEventPage()
    {
        eventGenerator.EventEnabler();
        eventGenerator.RandomizeEvent();
        eventPage.CheckButtons();

        eventGenerator.currentEvent.completed = true;
        eventGenerator.pastEvent = eventGenerator.currentEvent;

        eventPage.pageHead.text = timeManager.currentDay + " :\n";
        Event rnEvent = eventGenerator.currentEvent;
        eventPage.pageBody.text = rnEvent.description;
        

    }

    public void ApplyAction()
    {
        
    }
}

