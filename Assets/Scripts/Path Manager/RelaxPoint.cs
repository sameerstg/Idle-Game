using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxPoint : MonoBehaviour
{
    public Npc equipedNpc;
    public float time;
    public virtual void DoWork(Npc npc)
    {
        equipedNpc = npc;
        //npc.statemachine.SwitchState(new WaitState(npc));

    }
    public IEnumerator Wait()
    {

        yield return new WaitForSeconds(time);
        //equipedNpc.statemachine.SwitchState(new IdleState(equipedNpc));
    } 
    
}
public enum RelaxPointType
{
    relax,work
}
