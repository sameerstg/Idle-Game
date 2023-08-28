using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    internal Npc npc;

    public State(Npc npc)
    {
        this.npc = npc;
    }
    public virtual void Enter(){}


    public virtual void Exit(){}

}

