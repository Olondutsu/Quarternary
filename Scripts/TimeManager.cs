using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int currentDay = 1;
    public TeamManager teamManager;
    public int arrivalDayTime;
    public int returnDayTime;
    public bool travelChecked = false;

   public void NextDay()
    {
    currentDay += 1;
    teamManager.AdjustTeamStats(-teamManager.feedRate, -teamManager.drinkRate, 0, 0);
    teamManager.LifeCheck();
    teamManager.DisplayTeam();

        if(travelChecked)
        {
            OnTimeTravelTeam();
        }
    }
   
    public void TravelCheck(int arrivalDay, int returnDay)
    {
        arrivalDayTime = arrivalDay;
        returnDayTime = returnDay;
    }

    public void OnTimeTravelTeam()
    {   
        if(travelChecked)
        {
            if(currentDay >= arrivalDayTime)
            {
                // Display l'event du lieu d'arriver
            }

            if(currentDay >= returnDayTime)
            {
                foreach(Member travelingMember in teamManager.travelingMembers)
                {
                    teamManager.AddMember(travelingMember);
                    teamManager.travelingMembers.Remove(travelingMember);
                    travelChecked = false;
                }
            }
        }
    }
}