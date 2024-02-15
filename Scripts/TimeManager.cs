using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int currentDay = 0;
    public TeamManager teamManager;

   public void NextDay()
   {
    currentDay += 1;
    teamManager.AdjustTeamStats(-teamManager.feedRate, -teamManager.drinkRate,-teamManager.feedRate, -teamManager.drinkRate);
    teamManager.LifeCheck();
    teamManager.DisplayTeam();
   }
}