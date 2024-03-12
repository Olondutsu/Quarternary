using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public List<MapCase> mapCases = new List<MapCase>();
    public GameObject mapCasePrefab;
    public GameObject mapButton;
    public GameObject mapUI;
    public GameObject memberParent;
    public GameObject memberPrefab;
    public Page mapEventPage;
    public MapCase mapCase;
    public MapCase playerCase;
    public TeamManager teamManager;
    public Base baseCase;
    public Base selectedBase;
    public DisplayJournal displayJournal;

    public GameObject caseParent;
    public GameObject selectionPage;

    public List<Slot> selectionSlots = new List<Slot>();

    public Image image;

    public Sprite treeCaseSprite;
    public Sprite grassCaseSprite;
    public Sprite mountainCaseSprite;
    public Sprite baseCaseSprite;
    
    public Text travelText;
    public int caseYCount = 8;
    public int caseXCount = 8;
    public int onTravel;
    public bool mapDisplayed = false;
    public bool mapUIActive = false;
    public bool casesMemberGenerated = false;
    public void Start()
    {
        displayJournal = FindObjectOfType<DisplayJournal>();
        selectedBase = FindObjectOfType<Base>();
        PopulateMap();
        mapUI.SetActive(false);
        
    }

    public MapCase GetMapCase(int x, int y)
    {
        foreach (MapCase mapCase in mapCases)
        {
            if (mapCase.XCoordinate == x && mapCase.YCoordinate == y)
            {
                return mapCase;
            }
            else
            {Debug.Log("Pas de MapCase");}
        }
        return null; // Si aucune case correspondante n'est trouvée
    }

    public void PopulateMemberSelection(Member addMember)
    {
        GameObject go;

        go = Instantiate(memberPrefab, memberParent.transform);
        go.transform.SetParent(memberParent.transform);
        
        Slot slot = go.GetComponent<Slot>();

        slot.slotMember = addMember;
        slot.isCharacterSlot = true;
        slot.image.sprite = addMember.journalVisual;
        selectionSlots.Add(slot);
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

    public void PopulateCasesMember()
    {
        foreach(Member aMember in teamManager.everyMembers)
        {
            // grv a chier cette façon de faire javais juste la flemme et jveux du resultat vif sinon il faut évidmment créer une autre liste qu'EveryMembers
            if(!aMember.isInTeam)
            {
                int randomIndex = Random.Range(0, mapCases.Count);
                MapCase randomCase = mapCases[randomIndex];

                if(randomCase.isFree)
                {
                    randomCase.member = aMember;
                    randomCase.isFree = false;
                    randomCase.memberOccupied = true;
                    randomCase.notification.SetActive(true);
                }
                else
                {
                }
            }
        }
        casesMemberGenerated = true;
    }

    public void DefinePlayerCase()
    {
        int randPC = Random.Range(0, mapCases.Count);
        playerCase = mapCases[randPC];
        playerCase.isFree = false;
        playerCase.image.sprite = baseCaseSprite;
    }

    public void OnMapClick()
    {
        mapDisplayed = !mapDisplayed;
        mapUI.SetActive(mapDisplayed);
        if(playerCase == null)
        {
            DefinePlayerCase();
        }
        if(!casesMemberGenerated)
        {
            PopulateCasesMember();
        }
    }

    public void DefineTravelCase()
    {
    }

    public void MapEventArrivalTrigger()
    {
        MapCase clickedCase = selectedBase.clickedCase;
        GameObject mapEventPageGO = mapEventPage.gameObject;


        if (clickedCase.memberOccupied)
        {
            Debug.Log("La Case est occupé par un membre");
            
            mapEventPage.pageBody.text = clickedCase.member.name + "is here and joined your team";
            selectedBase.membersInTravel.Add(clickedCase.member);
            mapEventPageGO.SetActive(true);
        }
        else
        {
            Debug.Log("La Case est pas occupé par un membre");
        }
        if (clickedCase.eventOccupied)
        {
            Debug.Log("La Case est occupé par un event");
            mapEventPage.pageBody.text = clickedCase.thisCaseEvent.description;
            mapEventPageGO.SetActive(true);
        }
        else
        {
            Debug.Log("La Case est pas occupé par un membre");
        }
    }

    public void PopulateMapEventPage()
    {

    }

    public void MapEventReturnTrigger()
    {
        foreach(Member member in selectedBase.membersInTravel)
        {
            selectedBase.membersInBase.Add(member);
        }
    }

    public void CalculateMapDistance(MapCase mapCase)
    {
        int xDistance = Mathf.Abs(playerCase.XCoordinate - mapCase.XCoordinate);
        int yDistance = Mathf.Abs(playerCase.YCoordinate - mapCase.YCoordinate);
        mapCase.travelTime = xDistance + yDistance;
        onTravel = mapCase.travelTime/2;
        onTravel++;

        DisplayTravel(onTravel);
        OnClickTravel();
    }
    
    public void DisplayTravel(int travelTime)
    {
        travelText.text = travelTime + "DAYS";
    }

    public void OnConfirmTravel()
    {
        teamManager.OnTravel();
        displayJournal.travelButtons.SetActive(false);
        selectionPage.SetActive(false);
    }
    public void OnCancelTravel()
    {
        displayJournal.travelButtons.SetActive(false);
        selectionPage.SetActive(false);
        
    }
    public void OnClickTravel()
    {
        displayJournal.travelButtons.SetActive(true);
        selectionPage.SetActive(true);
    }
    
    public void DisplayCasesEvent()
    {
        foreach(MapCase mapCase in mapCases)
        {
            if(mapCase.memberOccupied)
            {
                mapCase.eventName.text = mapCase.member.name;
            }

            if(mapCase.eventOccupied)
            {
                
            }
        }
    }
    
    public void PopulateMap()
    {
        float yOffset = 50f;
        float xOffset = 50f;

        for (int i = 0; i < caseYCount; i++)
        {
            for (int j = 0; j < caseXCount; j++)
            {
                GameObject go;

                go = Instantiate(mapCasePrefab, caseParent.transform);
                go.transform.SetParent(caseParent.transform);

                MapCase mapCase = go.GetComponent<MapCase>();
                mapCase.XCoordinate = j;
                mapCase.YCoordinate = i;

                go.transform.position = new Vector3(j * xOffset, i * yOffset);
                
                mapCases.Add(mapCase);
            }
        }

        foreach(MapCase mapCase in mapCases)
        {
            mapCase.DefineCasesAround();
        }

       foreach(MapCase mapCase in mapCases)
        {
            float randomValue = Random.value;
            mapCase.isFree = true;
            float treeWeight = 0.1f;
            float mountainWeight = 0.05f;
            float grassWeight = 1f - treeWeight - mountainWeight;

            foreach(MapCase neighbor in mapCase.neighbors)
            {
                if (neighbor.isTree)
                    treeWeight += 0.1f; // Ajouter 2% de chance d'avoir un arbre si le voisin est un arbre
                else if (neighbor.isMountain)
                    mountainWeight += 0.05f; // Ajouter 1% de chance d'avoir une montagne si le voisin est une montagne
            }

            if (randomValue < treeWeight)
            {
                mapCase.isTree = true;
                mapCase.image.sprite = treeCaseSprite;
            }
            else if (randomValue < treeWeight + mountainWeight)
            {
                mapCase.isMountain = true;
                mapCase.image.sprite = mountainCaseSprite;
            }
            else
            {
                mapCase.isGrass = true;
                mapCase.image.sprite = grassCaseSprite;
            }
        }
    }
}
