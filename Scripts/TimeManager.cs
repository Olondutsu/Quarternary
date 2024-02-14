using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int currentDay = 0;

   public void NextDay()
   {
    currentDay += 1;
   }
}