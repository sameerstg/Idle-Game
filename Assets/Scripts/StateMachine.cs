[System.Serializable]
public class Statemachine
{
    public State currentState;
    public Npc npc;
    public Statemachine(Npc npc)
    {
        this.npc = npc;
    }
    public void SwitchState(State state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
