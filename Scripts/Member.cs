using UnityEngine;

[CreateAssetMenu(fileName = "New Member", menuName = "Members/Member")]

public class Member : ScriptableObject
{
    public string fullname;
    public int physicalHealth;
    public int mentalHealth;
    public int hunger;
    public int thirst;
    public bool isInTeam;

    // public Member(string name, int physicalHealth, int mentalHealth, int hunger, int thirst)
    // {
    //     Name = name;
    //     PhysicalHealth = physicalHealth;
    //     MentalHealth = mentalHealth;
    //     Hunger = hunger;
    //     Thirst = thirst;
    // }
}