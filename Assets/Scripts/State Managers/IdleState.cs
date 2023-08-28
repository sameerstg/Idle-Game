using UnityEngine;

public class IdleState : State
{
    public IdleState(Npc npc) : base(npc)
    {
    }

    public override  void Enter()
    {
        Debug.Log("Idle state");
       
    }

    public override void Exit()
    {
       
    }

  
}
