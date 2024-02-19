using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]

public class ItemData : ScriptableObject
{
    [SerializeField]
    public Sprite visual;
     [SerializeField]
    public Sprite journalVisualAvailable;
     [SerializeField]
    public Sprite journalVisualUnAvailable;
    public bool isSupplie;
    public bool isFood;
    public bool isDrink;
    public bool isHealKit;
}