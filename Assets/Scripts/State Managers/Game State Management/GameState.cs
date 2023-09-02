
using UnityEngine;

[System.Serializable]
public class GameState:IState
{
    public float time,endTime;

    public GameStateName gameStateName;
    public PlaceName placeName;

    public GameState()
    {
        time = 0;
        endTime = 120;
    }

    public virtual void Enter()
    {
        NpcManager._instance.SendNpc(placeName);
    }
    public virtual void Update()
    {
        time += Time.deltaTime;
        if (time >= endTime)
        {
            GameStateManager._instance.SwitchState();
        }
    }

    public virtual void Exit()
    {
    }
}

