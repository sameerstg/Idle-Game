using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public DateTime startTime;
 
    private void Awake()
    {
        startTime = DateTime.Now;
    }
  
}
public delegate void OnStateComplete();

