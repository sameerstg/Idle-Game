
using UnityEngine;

[System.Serializable]
public class GameState:IState
{

    public GameStateName gameStateName;

    public GameState(GameStateName gameStateName,float startTime)
    {
        this.gameStateName = gameStateName;
    }

    public virtual void Enter()
    {
    }
    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
    }
}
[System.Serializable]
public class PrisonersEntryState : GameState
{
    public float endTime = 60*2;
    
    public PrisonersEntryState(GameStateName gameStateName, float startTime) : base(gameStateName, startTime )
    {
        
    }

    public override void Update()
    {
        base.Update();


    }

}
