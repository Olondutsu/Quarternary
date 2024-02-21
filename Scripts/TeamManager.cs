using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.VisualScripting;
using UnityEngine;

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
    void Start()
    {
        RandomizeLeader();
    }

    // BASIC LIST MANIPULATION
    public void AddMember(Base aBase, Member addMember)
    {
        aBase.membersInBase.Add(addMember);
        addMember.isInTeam = true;
        DisplayTeam();
        DisplayBases();
    }
    public void RemoveMember(Base aBase, Member rmvMember)
    {
        aBase.membersInBase.Remove(rmvMember);
        rmvMember.isInTeam = false;
        DisplayTeam();
        DisplayBases();
    }

    public void AddBase(Base aBase)
    {
        bases.Add(aBase);
    }

    public void RemoveMember(Base aBase)
    {
        bases.Remove(aBase);
    }

    public void PopulationCheck()
    {
        if(selectedBase.membersInBase.Count >= 4)
        {

        }
    }

    // Pour build la team dès le début.
    public void PickUpTeamMember()
    {
        // ajouter ici tout le truc du début où on peut chosiir entre 2 Member qui s'ajoutent a notre team.
        int rand1 = Random.Range(0, everyMembers.Count);
        int rand2 = Random.Range(0, everyMembers.Count);
        int rand3 = Random.Range(0, everyMembers.Count);
        
        Member firstChoiceMember = everyMembers[rand1];
        Member secondChoiceMember = everyMembers[rand2];
        Member thirdChoiceMember = everyMembers[rand3];

        proposedMembers.Add(firstChoiceMember);
        proposedMembers.Add(secondChoiceMember);
        proposedMembers.Add(thirdChoiceMember);

        foreach(Member proposedMember in proposedMembers)
        {
            // Ajouter une prefab ici ?
            // Puis rechercher le slot pour chaque proposedMember;
            // slot.image.sprite = firstChoiceMember.journalVisual;
        }
        // Ajouter le visuel pour les 3 joueurs, genre un Prefab cliquable.

        if(firstChoiceMember.selected)
        {
            selectedBase.membersInBase.Add(firstChoiceMember);
        }

        if(secondChoiceMember.selected)
        {
            selectedBase.membersInBase.Add(secondChoiceMember);
        }

        if(thirdChoiceMember.selected)
        {
            selectedBase.membersInBase.Add(thirdChoiceMember);
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
            int randomIndex = Random.Range(0, aBase.membersInBase.Count);

            aBase.squadLeader = aBase.membersInBase[randomIndex];
            squadLeaders.Add(aBase.squadLeader);
        }
    }

    public void DisplayBases()
    {
        int xOffset = 0;

        foreach(Base aBase in bases)
        {
            if(aBase.membersInBase != null)
            {
                GameObject go;
                xOffset += 3;

                go = Instantiate(basePrefab, (parentDisplayBases.transform)) as GameObject;

                go.transform.SetParent(parentDisplayBases.transform);

                foreach(Transform child in parentDisplayBases.transform)
                {
                    // Instantiate un Prefab ici d'un truc clickable qui contient chaque leader ?
                    Slot teamSlot = child.GetComponent<Slot>();
                    
                    if(teamSlot != null)
                    {

                        teamSlots.Add(teamSlot);

                        if(aBase.squadLeader != null)
                        {
                            foreach(Slot teamSlot2 in teamSlots)

                            teamSlot2.image.sprite = aBase.squadLeader.journalVisual;
                            aBase.available = true;
                        }
                    }
                }
            }
        }
    }

    public void OnLeaderClick()
    {
        Slot slot = transform.GetComponent<Slot>();

        slot.slotBase.isSelected = true;
        selectedBase = slot.slotBase;
        displayJournal.selectedBase = slot.slotBase;
        eventGenerator.thisBase = slot.slotBase;
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
        foreach(Base aBase in bases)
        {
            foreach(Member member in aBase.membersInBase)
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
    }

    // ACTIONS
    public void OnTravel()
    {
        foreach(Base aBase in bases)
        {
            foreach(Member selectedMember in selectedMembers)
            {
                RemoveMember(aBase, selectedMember);
                travelingMembers.Add(selectedMember);
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