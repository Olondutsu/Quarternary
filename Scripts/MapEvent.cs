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
    public int delayDays;
    


    public Event[] outcomeEvents;
    public Event[] yesOutcomeEvents;
    public Event[] noOutcomeEvents;
    public ItemData[] reward;
    public ItemData[] loss;
    public ItemData[] neededItems;
    public Member[] addedMember;
    public Member[] removedMember;
}