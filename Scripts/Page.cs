using UnityEngine;
using UnityEngine.UI;

public class Page: MonoBehaviour
{

    public Slot slot;
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

