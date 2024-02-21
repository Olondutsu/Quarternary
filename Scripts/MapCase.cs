using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
public class MapCase: MonoBehaviour
{
    
    public Member member;
    public ItemsManager itemManager;
    public MapManager mapManager;
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
            Debug.Log("You can't travel to your base");
        }
        else
        {
            // Afficher les boutons de confirmation ou non du voyage avec peut-être le display du jour.
            // confirmClick où on appelle OnConfirmTravel(),
            // cancel Clic où on supprime simplement l'affichage des boutons don le reverse d'ici;
        }
    }

}

