using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : State
{
    public WaitState(Npc npc) : base(npc)
    {
        
    }
    public override void Enter()
    {
        base.Enter();

        Debug.Log("waiting state");
    }
}
