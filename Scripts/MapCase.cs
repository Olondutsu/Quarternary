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

    public int XCoordinate;
    public int YCoordinate;
    public int travelTime;

    public bool isBaseFrom;
    public bool isFree;
    public bool memberOccupied;
    public bool eventOccupied;

    public bool isClicked;
    
    void Start()
    {

    }
    
    void OnCaseClick()
    {
        mapManager.CalculateMapDistance(this);

        if(this.isBaseFrom)
        {
            teamManager.OnLeaderClick();
        }
        
        else
        {
            displayJournal.travelButtons.SetActive(true);
        }
    }

}

