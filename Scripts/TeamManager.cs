using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public List<Member> teamMembers = new List<Member>(); 

    public void AddMember(Member newMember)
    {
        teamMembers.Add(newMember);
    }

    public void RemoveMember(Member memberToRemove)
    {
        teamMembers.Remove(memberToRemove);
    }

    public void AdjustTeamThirst(int amount)
    {
        foreach (Member member in teamMembers)
        {
            member.thirst += amount;
        }
    }

    // Méthode pour ajuster la faim de tous les membres de l'équipe
    public void AdjustTeamHunger(int amount)
    {
        foreach (Member member in teamMembers)
        {
            member.hunger += amount;
        }
    }

    public void AdjustTeamPhysicalHealth(int amount)
    {
        foreach (Member member in teamMembers)
        {
            member.physicalHealth += amount;
        }
    }

    // Méthode pour ajuster la faim de tous les membres de l'équipe
    public void AdjustTeamMentalHealth(int amount)
    {
        foreach (Member member in teamMembers)
        {
            member.mentalHealth += amount;
        }
    }

    
}