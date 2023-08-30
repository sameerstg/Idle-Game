using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class State
{
    internal Npc npc;
    public State(Npc npc)
    {
        this.npc = npc;
    }
    public virtual void Enter(){
    
        
        //Debug.Log("Entering " + stateName);
    }


    public virtual void Exit(){
        //Debug.Log("Exiting " + stateName);

    }

}

