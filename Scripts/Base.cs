using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    MapCase baseCase;
    public List<Member> membersInBase = new List<Member>(); 
    public List<Member> membersInTravel = new List<Member>();
    public List<Member> selectedMembers = new List<Member>();
    
    public List<ItemData> itemsInBase = new List<ItemData>();
    public List<Page> pagesForBase = new List<Page>();
    public List<GameObject> pagePrefab = new List<GameObject>();

    public Member squadLeader;
    public bool journalLoaded;
    public bool isSelected;
    public bool available;
    public bool displayed = false;
    public bool isFull = false;
}