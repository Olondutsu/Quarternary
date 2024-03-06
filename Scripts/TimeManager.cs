using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int currentDay = 1;
    public TeamManager teamManager;
    public DisplayJournal displayJournal;
    public int arrivalDayTime;
    public int returnDayTime;
    public bool travelChecked = false;

    public void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
        currentDay = 1;
    }

   public void NextDay()
    {
        currentDay += 1;
        if(travelChecked)
        {
            teamManager.AdjustTeamStats(0, 0, 0, 0);
             teamManager.LifeCheck();  
        }

        else
        {
            teamManager.AdjustTeamStats(-teamManager.feedRate, -teamManager.drinkRate, 0, 0);
            teamManager.LifeCheck();
        }

        //teamManager.DisplayTeam();
        displayJournal.NewDay();
        
        // Il faut que je fasse ça ailleurs
        // if(travelChecked)
        // {
        //     OnTimeTravelTeam();
        // }

        foreach(Base aBase in teamManager.bases)
        {
            aBase.journalLoaded = false;
        }
    }

   
    public void TravelCheck(int arrivalDay, int returnDay)
    {
        arrivalDayTime = arrivalDay;
        returnDayTime = returnDay;
    }

    public void OnTimeTravelTeam()
    {   
        Debug.Log("arrivalDayTime " + arrivalDayTime + "&& ReturnDayTime" + returnDayTime);
        if(travelChecked)
        {
            int membersCount = 0;
            bool arrived = false;

            // Member count to check if you still have one member in a base or if we skip days;
            foreach(Base aBase in teamManager.bases)
            {
                foreach(Member member in aBase.membersInBase)
                {
                    membersCount++;
                }
            }
            // Skip Days
            if(membersCount == 0)
            {
                Debug.Log("membersCount == 0");
                for(int i = currentDay; i <= arrivalDayTime; i++)
                {
                    Debug.Log("currentDay " + currentDay + "&& arrivalDayTime" + arrivalDayTime + "i = " + i );
                    // Display l'event du lieu d'arriver
                    NextDay();
                    if( i >= arrivalDayTime)
                    {

                        arrived = true;
                        Debug.Log("Arrivé sur les lieux de l'event, jour = " + currentDay);
                        OnTimeTravelTeam();
                    }
                }
                
                if(arrived)
                {
                    for(int i = currentDay; i <= returnDayTime; i++)
                    {
                        Debug.Log("currentDay " + currentDay + "&& ReturnDayTime" + returnDayTime + "i = " + i );
                        NextDay();

                        if(i >= returnDayTime)
                        {
                            foreach(Member travelingMember in teamManager.travelingMembers)
                            {
                                teamManager.AddMember(travelingMember.baseComingFrom , travelingMember);
                                teamManager.travelingMembers.Remove(travelingMember);
                                
                                Debug.Log("retour à la baraque " + currentDay);
                                travelChecked = false;
                            }
                        }
                    }
                }
            }
            // If you have teammates in somes Bases
            else
            {
                if(currentDay == arrivalDayTime)
                {
                    // Display de l'event sur place ou ajout d'un texte jcp
                }
                if(currentDay == returnDayTime)
                {
                    foreach(Member travelingMember in teamManager.travelingMembers)
                    {
                        teamManager.AddMember(travelingMember.baseComingFrom , travelingMember);
                        teamManager.travelingMembers.Remove(travelingMember);
                        travelChecked = false;
                    }
                }
            }
        }
    }
}