using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int currentDay = 1;
    MapManager mapManager;
    public TeamManager teamManager;
    public DisplayJournal displayJournal;
    public EventGenerator eventGenerator;
    public Base selectedBase;
    public int arrivalDayTime;
    public int returnDayTime;
    public bool travelChecked = false;

    public void Start()
    {
        teamManager = FindObjectOfType<TeamManager>();
        eventGenerator = FindObjectOfType<EventGenerator>();
        mapManager = FindObjectOfType<MapManager>();
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
            if(currentDay > 1)
            {
                teamManager.AdjustTeamStats(-teamManager.feedRate, -teamManager.drinkRate, 0, 0);
                teamManager.LifeCheck();
                
            }
        }

        if(currentDay >1)
        {
            OverTimeTravelCheck();
        };
        displayJournal.NewDay();
        eventGenerator.ResetEvent();
        
    }

    public void SkipDays()
    {       
        if(selectedBase.membersInTravel[0].arrived)
        {
            Base aBase = selectedBase.membersInTravel[0].baseComingFrom;

            Debug.Log("if(selectedBase.membersInTravel[0].arrived)");
            for(int i = currentDay; i <= returnDayTime; i++)
            {
                    Debug.Log("for(int i = currentDay; i < returnDayTime; i++)");
                if(i >= returnDayTime)
                {
                    Debug.Log("if(i >= returnDayTime)");
                    OverTimeTravelCheck();
                    // for(int y = 0; 0 < selectedBase.membersInTravel.Count;y++)
                    // {
                    //     Member travelingMember = selectedBase.membersInTravel[0];

                    //     // mapManager.MapEventArrivalTrigger();
                    //     // teamManager.AddMember(selectedBase , travelingMember);
                    //     selectedBase.membersInTravel.Remove(travelingMember);
                        
                    //     Debug.Log("selectedBase.membersInTravel.Count  = "+ selectedBase.membersInTravel.Count + "y = " + y + "member = " + travelingMember );

                    //     Debug.Log("retour à la baraque " + currentDay);
                    //     travelChecked = false;
                    //     travelingMember.arrived = false;
                    //     if(selectedBase.membersInTravel.Count == 0)
                    //     {
                    //         break;
                    //     }
                    // }
                }
                else
                {
                    Debug.Log("else du if(i >= returnDayTime)");
                    NextDay();
                }
            }
        }
        else
        {
            for(int i = currentDay; i <= arrivalDayTime; i++)
            {
                Debug.Log("currentDay " + currentDay + "&& arrivalDayTime" + arrivalDayTime + "i = " + i );
                if( i >= arrivalDayTime)
                {
                    
                    foreach(Member member in selectedBase.membersInTravel)
                    {
                        member.arrived = true;
                    }
                    mapManager.MapEventArrivalTrigger();
                    mapManager.mapUI.SetActive(false);
                    
                    Debug.Log("Arrivé sur les lieux de l'event, jour = " + currentDay);
                    break;
                    // OnTimeTravelTeam();
                }
                else
                {
                    NextDay();
                }
            }
            Debug.Log("else du if(selectedBase.membersInTravel[0].arrived)");
        }
    }
    
    public void TravelCheck(int arrivalDay, int returnDay)
    {
        arrivalDayTime = arrivalDay;
        returnDayTime = returnDay;
    }

    public void OverTimeTravelCheck()
    {
        if(currentDay == arrivalDayTime)
        {
            mapManager.MapEventArrivalTrigger();
            Debug.Log("if(currentDay == arrivalDayTime)");
            foreach(Member member in selectedBase.membersInTravel)
            {
                member.arrived = true;
            }

            
        }
        
        if(currentDay == returnDayTime)
        {
            Debug.Log("if(currentDay == returnDayTime)");
            for(int i = 0 ;selectedBase.membersInTravel.Count > 0 ; i++)
            {
                if(selectedBase.membersInTravel.Count > 0 )
                {
                    Debug.Log("selectedBase.membersInTravel.Count  = "+ selectedBase.membersInTravel.Count + "i = " + i + "member = " + selectedBase.membersInTravel[0] );
                    Member travelingMember = selectedBase.membersInTravel[0];

                    teamManager.AddMember(selectedBase , travelingMember);
                    selectedBase.membersInTravel.Remove(travelingMember);
                    travelChecked = false;
                    travelingMember.arrived = false;
                }
                else{break;}
            }
        }
    }
    public void OnTimeTravelTeam()
    {   
        Debug.Log("arrivalDayTime " + arrivalDayTime + "&& ReturnDayTime" + returnDayTime);
        int membersCount = 0;

        // Member count to check if you still have one member in a base or if we skip days;
        foreach(Base aBase in teamManager.bases)
        {
            foreach(Member member in aBase.membersInBase)
            {
                membersCount++;
            }
        }
        if(membersCount > 0)
        {
            OverTimeTravelCheck();
        }
        else
        {
            SkipDays();
        }
    }
}
    // public void OnTimeTravelTeam()
    // {   
    //     Debug.Log("arrivalDayTime " + arrivalDayTime + "&& ReturnDayTime" + returnDayTime);
    //     if(travelChecked)
    //     {
    //         int membersCount = 0;

    //         // Member count to check if you still have one member in a base or if we skip days;
    //         foreach(Base aBase in teamManager.bases)
    //         {
    //             foreach(Member member in aBase.membersInBase)
    //             {
    //                 membersCount++;
    //             }
    //         }
    //         // Skip Days
    //         if(membersCount == 0)
    //         {
    //             Debug.Log("membersCount == 0");

                
    //             if(selectedBase.membersInTravel[0].arrived)
    //             {
    //                 Base aBase = selectedBase.membersInTravel[0].baseComingFrom;

    //                 Debug.Log("if(selectedBase.membersInTravel[0].arrived)");
    //                 for(int i = currentDay; i <= returnDayTime; i++)
    //                 {
    //                      Debug.Log("for(int i = currentDay; i < returnDayTime; i++)");
    //                     if(i >= returnDayTime)
    //                     {
    //                         Debug.Log("if(i >= returnDayTime)");
    //                         for(int y = 0; 1 < selectedBase.membersInTravel.Count;y++)
    //                         {
    //                             Member travelingMember = selectedBase.membersInTravel[0];

    //                             // mapManager.MapEventArrivalTrigger();
    //                             teamManager.AddMember(selectedBase , travelingMember);
    //                             selectedBase.membersInTravel.Remove(travelingMember);
                                
    //                             Debug.Log("selectedBase.membersInTravel.Count  = "+ selectedBase.membersInTravel.Count + "y = " + y + "member = " + travelingMember );

    //                             Debug.Log("retour à la baraque " + currentDay);
    //                             travelChecked = false;
    //                             travelingMember.arrived = false;
    //                         }
    //                     }
    //                     else
    //                     {
    //                         Debug.Log("else du if(i >= returnDayTime)");
    //                         NextDay();
    //                     }
    //                 }
    //             }
    //             else
    //             {
    //                 for(int i = currentDay; i <= arrivalDayTime; i++)
    //                 {
    //                     Debug.Log("currentDay " + currentDay + "&& arrivalDayTime" + arrivalDayTime + "i = " + i );
    //                     if( i >= arrivalDayTime)
    //                     {
                            
    //                         foreach(Member member in selectedBase.membersInTravel)
    //                         {
    //                             member.arrived = true;
    //                         }
    //                         mapManager.MapEventArrivalTrigger();
    //                         mapManager.mapUI.SetActive(false);
                            
    //                         Debug.Log("Arrivé sur les lieux de l'event, jour = " + currentDay);
    //                         break;
    //                         // OnTimeTravelTeam();
    //                     }
    //                     else
    //                     {
    //                         NextDay();
    //                     }
    //                 }
    //                 Debug.Log("else du if(selectedBase.membersInTravel[0].arrived)");
    //             }
    //         }
    //         // If you have teammates in somes Bases
    //         else
    //         {
    //             if(currentDay == arrivalDayTime)
    //             {
    //                 mapManager.MapEventArrivalTrigger();
    //             }
                
    //             if(currentDay == returnDayTime)
    //             {
    //                 for(int i = 1; i < selectedBase.membersInTravel.Count; i++)
    //                 {
    //                     Debug.Log("selectedBase.membersInTravel.Count  = "+ selectedBase.membersInTravel.Count + "i = " + i + "member = " + selectedBase.membersInTravel[i] );
    //                     Member travelingMember = selectedBase.membersInTravel[0];

    //                     teamManager.AddMember(selectedBase , selectedBase.membersInTravel[i]);
    //                     selectedBase.membersInTravel.Remove(selectedBase.membersInTravel[i]);
    //                     travelChecked = false;
    //                     selectedBase.membersInTravel[i].arrived = false;
    //                 }
    //             }
    //         }
    //     }
    // }
// }