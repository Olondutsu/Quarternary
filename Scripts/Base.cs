using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    MapCase baseCase;
    public List<Member> membersInBase = new List<Member>(); 
    public List<ItemData> itemsInBase = new List<ItemData>();

    public Member squadLeader;
    public bool journalLoaded;
    public bool isSelected;
    public bool available;
    public bool displayed = false;
    public bool isFull = false;
}