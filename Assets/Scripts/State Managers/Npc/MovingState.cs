using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{
    Place placeToGo;
    public MovingState(Npc npc,Place placeToGo) : base(npc)
    {
        this.placeToGo = placeToGo;
    }

    public override void Enter()
    {
        base.Enter();
        npc.Move(placeToGo);
    }

    

    
}
