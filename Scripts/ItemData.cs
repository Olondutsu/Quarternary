using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item")]

public class ItemData : ScriptableObject
{
    [SerializeField]
    public GameObject gameVisual;

    [SerializeField]
    public Sprite journalVisualAvailable;
    [SerializeField]
    public Sprite journalVisualUnAvailable;
    public int quantity;
    public int maxStack;
    public bool stackable;
    public bool isSupplie;
    public bool isFood;
    public bool isDrink;
    public bool isHealKit;
    public bool selected;
    public bool Displayed;
    public bool isRare;
    public bool isCommon;
}