using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]

public class ItemData : ScriptableObject
{
    public GameObject visual;
    public GameObject journalVisualAvailable;
    public GameObject journalVisualUnAvailable;
    public bool isSupplie;
    public bool isFood;
    public bool isDrink;
    public bool isHealKit;
}