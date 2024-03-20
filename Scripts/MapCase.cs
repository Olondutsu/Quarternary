using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class MapCase: MonoBehaviour
{
    public Image image;
    public Member member;
    public ItemsManager itemManager;
    public MapManager mapManager;
    public TeamManager teamManager;
    public DisplayJournal displayJournal;
    public Text eventName;
    public MapEvent[] mapEvent;
    public MapEvent thisCaseEvent;
    public Base selectedBase;
    public Base hostedBase;
    public List<MapEvent> mapEvents = new List<MapEvent>();

    public List<MapCase> neighbors = new List<MapCase>();
    
    public GameObject notification;
    
    public MapCase instance;
    public MapCase forwardCase;
    public MapCase rightCase;
    public MapCase leftCase;
    public MapCase backwardCase;

    public int XCoordinate;
    public int YCoordinate;
    public int travelTime;

    public bool isBorder;
    public bool isBaseFrom;
    public bool isFree;
    public bool memberOccupied;
    public bool eventOccupied;

    public bool isTree;
    public bool isMountain;
    public bool isGrass;

    public bool isClicked;
    
    void Awake()
    {


    }
    void Start()
    {
        instance = this;
        mapManager = FindObjectOfType<MapManager>();
        teamManager = FindObjectOfType<TeamManager>();
        itemManager = FindObjectOfType<ItemsManager>();
        displayJournal = FindObjectOfType<DisplayJournal>();
        image = transform.GetComponent<Image>();
        selectedBase = teamManager.selectedBase;

    }

    public void DefineCasesAround()
    {
        mapManager = FindObjectOfType<MapManager>();
        image = transform.GetComponent<Image>();

        instance = this;
        backwardCase = null;
        forwardCase = null;
        leftCase = null;
        rightCase = null;

        if(mapManager != null)
        {
            foreach (MapCase aCase in mapManager.mapCases)
            {
                if (aCase != null && aCase != this) // Assurez-vous que la case n'est pas celle actuelle
                {
                    if (aCase.XCoordinate == XCoordinate - 1 && aCase.YCoordinate == YCoordinate)
                    {
                        leftCase = aCase;
                        neighbors.Add(aCase);
                    }
                    else if (aCase.XCoordinate == XCoordinate + 1 && aCase.YCoordinate == YCoordinate)
                    {
                        rightCase = aCase;
                        neighbors.Add(aCase);
                    }
                    else if (aCase.YCoordinate == YCoordinate - 1 && aCase.XCoordinate == XCoordinate)
                    {
                        backwardCase = aCase;
                        neighbors.Add(aCase);
                    }
                    else if (aCase.YCoordinate == YCoordinate + 1 && aCase.XCoordinate == XCoordinate)
                    {
                        forwardCase = aCase;
                        neighbors.Add(aCase);
                    }
                }
            }
        }
    }

    public void OnCaseClick()
    {

        
        mapManager.GetMapCase(XCoordinate, YCoordinate);
        selectedBase.clickedCase = instance;

        mapManager.CalculateMapDistance(instance);
        isClicked = true;


        if(instance.isBaseFrom)
        {
            teamManager.OnLeaderClick();
        }
        
        else
        {
            // displayJournal.travelButtons.SetActive(true);
            mapManager.selectionPage.SetActive(true);
        }
    }

}

