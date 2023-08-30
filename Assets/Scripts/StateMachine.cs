

using UnityEngine;

[System.Serializable]
public class Statemachine
{
    internal State currentState;
    public string currentStateName,prevStateName;
    internal Npc npc;
    public Statemachine(Npc npc)
    {
        this.npc = npc;
        SwitchState(new IdleState(npc));
    }
    public void SwitchState(State state )
    {
        
        Debug.Log(state);
        
        if (currentState!=null)
        {
            prevStateName = currentState.GetType().Name;
            currentState.Exit();
            currentState = null;
        }
        currentState = state;
        Debug.Log(currentState);

        currentStateName = state.GetType().Name;
        Debug.Log(currentStateName);

        currentState.Enter();
    }
    
}
