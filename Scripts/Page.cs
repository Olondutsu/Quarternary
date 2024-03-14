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
    public GameObject parentGameObject;
    Page nextPage;
    public Base selectedBase;
    public Text pageHead;
    public Text pageBody;

    public Page instance;
    public bool isMapPage;
    public bool isEventPage;
    public bool isNeedsPage;
    public bool isSuppliesPage;

    void Start()
    {
        eventGenerator = FindObjectOfType<EventGenerator>();
        displayJournal = FindObjectOfType<DisplayJournal>();
        
        instance = this;
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
        eventGenerator.EventItemHandler(displayJournal.nextEvent);
        // displayJournal.ImmediateNextEventPage(this, eventGenerator.currentEvent);
        CheckButtons();
        displayJournal.OnNextPage();

    }
    public void OnNoClick()
    {
         eventGenerator.currentEvent.noChoice = true;
         eventGenerator.EventEnabler();
         eventGenerator.EventItemHandler(displayJournal.nextEvent);
        //  displayJournal.ImmediateNextEventPage(this, eventGenerator.currentEvent);
         CheckButtons();
         displayJournal.OnNextPage();
    }
    public void OnNextClick()
    {
        if(eventGenerator.currentEvent.hasImDis)
        {
            eventGenerator.EventItemHandler(displayJournal.nextEvent);
        }
        else
        {
            eventGenerator.EventItemHandler(eventGenerator.currentEvent);
        }
        displayJournal.pastIndex = displayJournal.journalIndex;
        
        displayJournal.OnNextPage();
    }
}

