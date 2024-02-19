using UnityEngine;
using UnityEngine.UI;

public class Page: MonoBehaviour
{
    public EventGenerator eventGenerator;
    public DisplayJournal displayJournal;

    public GameObject pageVisual;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject nextButton;

    public Text pageHead;
    public Text pageBody;

    public bool isEventPage;
    public bool isNeedsPage;
    public bool isSuppliesPage;

    void Start()
    {
    }

    public void CheckButtons()
    {
        if(isEventPage)
        {
            if(eventGenerator.currentEvent.boolChoice)
            {
                Debug.Log("event is a bool choice");
                yesButton.SetActive(true);
                noButton.SetActive(true);
                nextButton.SetActive(false);
            }
            else
            {
                Debug.Log("event isnot a bool choice");
                yesButton.SetActive(false);
                noButton.SetActive(false);
                nextButton.SetActive(true);
            }
        }
        else
        {
            Debug.Log("not an event");
            yesButton.SetActive(false);
            noButton.SetActive(false);
            nextButton.SetActive(true);
        }
    }

    public void OnYesClick()
    {
        eventGenerator.currentEvent.yesChoice = true;
        eventGenerator.EventEnabler();

    }
    public void OnNoClick()
    {
         eventGenerator.currentEvent.noChoice = true;
         eventGenerator.EventEnabler();
    }
    public void OnNextClick()
    {
        if(displayJournal.journalDisplayed)
        {
            Debug.Log("journalDisplayed = true on met eventDisplay true");
            displayJournal.eventDisplayed = true;
        }

        if(displayJournal.journalIndex == 1)
        {
            Debug.Log("displayJournal.journalIndex == 1");
            displayJournal.teamInfosPageDisplayed = true;
        }

        if(displayJournal.journalIndex == 2)
        {
            Debug.Log("displayJournal.journalIndex == 2");
            displayJournal.suppliesPageDisplayed = true;
        }
        if(displayJournal.journalIndex == 3)
        {
            Debug.Log("displayJournal.journalIndex == 3");
            displayJournal.readytoNextDay = true;
        }
        if(displayJournal.teamInfosPageDisplayed && displayJournal.teamInfosPageDisplayed && displayJournal.journalDisplayed && displayJournal.readytoNextDay)
        {
            Debug.Log("confirmation du readytoNextday");
            displayJournal.ResetJournal();
        }

        displayJournal.pastIndex = displayJournal.journalIndex;
        displayJournal.OnNextPageButtonClick();


    }
}

