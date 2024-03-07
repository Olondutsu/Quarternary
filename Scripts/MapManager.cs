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
    public GameObject memberParent;
    public GameObject memberPrefab;
    
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
    
    public void Start()
    {
        displayJournal = FindObjectOfType<DisplayJournal>();
        selectedBase = FindObjectOfType<Base>();
        PopulateMap();
        mapUI.SetActive(false);
        
    }

    public void PopulateMemberSelection()
    {
        foreach(Member member in selectedBase.membersInBase)
        {

            if(member != null)
            {
                Debug.Log("Member n'est pas nulll Populate Member selection");
                GameObject go;

                go = Instantiate(memberPrefab, memberParent.transform);
                go.transform.SetParent(memberParent.transform);
                
                Slot slot = go.GetComponent<Slot>();
                slot.slotMember = member;
                slot.isCharacterSlot = true;
                slot.image.sprite = member.journalVisual;
                selectionSlots.Add(slot);
            }
            else
            {
                Debug.Log("On détruit les child popualte member selection");
                foreach(Transform child in memberParent.transform)
                {
                    Destroy(child.gameObject);
                }
            }

        }
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
        PopulateMemberSelection();

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
        PopulateCasesMember();
    }
}
