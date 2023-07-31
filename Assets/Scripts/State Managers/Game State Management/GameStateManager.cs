using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public DateTime startTime;
    public GameState currentGameState;
    public string currentTimeFrame;
    public List<GameState> gameStates;
    public OnStateComplete onStateCompleteCallback;
    private void Awake()
    {
        startTime = DateTime.Now;
        InitializeStates();


    }
    private void Start()
    {
        
    }
    public void SwichState()
    {
        if (currentGameState != null)
        {
        currentGameState.Exit();

        }
        if (gameStates.IndexOf(currentGameState)+1 <gameStates.Count)
        {
            currentGameState = gameStates[gameStates.IndexOf(currentGameState) + 1];
        }
        else
        {

            currentGameState = gameStates[0];
        }
        currentGameState.Enter();
    }

    private void Update()
    {
        currentTimeFrame = (DateTime.Now.Subtract(startTime).ToString());

        currentGameState.Update();
    }
    public void InitializeStates()
    {
        gameStates = new();
        onStateCompleteCallback = SwichState;
        gameStates.Add(new SleepingGameState(10f, onStateCompleteCallback));
        gameStates.Add(new BathingGameState(10f, onStateCompleteCallback));
        gameStates.Add(new EatingGameState(10f, onStateCompleteCallback));
        gameStates.Add(new WorkingGameState(10f, onStateCompleteCallback));
        SwichState();
        
    }
}
public delegate void OnStateComplete();

