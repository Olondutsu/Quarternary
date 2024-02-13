using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]

public class ItemData : ScriptableObject
{
    public GameObject visual;
    public GameObject journalVisualAvailable;
    public GameObject journalVisualUnAvailable;

}