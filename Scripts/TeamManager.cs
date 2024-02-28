using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public int feedRate;
    public int drinkRate;

    public List<Slot> teamSlots = new List<Slot>();
    public List<Base> bases = new List<Base>();
    public List<Member> proposedMembers = new List<Member>();
    public List<Member> selectedMembers = new List<Member>();
    public List<Member> travelingMembers = new List<Member>();
    public List<Member> everyMembers = new List<Member>();
    public List<Member> squadLeaders = new List<Member>();
    public Base selectedBase;
    public MapManager mapManager;
    public TimeManager timeManager;
    public DisplayJournal displayJournal;
    public EventGenerator eventGenerator;
    public GameObject parentDisplayBases;
    public GameObject basePrefab;
    public GameObject parentPickUp;
    public GameObject pickUpPrefab;

    void Start()
    {
        OnBegin();
    }

    // BASIC LIST MANIPULATION
    public void AddMember(Base aBase, Member addMember)
    {
        Debug.Log("AddMember appelé");
        
        if(aBase == null)
        {
            Debug.Log("aBase est bien null, dans AddMember");

            GameObject go;

            go = Instantiate(basePrefab, (parentDisplayBases.transform)) as GameObject;

            go.transform.SetParent(parentDisplayBases.transform);

            Base newBase = go.GetComponent<Base>();

            CreateNewBase(newBase);
        }

        aBase.membersInBase.Add(addMember);


        addMember.isInTeam = true;

        //DisplayTeam();
        RandomizeLeader();
        CheckPopulation(); 
    }

    public void RemoveMember(Base aBase, Member rmvMember)
    {
        Debug.Log("RemoveMemberappelé");
        aBase.membersInBase.Remove(rmvMember);
        rmvMember.isInTeam = false;
        //DisplayTeam();
        CheckPopulation(); 
    }

    public void CreateNewBase(Base aBase)
    {
        Debug.Log("CreateNewBase appelé");
        // Base newBase = new Base();
        bases.Add(aBase);
        RandomizeLeader();
    }

    public void RemoveBase(Base baseToRemove)
    {
        Debug.Log("RemoveBase appelé");
        bases.Remove(baseToRemove);


        // Delete le visuel ? Ou plutôt mettre à jour avec le vide;
        Slot slot = baseToRemove.transform.GetComponent<Slot>();
        Base aBase = baseToRemove.transform.GetComponent<Base>();

        slot.enabled = false;
        aBase.enabled = false;
        Destroy(baseToRemove.gameObject);
    }

    public void CheckPopulation()
    {
        if(bases.Count == 0)
        {
            Debug.Log("CheckPopulation : base Count == 0");
            foreach(Transform child in parentDisplayBases.transform)
            {
                Base aBase = child.GetComponent<Base>();
                Slot slot = child.GetComponent<Slot>();

                if(aBase != null)
                {
                    Debug.Log("CheckPopulation : aBase est different de nul");

                    slot.slotBase = aBase;
                    selectedBase = aBase;
                    displayJournal.selectedBase = aBase;
                    eventGenerator.thisBase = aBase;
                    
                    // CreateNewBase(aBase);

                    break;
                }
                else
                {
                    Debug.Log("CheckPopulation : aBase = null du chckpopulation");
                }
            }
        }

        foreach(Transform child in parentDisplayBases.transform)
        {
            Base aBase = child.GetComponent<Base>();

                if(aBase != null)
                {
                    // a deplacer car ça fout un boordel au niveau de la creation de base
                    if (aBase.membersInBase.Count >= 4)
                    {
                        Debug.Log("On crée une nouvelle Base dans le checkpopulation");
                        CreateNewBase(aBase); 
                        break; 
                    }
                    else if (aBase.membersInBase.Count == 0 && aBase.displayed)
                    {
                        Debug.Log("On suppr une Base dans le checkpopulation");
                        RemoveBase(aBase); // Supprimer la base si elle est vide
                        break; // Sortir de la boucle après la suppression de la base
                    }
                }
        }
        // DisplayBases();
    }

    public void OnBegin()
    {
        // A revoir toute cette phase, il afut qu'on trouve le component Base de notre truc
        Base newBase = new Base();
        
        GameObject go;

        go = Instantiate(basePrefab, (parentDisplayBases.transform)) as GameObject;

        go.transform.SetParent(parentDisplayBases.transform);
            
        newBase = go.GetComponent<Base>();

        bases.Add(newBase);
    
        Debug.Log("OnBeginESt fait ça appelle jsute 2 methode mtn");
        
        // DisplayBases();

        foreach(Transform child in parentDisplayBases.transform)
        {
            Debug.Log("Pour chaque enfant "+ child + "de" + parentDisplayBases.transform);

            Base aBase = child.GetComponent<Base>();
            Slot slot = child.GetComponent<Slot>();

            if(aBase != null)
            {
                Debug.Log("aBase est different de nul OnBegin");

                slot.slotBase = aBase;
                selectedBase = aBase;
                displayJournal.selectedBase = aBase;
                eventGenerator.thisBase = aBase;
                
                // CreateNewBase(aBase);

            }
            else if (aBase.membersInBase.Count == 0)
            {
                Destroy(aBase.gameObject);
                Debug.Log("On destroy une aBase dans le gameobject car le membersInBase == 0");
            }
            if (child == null)
            {
                Debug.Log("Child est null dans OnBegin la boucle");
            }
        }

        CheckPopulation();

        PickUpTeamMember();
    }

    // Pour build la team dès le début.
    public void PickUpTeamMember()
    {
        Debug.Log("on calcul le random pickup");
        
        ShuffleList(everyMembers);

        proposedMembers.Add(everyMembers[0]);
        proposedMembers.Add(everyMembers[1]);
        proposedMembers.Add(everyMembers[2]);

        int xOffset = 0;

        foreach(Member proposedMember in proposedMembers)
        {
            Debug.Log("Pour chaque des joueurs proposés (normalement 3), on créer un GO gestion de slot des prefab du pickupteammember");
            GameObject go;

            xOffset += 3;

            go = Instantiate(pickUpPrefab, (parentPickUp.transform)) as GameObject;

            go.transform.SetParent(parentPickUp.transform);

            Slot goSlot = go.GetComponent<Slot>();
            goSlot.image.sprite = proposedMember.journalVisual;
            goSlot.slotMember = proposedMember;

            foreach(Transform child in parentPickUp.transform)
            {
                // Base aBase = child.GetComponent<Base>();
                Slot slot = child.GetComponent<Slot>();
                
                slot.slotBase = selectedBase;
                
                Debug.Log("Pour chaque enfant des parentsPickUp pour chaque joueurs proposés (normalement 3) gestion de slot du pickupteammember");
                
            }
            proposedMember.isPickUp = true;
        }

    }

    public void OnPickUpClick()
    {
        //Destroy
        foreach(Transform child in parentPickUp.transform)
        {
            Slot slot = child.GetComponent<Slot>();
            UnityEngine.UI.Image image = child.GetComponent<UnityEngine.UI.Image>();
            slot.enabled = false;

            Destroy(image);

            Destroy(child.gameObject);
        }
        foreach(Member proposedMember in proposedMembers)
        {
            proposedMember.isPickUp = false;
        }

    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // // DISPLAY & UPDATE VISUALS
    public void DisplayTeam()
    {
        foreach (Member member in selectedBase.membersInBase)
        {
            // member.gameVisual.SetActive(true);
            OnSelectionClick(member);
        }
    }

    public void RandomizeLeader()
    {
        // ici on a bien raison d'utiliser le foreach abase
        foreach(Base aBase in bases)
        {
            if(aBase.membersInBase.Count != 0)
            {
                if(aBase.squadLeader == null)
                {
                    Debug.Log("For each Base in bases du RandomizeLeader");
                    int randomIndex = Random.Range(0, aBase.membersInBase.Count);

                    aBase.squadLeader = aBase.membersInBase[randomIndex];
                    squadLeaders.Add(aBase.squadLeader);

                    foreach(Transform child in parentDisplayBases.transform)
                    {
                        Slot slot = child.GetComponent<Slot>();
                        
                        slot.image.sprite = aBase.squadLeader.journalVisual;

                    }
                }
                else
                {
                    Debug.Log("There is already a leader for this squad");
                }

            }
        }
    }

    public void DisplayBases()
    {
        int xOffset = 0;

        foreach(Base aBase in bases)
        {
            if(aBase != null)
            {
                Debug.Log("ABas.membersInBase n'est pas null");
                if(!aBase.displayed)
                {
                    Debug.Log("ABase n'est pas displayed dans displayBase");
                    GameObject go;
                    xOffset += 3;

                    go = Instantiate(basePrefab, (parentDisplayBases.transform)) as GameObject;

                    go.transform.SetParent(parentDisplayBases.transform);
                    
                    aBase.displayed = true;

                    foreach(Transform child in parentDisplayBases.transform)
                    {
                        // Instantiate un Prefab ici d'un truc clickable qui contient chaque leader ?
                        Slot teamSlot = child.GetComponent<Slot>();
                        Base teamBase = child.GetComponent<Base>();

                        
                        if(teamBase != null && teamBase.displayed)
                        {
                            selectedBase = teamBase;
                        
                            if (teamBase.membersInBase.Count > 0 && teamBase.displayed)
                            {

                            }
                            else
                            {
                                Destroy(child);
                                bases.Remove(teamBase);
                            }
                        }
                        else
                        {
                            teamSlot.image.enabled = false;
                            teamSlot.enabled = false;
                            teamBase.enabled = false;
                            Destroy(child);
                            bases.Remove(teamBase);
                        }

                        if(teamSlot != null)
                        {
                            teamSlots.Add(teamSlot);

                            if(aBase.squadLeader != null)
                            {
                                foreach(Slot teamSlot2 in teamSlots)
                                {
                                    teamSlot2.slotBase = aBase;
                                    teamSlot2.image.sprite = aBase.squadLeader.journalVisual;
                                    aBase.available = true;
                                }
                            }
                        }
                        if(teamBase.membersInBase != null && teamBase.displayed)
                        {
                            
                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                }
            }
            else
            {
                GameObject go;
                xOffset += 3;

                go = Instantiate(basePrefab, (parentDisplayBases.transform)) as GameObject;
                go.transform.SetParent(parentDisplayBases.transform);

                foreach(Transform child in parentDisplayBases.transform)
                {
                    Base teamBase = child.GetComponent<Base>();

                    bases.Add(teamBase);
                    break;
                }
                break;
                // bases.Remove(aBase);
                // CheckPopulation();
            }
        }
    }

    public void OnLeaderClick()
    {
        Slot slot = parentDisplayBases.transform.GetComponentInChildren<Slot>();

        if(selectedBase != slot.slotBase)
        {
            slot.slotBase.isSelected = !slot.slotBase.isSelected;
            selectedBase = slot.slotBase;
            displayJournal.selectedBase = slot.slotBase;
            eventGenerator.thisBase = slot.slotBase;

            if(!selectedBase.journalLoaded)
            {
                // displayJournal.ResetJournal();
                eventGenerator.RandomizeEvent();
                displayJournal.ResetJournal();
                displayJournal.InitializePages();
            }
            // displayJournal.TeamPopulate();
            
        }
    }

    public void LifeCheck()
    {
        foreach (Base aBase in bases)
        {
            foreach (Member member in aBase.membersInBase)
            {
                if(member.hunger == 0|| member.thirst == 0 || member.physicalHealth == 0 || member.mentalHealth == 0)
                {
                    aBase.membersInBase.Remove(member);
                }
            }
        }
    }
    
    // PERSONAL NEEDS
    public void AdjustTeamStats(int hungerAmount, int thirstAmount, int physicalHealthAmount, int mentalHealthAmount)
    {
        foreach (Base aBase in bases)
        {
            foreach (Member member in aBase.membersInBase)
            {
                member.hunger += hungerAmount;
                member.thirst += thirstAmount;
                member.physicalHealth += physicalHealthAmount;
                member.mentalHealth += mentalHealthAmount;
            }
        }
    }

    public void FeedMember(Member member, int feedRate)
    {
        member.hunger += feedRate;
    }

    public void UnthirstMember(Member member, int drinkRate)
    {
        member.hunger += drinkRate;
    }    

    public void HealMember(Member member)
    {
        member.physicalHealth += 5;
        member.mentalHealth += 5;
    }

    // SELECTION
    public void OnSelectionClick(Member selected)
    {
        selected.selected = !selected.selected;
        if(selected.selected)
        {
            SelectMembers(selected);
        }
    }


    public void SelectMembers(Member selected)
    {
        foreach(Member member in selectedBase.membersInBase)
        {
            if(member.selected)
            {
                selectedMembers.Add(selected);
                // member.journalVisualHighlight.SetActive(true);

            }
            else
            {
                selectedMembers.Remove(selected);
                // member.journalVisualHighlight.SetActive(false);
            }
        }
    }

    // ACTIONS
    public void OnTravel()
    {
        foreach(Base aBase in bases)
        {
            foreach(Member selectedMember in aBase.selectedMembers)
            {
                RemoveMember(aBase, selectedMember);
                aBase.membersInTravel.Add(selectedMember);
                selectedMember.isInTeam = false;
                MemberDayCount();
                timeManager.travelChecked = true;
            }
        }
    }

    public void MemberDayCount()
    {
        int arrivalDay = timeManager.currentDay + mapManager.onTravel;
        int returnDay = timeManager.currentDay + mapManager.onTravel + mapManager.onTravel;
        timeManager.TravelCheck(arrivalDay, returnDay);
    }

}