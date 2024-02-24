using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/Event")]
public class Event : ScriptableObject
{
    public string title;
    [TextArea(3, 10)]
    public string description;
    public bool yesChoice;
    public bool noChoice;
    public bool boolChoice;
    public bool isMainEvent;
    public bool conditionsMet;
    public bool completed;
    public bool isEnd;
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