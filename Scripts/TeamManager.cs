using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public int feedRate;
    public int drinkRate;

    public List<Member> teamMembers = new List<Member>(); 

    public void AddMember(Member addMember)
    {
        teamMembers.Add(addMember);

    }
    public void RemoveMember(Member rmvMember)
    {
        teamMembers.Remove(rmvMember);
    }
    
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
    // AdjustTeam, for the whole team, good at the end of a turn
    
    public void FeedMember(Member member, int feedRate)
    {
        member.hunger += feedRate;
    }

    public void UnthirstMember(Member member, int drinkRate)
    {
        member.hunger += drinkRate;
    }    

    public void DisplayTeam()
    {
        foreach (Member member in teamMembers)
        {
            member.gameVisual.SetActive(true);
        }
    }
}