[System.Serializable]
public class Statemachine
{
    public State currentState;
    public Npc npc;
    public Statemachine(Npc npc)
    {
        this.npc = npc;
        SwitchState(new IdleState(npc));
    }
    public void SwitchState(State state)
    {
        if (currentState!=null)
        {
            currentState.Exit();
        }
        currentState = state;
        currentState.Enter();
    }
}
