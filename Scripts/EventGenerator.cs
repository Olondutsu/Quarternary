using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventGenerator : MonoBehaviour
{
    public Text journalText;
    public Event[] events;
    public Event currentEvent;
    private Event pastEvent;
    public bool outcomeDisplayed;
    private ItemsManager itemManager;
    private TeamManager teamManager;

     public void RandomizeEvent()
    {
        // Filtrer les événements disponibles, faire une liste pour pouvoir ensuite ajouter, to ut ça dans le but de créer un randoom à partir de cette liste
        List<Event> availableEvents = new List<Event>();

        foreach (Event ev in events)
        {
            // on ajoute tous ceux qui ont la conditionMet a cette liste trjs dans le but de faire un random
            if (ev.conditionsMet)
            {
                availableEvents.Add(ev);
            }
        }

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

    public void DisplayEvent()
    {
        // a deplacer dans le journalDisplay (ou pas)
        if (currentEvent.conditionsMet)
        {
            journalText.text = $"Journal:\n{currentEvent.title}: {currentEvent.description}";
            currentEvent.completed = true;
            currentEvent = pastEvent;
            EventEnabler();
        }
    }

    public void EventItemHandler()
    {
        foreach(ItemData rewards in currentEvent.reward)
        {
            itemManager.AddItem(rewards);
        }

        foreach(ItemData loss in currentEvent.loss)
        {
             itemManager.AddItem(loss);
        }

        foreach(ItemData neededItems in currentEvent.neededItems)
        {
                foreach(ItemData ownedItems in itemManager.inventoryItems)
                {
                // A modifier, ici ptet créer une liste d'Items pour verifier si ya bien les items dans l'inventaire blblbla
                    if(ownedItems == neededItems) 
                    {
                        neededItems.journalVisualAvailable.SetActive(true);
                        neededItems.journalVisualAvailable.SetActive(false);
                    }
                    else
                    {
                        neededItems.journalVisualAvailable.SetActive(false);
                        neededItems.journalVisualUnAvailable.SetActive(true);
                    }
                }
        }
    }

     public void MemberHandler()
    {
        foreach(Member eventAddedMember in currentEvent.addedMember)
        {
            teamManager.AddMember(eventAddedMember);
        }
        
        foreach(Member eventRemovedMember in currentEvent.removedMember)
        {
            teamManager.RemoveMember(eventRemovedMember);
        }
    }
}


