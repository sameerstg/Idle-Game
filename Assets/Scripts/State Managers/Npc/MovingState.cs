using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{
    PlaceName placeToGo;
    public MovingState(Npc npc,PlaceName placeToGo) : base(npc)
    {
        this.placeToGo = placeToGo;
    }

    public override void Enter()
    {
        base.Enter();
        npc.Move(placeToGo);
    }

    

    
}
