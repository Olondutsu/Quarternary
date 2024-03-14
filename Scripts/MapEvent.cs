using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapEvent", menuName = "Events/MapEvent")]
public class MapEvent : ScriptableObject
{
    public string title;
    [TextArea(3, 10)]
    public string description;
    public bool yesChoice;
    public bool noChoice;
    public bool boolChoice;
    public bool conditionsMet;
    public bool completed;
    public bool isFree;
    public bool isEnnemyBase;
    public bool isEnnemyPatrol;
    public bool isOnMap;
    public bool isRandom;
    public int delayDays;
    


    public MapEvent[] outcomeEvents;
    public MapEvent[] yesOutcomeEvents;
    public MapEvent[] noOutcomeEvents;
    public ItemData[] reward;
    public ItemData[] loss;
    public ItemData[] neededItems;
    public Member[] addedMember;
    public Member[] removedMember;
}