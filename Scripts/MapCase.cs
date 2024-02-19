using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class MapCase: MonoBehaviour
{
    
    public Member member;
    public ItemsManager itemManager;
    public Text eventName;
    public MapEvent[] mapEvent;
    List<MapEvent> mapEvents = new List<MapEvent>();

    public int XCoordinate;
    public int YCoordinate;
    public int travelTime;

    public bool memberOccupied;
    public bool eventOccupied;

    public bool isClicked;
    
    void Start()
    {

    }
    void OnCaseClick()
    {

    }

}

