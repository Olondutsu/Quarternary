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

    // THIS PART HAVE TO BEEN WORKED AGAIN,
    

    //On Randomize la map,
    public void RandomizeCasesEvent()
    {
        // A REVOIR TOUTE CETTE FONCTION
        foreach(MapCase mapCase in mapCases)
        {
            foreach(MapEvent mapEvent in mapCase.mapEvents)
            {
                if(mapCase.isFree)
                {
                    mapCase.isFree = false;
                    int randomIndex = Random.Range(0, 10);

                    // A remplacer la liste de mapCase par une liste sur MapManager pour seulement mettre 1 event sur une case
                    MapEvent currentMapEvent = mapCase.mapEvents[randomIndex];

                    mapCase.thisCaseEvent = currentMapEvent;
                }
            }
        }
    }

    public void CalculateMapDistance(MapCase mapCase)
    {
        // foreach(MapCase mapCase in mapCases)
        // {
        if(mapCase.isClicked)
        {
            int xDistance = Mathf.Abs(playerCase.XCoordinate - mapCase.XCoordinate);
            int yDistance = Mathf.Abs(playerCase.YCoordinate - mapCase.YCoordinate);
            mapCase.travelTime = xDistance + yDistance;
            onTravel = mapCase.travelTime/4 ;    
            DisplayTravel(onTravel);
            // onTravel = onTravel;
        }
        // }
    }
    
    // Display le TravelTime,
    public void DisplayTravel(int travelTime)
    {
        travelText.text = travelTime + "DAYS";
    }

    public void OnConfirmTravel()
    {
        teamManager.OnTravel();
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
    
    public void DisplayMap()
    {
        // for(int i = 0 ; maxCasesCount; i++)
        // {

        // }
        // donner un Prefab avec une map en 32x32 (peut-être) puis ajouter un foreach MapCase, 
        // faire les verifications de là pour les afficher sur les bonnes cases, 
        // faire en sorte que ce soit cliquable et que ça affiche les trucs audessus etc;
    }
}
