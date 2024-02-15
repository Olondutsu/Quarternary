using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public int feedRate;
    public int drinkRate;
    public List<Member> teamMembers = new List<Member>(); 

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
            member.gameVisual.SetActive(true);
        }
    }

    public void LifeCheck()
    {
        foreach (Member member in teamMembers)
        {
            if(member.hunger > 1 || member.thirst > 1 || member.physicalHealth > 1 || member.mentalHealth > 1)
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
}