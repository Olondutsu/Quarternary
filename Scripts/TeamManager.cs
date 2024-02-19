using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public int feedRate;
    public int drinkRate;

    public List<Member> teamMembers = new List<Member>(); 
    public List<Member> selectedMembers = new List<Member>();
    public List<Member> travelingMembers = new List<Member>();

    public MapManager mapManager;
    public TimeManager timeManager;


    // BASIC LIST MANIPULATION
    public void AddMember(Member addMember)
    {
        teamMembers.Add(addMember);
        addMember.isInTeam = true;
        DisplayTeam();

    }
    public void RemoveMember(Member rmvMember)
    {
        teamMembers.Remove(rmvMember);
        rmvMember.isInTeam = false;
        DisplayTeam();
    }

    // DISPLAY & UPDATE VISUALS
    public void DisplayTeam()
    {
        foreach (Member member in teamMembers)
        {
            // member.gameVisual.SetActive(true);
            OnSelectionClick(member);
        }
    }

    public void LifeCheck()
    {
        foreach (Member member in teamMembers)
        {
            if(member.hunger == 0|| member.thirst == 0 || member.physicalHealth == 0 || member.mentalHealth == 0)
            {
                teamMembers.Remove(member);
            }
        }
    }
    
    // PERSONAL NEEDS
    public void AdjustTeamStats(int hungerAmount, int thirstAmount, int physicalHealthAmount, int mentalHealthAmount)
    {
        foreach (Member member in teamMembers)
        {
            member.hunger += hungerAmount;
            member.thirst += thirstAmount;
            member.physicalHealth += physicalHealthAmount;
            member.mentalHealth += mentalHealthAmount;
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
        foreach(Member member in teamMembers)
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
        foreach(Member selectedMember in selectedMembers)
        {
            RemoveMember(selectedMember);
            travelingMembers.Add(selectedMember);
            selectedMember.isInTeam = false;
            MemberDayCount();
            timeManager.travelChecked = true;
        }
    }

    public void MemberDayCount()
    {
        int arrivalDay = timeManager.currentDay + mapManager.onTravel;
        int returnDay = timeManager.currentDay + mapManager.onTravel + mapManager.onTravel;
        timeManager.TravelCheck(arrivalDay, returnDay);
    }

}