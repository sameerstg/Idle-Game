
using UnityEngine;

[System.Serializable]
public class GameState : State
{
    public string name;
    public float timeFromStart;
    public GameState(float periodOfState, OnStateComplete onStateCompleteCallback)
    {
        name = GetType().Name;
        this.periodOfState = periodOfState;
        this.onStateComplete = onStateCompleteCallback;
    }

  

    public float periodOfState;
    private OnStateComplete onStateComplete;

    public override void Enter()
    {
        timeFromStart =0;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        timeFromStart += Time.deltaTime;
        if (timeFromStart>periodOfState)
        {
            onStateComplete?.Invoke();
        }
    }
}
