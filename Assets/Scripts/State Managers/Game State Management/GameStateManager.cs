using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager _instance;
    public float time;
    public GameState currentGameState;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        currentGameState = null;
        SwitchState();
    }
    private void Update()
    {
        currentGameState?.Update();
    }
  

    internal void SwitchState()
    {
        currentGameState?.Exit();
        if (currentGameState!=null)
        {
            switch (currentGameState.gameStateName)
            {
                case GameStateName.PrisonersEntry:
                    currentGameState = new CellGameState();
                    break;
                case GameStateName.Cell:
                    currentGameState = new SleepingGameState();

                    break;
                case GameStateName.Sleeping:
                    currentGameState = new BathingGameState();

                    break;
                
                case GameStateName.Bathing:
                    currentGameState = new EatingGameState();

                    break;
                case GameStateName.Eating:
                    currentGameState = new GymingGameState();

                    break;
                case GameStateName.Gym:
                    currentGameState = new PrisonersEntryState();

                    break;
                default:
                    break;
            }
        }
        else
        {
            currentGameState = new PrisonersEntryState();
        }
        
        Debug.Log(currentGameState.gameStateName); 
        
        currentGameState.Enter();
    }

   

}
public enum GameStateName
{
    PrisonersEntry,Sleeping,Cell,Bathing,Eating,Gym
}
[CustomEditor(typeof(GameStateManager))]
public class GameStateManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawDefaultInspector();
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Change State"))
            {
                GameStateManager gm = (GameStateManager)target;
                gm.SwitchState();
            }
        }
    }
}
