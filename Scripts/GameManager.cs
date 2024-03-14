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
        displayJournal.transitionPanel.SetActive(false);
        Animation animGO = displayJournal.transitionPanel.GetComponent<Animation>();
        if(animGO != null)
        {
            displayJournal.transitionPanel.SetActive(true);
            displayJournal.transiDayText.text = "GAME OVER";
            animGO.Play("GameOver");
            Debug.Log("Game OVer !!");
        }
        else
        {
            Debug.Log("Else GameOver donc animGO = null :'()");
        }

    }
}