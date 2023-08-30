using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxPoint : MonoBehaviour
{
    public Npc equipedNpc;
    public float waitTime;
    public virtual void DoWork(Npc npc)
    {
        equipedNpc = npc;
        npc.statemachine.SwitchState(new WaitState(npc));
        StartCoroutine(Wait());

    }
    public IEnumerator Wait()
    {

        Debug.Log($"waiting time = {waitTime}");
        yield return new WaitForSeconds(waitTime);
        equipedNpc.statemachine.SwitchState(new IdleState(equipedNpc));
        equipedNpc = null; 

    } 
    
}
public enum RelaxPointType
{
    relax,work
}
