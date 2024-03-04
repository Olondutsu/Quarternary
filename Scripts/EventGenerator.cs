using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class EventGenerator : MonoBehaviour
{
    public Text journalText;
    public Event[] events;
    public Event currentEvent;
    public Event pastEvent;
    public Base thisBase;
    public Base selectedBase;

    public Slot slot;
    private ItemsManager itemManager;
    private TeamManager teamManager;
    private DisplayJournal displayJournal;

    List<Event> availableEvents = new List<Event>();
    List<Event> doneEvents = new List<Event>();

    void Start()
    {
        EventChecker();

        displayJournal = transform.GetComponent<DisplayJournal>();
        itemManager = transform.GetComponent<ItemsManager>();
        teamManager = transform.GetComponent<TeamManager>();
    }

    public void EventChecker()
    {
        foreach (Event ev in events)
        {
            if (ev.conditionsMet)
            {
                availableEvents.Add(ev);
            }
            if (ev.completed)
            {
                availableEvents.Remove(ev);
            }
        }
    }

     public void RandomizeEvent()
    {
        // if(thisBase != null)
        // {
            if (availableEvents.Count == 0)
            {
                Debug.LogError("Aucun événement disponible n'a conditionsMet à true.");
                return;
            }
            // Choisir un événement aléatoire parmi les événements disponibles.
            
            int randomIndex = Random.Range(0, availableEvents.Count);
            currentEvent = availableEvents[randomIndex];
            
            Debug.Log("On tire l'aléatoire" + randomIndex);
            Debug.Log("CurrentEvent =" + currentEvent.title);

            if (currentEvent.isMainEvent)
            {
                // On desactive les autres main events si le currentEvent est un mainEvent.
                foreach (Event unavailableEvents in events)
                {
                    if (currentEvent.isMainEvent)
                    {
                        unavailableEvents.conditionsMet = false;
                        currentEvent.conditionsMet = true;
                    }
                }
            }
    }

    public void EventEnabler()
    {
        // a appeler via un autre script où l'on clique oui ou non en précisant eventGenerator.currentEvent.yesChoice = true; ou  eventGenerator.currentEvent.noChoice = true;
        if(currentEvent.boolChoice)
        {
            if(currentEvent.completed && currentEvent.yesChoice)
            {
                foreach(Event anEvent in currentEvent.yesOutcomeEvents)
                {
                    anEvent.conditionsMet = true;
                }
            }

            if(currentEvent.completed && currentEvent.noChoice)
            {
                foreach(Event anEvent in currentEvent.noOutcomeEvents)
                {
                    anEvent.conditionsMet = true;
                }
            }
        }

        if(currentEvent.successOutcome != null)
        {
            if(CheckFirePower(currentEvent))
            {
                foreach(Event anEvent in currentEvent.successOutcome)
                {
                    anEvent.conditionsMet = true;
                }
            }
            else
            {
                foreach(Event anEvent in currentEvent.failOutcome)
                {
                    anEvent.conditionsMet = true;
                }
            }
        }
    }

    private bool CheckFirePower(Event anEvent){
        int firePowerGlobal = 0;
        int firePowerEnnemy = anEvent.firePower;

        foreach(Member member in selectedBase.membersInBase)
        {
            firePowerGlobal += member.firePower;
        }
        if(firePowerGlobal > firePowerEnnemy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OutcomeManager()
    {
        if(currentEvent.isEnd)
        {
            currentEvent.completed = true;
        }
    }

    public void EventItemHandler()
    {
        foreach(ItemData rewards in currentEvent.reward)
        {
            itemManager.AddItem(thisBase, rewards);
        }

        foreach(ItemData loss in currentEvent.loss)
        {
             itemManager.AddItem(thisBase, loss);
        }

        foreach(ItemData neededItems in currentEvent.neededItems)
        {
            foreach(ItemData ownedItems in thisBase.itemsInBase)
            {
            // A modifier, ici ptet créer une liste d'Items pour verifier si ya bien les items dans l'inventaire blblbla
                if(ownedItems == neededItems) 
                {
                    slot.emptyVisual = neededItems.journalVisualAvailable;
                }
                else
                {
                    slot.emptyVisual = neededItems.journalVisualUnAvailable;
                }
            }
        }
    }

     public void MemberHandler()
    {
        foreach(Member eventAddedMember in currentEvent.addedMember)
        {
            if(currentEvent.addedMember != null)
            {
                if(thisBase != null)
                {
                    teamManager.AddMember(thisBase, eventAddedMember);
                }
            }
        }
        
                
        foreach(Member eventRemovedMember in currentEvent.removedMember)
        {
            if(eventRemovedMember != null)
            {
                teamManager.AddMember(thisBase, eventRemovedMember);
            }
        }
        
        displayJournal.TeamDepopulate();
        displayJournal.TeamPopulate();
        displayJournal.PopulatePages();
    }
}