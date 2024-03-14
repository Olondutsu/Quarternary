
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
    Page nextPage;
    public Slot slot;
    private ItemsManager itemManager;
    private TeamManager teamManager;
    private DisplayJournal displayJournal;

    public List <Event> anyEvents = new List<Event>();
    public List <Event> availableEvents = new List<Event>();
    public List<Event> doneEvents = new List<Event>();
    public List <Event> resetEvents = new List<Event>();

    void Start()
    {
        EventChecker();
        ResetEvent();

        displayJournal = transform.GetComponent<DisplayJournal>();
        itemManager = transform.GetComponent<ItemsManager>();
        teamManager = transform.GetComponent<TeamManager>();
    }

    public void ResetEvent()
    {
        foreach(Event ev in anyEvents)
        {
            if(ev.completed && ev.restartable)
            {
                resetEvents.Add(ev);
            }
        }

        if(resetEvents.Count > 0)
        {
            foreach(Event ev in resetEvents)
            {
                    ev.completed = false;
                    ev.yesChoice = false;
                    ev.noChoice = false;
            }
        }
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

    public void GetNextPage(Page aPage)
    {
        nextPage = aPage;
    }

    public void EventEnabler()
    {
        displayJournal.ImmediateNextEventPage(nextPage, currentEvent);

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

        int successCount = 0;

        foreach(Event succEvent in currentEvent.successOutcome)
        {
            successCount++;
        }

        if(successCount >= 1)
        {
        }
        else
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

        foreach(Member member in thisBase.membersInBase)
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

    public void EventItemHandler(Event anEvent)
    {
        int amount = 1;
        
        foreach(ItemData reward in anEvent.reward)
        {
            Debug.Log("Each reward " + reward);
            if(reward.isCommon)
            {
                amount = Random.Range(0, 5);
            }
            if(reward.isRare)
            {
                amount = Random.Range(0, 1);
            }
            
            if(amount > 0)
            {
                itemManager.AddItem(thisBase, reward, amount);
            }
        }

        foreach(ItemData loss in anEvent.loss)
        {
             itemManager.RemoveItem(thisBase, loss, amount);
        }

        foreach(ItemData neededItems in anEvent.neededItems)
        {
            foreach(Base.InventoryItem ownedItems in thisBase.itemsInBase)
            {
            // Base.InventoryItem existingItem = selectedBase.itemsInBase.Find(item => item.itemData == ownedItems);
            // A modifier, ici ptet créer une liste d'Items pour verifier si ya bien les items dans l'inventaire blblbla
                if(ownedItems.itemData == neededItems) 
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