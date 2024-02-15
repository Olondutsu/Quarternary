using UnityEngine;
using UnityEngine.UI;

public class DisplayJournal : MonoBehaviour
{
    public Text currentPage;
    public GameObject visualPage;

    public EventGenerator eventGenerator;
    public TeamManager teamManager;
    public TimeManager timeManager;
    public ItemsManager itemsManager;
    public Member member;

    public Page eventPage;
    public Page needsPage;
    public Page suppliesPage;


    private bool eventDisplayed;
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
        }

        if(eventDisplayed)
        {
            needsPage.pageVisual.SetActive(true);
        }

        if(teamInfosPageDisplayed)
        {
            suppliesPage.pageVisual.SetActive(true);
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
        eventGenerator.outcomeDisplayed = false;
        
        suppliesPage.pageHead.text = "\n";
        suppliesPage.pageBody.text =  "\n";

        needsPage.pageHead.text = "\n";
        needsPage.pageBody.text =  "\n";

        eventPage.pageHead.text = "\n";
        eventPage.pageBody.text =  "\n";

        teamManager.AdjustTeamStats(teamManager.feedRate, teamManager.drinkRate,teamManager.feedRate, teamManager.drinkRate);
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

        suppliesPageDisplayed = true;
    }

    void PopulateNeedsPage() 
    {
        needsPage.CheckButtons();

        needsPage.pageHead.text += timeManager.currentDay + " :\n";

        foreach(Member member in teamManager.teamMembers)
        {
            if(member.isInTeam)
            {
                needsPage.pageBody.text += member.fullname + " :\n";
                needsPage.pageBody.text += "Physical Health : " + member.physicalHealth + "\n";
                needsPage.pageBody.text += "Mental Health : " + member.mentalHealth + "\n";
                needsPage.pageBody.text += "Hunger : " + member.hunger + "\n";
                needsPage.pageBody.text += "Thirst : " + member.thirst + "\n";
                needsPage.pageBody.text += "\n";
            }
        }

        teamInfosPageDisplayed = true;
    }
    void PopulateEventPage()
    {
        eventPage.CheckButtons();
        eventPage.pageHead.text = timeManager.currentDay + " :\n";
        Event rnEvent = eventGenerator.currentEvent;
        eventPage.pageBody.text = rnEvent.description;

    }

    public void ApplyAction()
    {
        
    }
}

