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

    List<Event> availableEvents = new List<Event>();

    void Start()
    {
        EventChecker();
        RandomizeEvent();
        BaseChecker();
    }

    public void BaseChecker()
    {
        foreach(Base aBase in teamManager.bases)
        {
            if(aBase.isSelected)
            {
                thisBase = aBase;
            }
        }
    }

    public void EventChecker()
    {
        foreach (Event ev in events)
        {
            // on ajoute tous ceux qui ont la conditionMet a cette liste trjs dans le but de faire un random
            if (ev.conditionsMet)
            {
                availableEvents.Add(ev);
                Debug.Log(ev.title + " a les conditionsMet");
            }
            if (ev.completed)
            {
                availableEvents.Remove(ev);
                Debug.Log(ev.title + " est completed");
            }
        }
    }

     public void RandomizeEvent()
    {
        foreach(Base aBase in teamManager.bases)
        {
            if(aBase.isSelected)
            {
                if (availableEvents.Count == 0)
                {
                    Debug.LogError("Aucun événement disponible n'a conditionsMet à true.");
                    return;
                }
                // Choisir un événement aléatoire parmi les événements disponibles
                int randomIndex = Random.Range(0, availableEvents.Count);
                currentEvent = availableEvents[randomIndex];

                if (currentEvent.isMainEvent)
                {
                    // On desactive les autres main events si le currentEvent est un maiinEvent
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
        }
    }

    public void EventEnabler()
    {
        // a appeler via un autre script où l'on clique oui ou non en précisant eventGenerator.currentEvent.yesChoice = true; ou  eventGenerator.currentEvent.noChoice = true;
        if(currentEvent.boolChoice)
        {
            if(currentEvent.completed && currentEvent.yesChoice)
            {
                foreach(Event yesOutcomeAvailableEvents in currentEvent.yesOutcomeEvents)
                {
                yesOutcomeAvailableEvents.conditionsMet = true;
                }
            }

            if(currentEvent.completed && currentEvent.noChoice)
            {
                foreach(Event noOutcomeAvailableEvents in currentEvent.noOutcomeEvents)
                {
                noOutcomeAvailableEvents.conditionsMet = true;
                }
            }
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
            foreach(ItemData ownedItems in itemManager.inventoryItems)
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
        foreach(Base aBase in teamManager.bases)
        {
            foreach(Member eventAddedMember in currentEvent.addedMember)
            {
                teamManager.AddMember(thisBase, eventAddedMember);
            }
                
            foreach(Member eventRemovedMember in currentEvent.removedMember)
            {
                    teamManager.RemoveMember(thisBase, eventRemovedMember);
            }
        }
    }
}


