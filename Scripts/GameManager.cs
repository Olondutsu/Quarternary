using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public DisplayJournal displayJournal;

    public void Start()
    {
        displayJournal = FindObjectOfType<DisplayJournal>();
    }


    public void GameOver()
    {
        Animation anim = displayJournal.transitionPanel.GetComponent<Animation>();
        displayJournal.transitionPanel.SetActive(true);
        displayJournal.transiDayText.text = "GAME OVER";
        anim.Play("GameOver");
        Debug.Log("Game OVer !!");
    }
}