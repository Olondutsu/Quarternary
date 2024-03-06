using UnityEngine;

[CreateAssetMenu(fileName = "New Member", menuName = "Members/Member")]

public class Member : ScriptableObject
{
    public Sprite journalVisual;
    public Sprite journalVisualHighlight;
    public Sprite gameVisual;
    public Base baseComingFrom;
    public Base baseLivingIn;
    public string fullname;
    public int physicalHealth;
    public int mentalHealth;
    public int hunger;
    public int thirst;
    public int firePower;
    public bool isInTeam;
    public bool isPickUp;
    public bool selected;
    public bool arrived;
    public bool returned;
    
    // public Member(string name, int physicalHealth, int mentalHealth, int hunger, int thirst)
    // {
    //     Name = name;
    //     PhysicalHealth = physicalHealth;
    //     MentalHealth = mentalHealth;
    //     Hunger = hunger;
    //     Thirst = thirst;
    // }
}