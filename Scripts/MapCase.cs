using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class MapCase: MonoBehaviour
{
    
    public Member member;
    public ItemsManager itemManager;
    public MapManager mapManager;
    public TeamManager teamManager;
    public DisplayJournal displayJournal;
    public Text eventName;
    public MapEvent[] mapEvent;
    public MapEvent thisCaseEvent;
    public List<MapEvent> mapEvents = new List<MapEvent>();
    public MapCase instance;

    public int XCoordinate;
    public int YCoordinate;
    public int travelTime;

    public bool isBaseFrom;
    public bool isFree;
    public bool memberOccupied;
    public bool eventOccupied;

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
        eventName = GetComponentInChildren<Text>();
    }
    
    public void OnCaseClick()
    {
        mapManager.CalculateMapDistance(instance);

        if(instance.isBaseFrom)
        {
            teamManager.OnLeaderClick();
        }
        
        else
        {
            displayJournal.travelButtons.SetActive(true);
        }
    }

    public void Bypass()
    {
        Start();
    }
}

