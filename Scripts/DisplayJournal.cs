using UnityEngine;
using UnityEngine.UI;

public class DisplayJournal : MonoBehaviour
{
    public Text journalPage;
    public Text currentPage;

    public EventGenerator eventGenerator;
    public TeamManager teamManager;
    public ItemsManager itemsManager;
    public Member member;
    private bool eventBegan = false;
    private bool journalDisplayed = false;
    private bool teamInfosPageDisplayed = false;
    private bool suppliesPageDisplayed = false;

    // Le Journal doit montrer une base commune sans prendre en compte les Event qui sont gérés par EventGenerator
    // Dans un premier temps on display les Event ou alors le Journal, a voir ce qui est le mieux
    // Pour Display le Journal, on doit pré-configuré certaines pages, les indications sur notre groupe, le rationnement.
    // On montre d'abord l'outcome de l'event précédent..
    // Ensuite on montre les indications sur le groupe
    // Ensuite le rationnement
    // Puis finalement l'event


    void Start()
    {
        eventGenerator.RandomizeEvent(); 
    }

    public void OnNextPageButtonClick()
    {
        if(journalDisplayed)
        {
            if(!eventBegan)
            {
                eventGenerator.RandomizeEvent();
            }
            else
            {
                eventGenerator.EventEnabler();
            }
        }

        if(eventGenerator.outcomeDisplayed)
        {
            TeamInfosPageDisplay();
        }

        else
        {
            eventGenerator.DisplayEvent();
        }

        if (teamInfosPageDisplayed)
        {
            SuppliesPageDisplay();
        }

        if(journalDisplayed && teamInfosPageDisplayed && suppliesPageDisplayed)
        {
            ResetJournal();
        }
    }

    public void ResetJournal()
    {
        journalDisplayed = false;
        teamInfosPageDisplayed = false;
        suppliesPageDisplayed = false;
        eventGenerator.outcomeDisplayed = false;
    }

     public void TeamInfosPageDisplay()
    {
        // teamMembers = teamManager.teamMembers; // Accéder à la liste des membres de l'équipe

        // journalPage.text = $"Journal:\n{currentPage.title}: {currentPage.description}";
            
        foreach (Member member in teamManager.teamMembers)
        {
            if(member.isInTeam)
            {
                journalPage.text += member.fullname + " :\n";
                journalPage.text += "Physical Health : " + member.physicalHealth + "\n";
                journalPage.text += "Mental Health : " + member.mentalHealth + "\n";
                journalPage.text += "Hunger : " + member.hunger + "\n";
                journalPage.text += "Thirst : " + member.thirst + "\n";
                journalPage.text += "\n";
            }
        }

        teamInfosPageDisplayed = true;
    }

    public void SuppliesPageDisplay()
    {
        journalPage.text += "Heal Kit : " + itemsManager.healKit + "\n";
        journalPage.text += "Food : " + itemsManager.food + "\n";
        journalPage.text += "Drink : " + itemsManager.drink + "\n";
        journalPage.text += "Money : " + itemsManager.money + "\n";
        journalPage.text += "\n";

        suppliesPageDisplayed = true;
    }
    //On doit display pour chaque membre de la team des infos différentes, si ils ont besoin de nourriture, d'eau, leurs état de santé mentale ou leurs état de santé physique
}
