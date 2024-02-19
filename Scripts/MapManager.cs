using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    List<MapCase> mapCases = new List<MapCase>();

    public MapCase mapCase;
    public MapCase playerCase;
    public TeamManager teamManager;
    
    public Text travelText;

    public int onTravel;

    // THIS PART HAVE TO BEEN WORKED AGAIN
    
    public void RandomizeCasesEvent()
    {
        // foreach(MapCase mapCase in mapCases)
        // {
            
        //     foreach(MapEvent mapEvent in mapCases.mapEvents)
        //     {
        //     }
        // }
    }

    public void CalculateMapDistance()
    {
        foreach(MapCase mapCase in mapCases)
        {
            if(mapCase.isClicked)
            {
            int xDistance = Mathf.Abs(playerCase.XCoordinate - mapCase.XCoordinate);
            int yDistance = Mathf.Abs(playerCase.YCoordinate - mapCase.YCoordinate);
            mapCase.travelTime = xDistance + yDistance;
            onTravel = mapCase.travelTime/4 ;    
            DisplayTravel(onTravel);
            // onTravel = onTravel;
            }
        }
    }
    
    // Display le TravelTime
    public void DisplayTravel(int travelTime)
    {
        travelText.text = travelTime + "DAYS";
    }
    
    public void OnClickTravel()
    {
        teamManager.OnTravel();
    }
    
    public void DisplayCasesEvent()
    {
        // Ajouter un script Case et lui faire récupérer le Member member si il y en a un
        foreach(MapCase mapCase in mapCases)
        {
            if(mapCase.memberOccupied)
            {
                mapCase.eventName.text = mapCase.member.name;
            }

            if(mapCase.eventOccupied)
            {
                // mapCase.eventName.text = mapCase.mapEvent.title;
            }
        }
    }
}
