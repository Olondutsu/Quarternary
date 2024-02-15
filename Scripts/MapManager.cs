using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    List<MapCase> mapCases = new List<MapCase>();

    public MapCase mapCase;
    public MapCase playerCase;

    public void RandomizeCasesEvent()
    {
        foreach(MapCase mapCase in mapCases)
        {
        int xDistance = Mathf.Abs(playerCase.XCoordinate - mapCase.XCoordinate);
        int yDistance = Mathf.Abs(playerCase.YCoordinate - mapCase.YCoordinate);

        mapCase.travelTime = xDistance + yDistance;
        }
    }

    public void CalculateMapDistance()
    {
        foreach(MapCase mapCase in mapCases)
        {
        int xDistance = Mathf.Abs(playerCase.XCoordinate - mapCase.XCoordinate);
        int yDistance = Mathf.Abs(playerCase.YCoordinate - mapCase.YCoordinate);
        mapCase.travelTime = xDistance + yDistance;            
        }
    }
    
    public void DisplayCasesEvent()
    {
        // Ajouter un script Case et lui faire récupérer le Member member si il y en a un
        foreach(MapCase mapCase in mapCases)
        {
            if(mapCase.memberOccupied)
            {
                // Display le nom du joueur sur cette même case
            }

            if(mapCase.eventOccupied)
            {

            }
        }
    }
}
