using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public List<MapCase> mapCases = new List<MapCase>();
    public GameObject mapCasePrefab;
    public GameObject mapButton;
    public GameObject mapUI;
    public bool mapUIActive = false;
    public MapCase mapCase;
    public MapCase playerCase;
    public TeamManager teamManager;
    public Base baseCase;
    public DisplayJournal displayJournal;
    public GameObject caseParent;
    
    public Text travelText;
    public int caseYCount = 8;
    public int caseXCount = 8;
    public int onTravel;
    public bool mapDisplayed = false;
    
    public void Start()
    {
        // Case add to a list 
        foreach(Transform child in caseParent.transform)
        {
            MapCase mapCase = child.GetComponent<MapCase>();
            mapCases.Add(mapCase);
            
        }

        displayJournal = FindObjectOfType<DisplayJournal>();
        mapCase.Bypass();

        // DefinePlayerCase();
    }

    public void RandomizeCasesEvent()
    {
        // A REVOIR TOUTE CETTE FONCTION
        // On va plutôt définir une mapCase pour chaque mapEvent, c'est plus logique
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

    public void DefinePlayerCase()
    {
        int randPC = Random.Range(0, mapCases.Count);
        playerCase = mapCases[randPC];
        playerCase.isFree = false;
        playerCase.eventName.text += "Base";
    }

    public void OnMapClick()
    {
        mapDisplayed = !mapDisplayed;
        mapUI.SetActive(mapDisplayed);
        if(playerCase == null)
        {
            DefinePlayerCase();
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
            OnClickTravel();
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
        displayJournal.travelButtons.SetActive(false);
    }
    public void OnCancelTravel()
    {
        displayJournal.travelButtons.SetActive(false);
    }
    public void OnClickTravel()
    {
        displayJournal.travelButtons.SetActive(true);
        // teamManager.OnTravel();
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
    
    public void PopulateMap()
    {
        int mapCaseXIndex = 0;
        int mapCaseYIndex = 0;

        for(int i = 0 ; i < caseYCount; i++)
        {
            GameObject goY;

            goY = Instantiate(mapCasePrefab, (caseParent.transform)) as GameObject;
            goY.transform.SetParent(caseParent.transform);
            MapCase yCase = goY.GetComponent<MapCase>();

            mapCaseYIndex++;
            yCase.YCoordinate = mapCaseYIndex;

            mapCases.Add(yCase);
            

            for(int y = 1; y > caseXCount; y++)
            {
                GameObject goX;

                goX = Instantiate(mapCasePrefab, (caseParent.transform)) as GameObject;
                goX.transform.SetParent(caseParent.transform);
                MapCase xCase = goX.GetComponent<MapCase>();

                mapCaseXIndex++;
                xCase.XCoordinate = mapCaseXIndex;

                mapCases.Add(xCase);
                
            }
        }
    }
}
