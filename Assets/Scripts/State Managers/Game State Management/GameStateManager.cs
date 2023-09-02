using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public float time;
    public float totalCourseTime = 10;
    public GameState currentGameState;
    private void Update()
    {
        time += Time.deltaTime;
    }
    public void CheckTime()
    {
        float currentCourse= time % totalCourseTime;

    }
    public void ChangeState()
    {

    }

}
public enum GameStateName
{
    PrisonersEntry,Cell,Bathroom,Food,Gym
}
