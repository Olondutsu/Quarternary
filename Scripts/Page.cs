using UnityEngine;
using UnityEngine.UI;

public class Page: MonoBehaviour
{
    bool isEventPage;
    bool isNeedsPage;
    bool isSuppliesPage;

    public Slot slot;
    public Text pageHead;
    public Text pageBody;
    public EventGenerator eventGenerator;
    public DisplayJournal displayJournal;
    public GameObject pageVisual;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject nextButton;



    void Start()
    {

    }

    public void CheckButtons()
    {
        if(eventGenerator.currentEvent.boolChoice && isEventPage)
        {
            yesButton.SetActive(true);
            noButton.SetActive(true);
            nextButton.SetActive(false);
        }
        else
        {
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
        displayJournal.OnNextPageButtonClick();
    }

}

