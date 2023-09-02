
[System.Serializable]
public class State:IState
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
public interface IState
{
    public void Enter();
    
    public void Exit();
}
