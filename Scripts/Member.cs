using UnityEngine;

[CreateAssetMenu(fileName = "New Member", menuName = "Members/Member")]

public class Member : ScriptableObject
{
    public Sprite journalVisual;
    public Sprite journalVisualHighlight;
    public Sprite gameVisual;
    public string fullname;
    public int physicalHealth;
    public int mentalHealth;
    public int hunger;
    public int thirst;
    public bool isInTeam;
    public bool selected;
    // public Member(string name, int physicalHealth, int mentalHealth, int hunger, int thirst)
    // {
    //     Name = name;
    //     PhysicalHealth = physicalHealth;
    //     MentalHealth = mentalHealth;
    //     Hunger = hunger;
    //     Thirst = thirst;
    // }
}