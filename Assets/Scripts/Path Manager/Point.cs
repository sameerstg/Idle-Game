using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Point : MonoBehaviour
{

    public PointConnection pointConnection;
    
    public PointType positionType;
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
//[CustomEditor(typeof(Point))]
//public class PointEditor :Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        //DrawDefaultInspector();

//        //if (Application.isPlaying)
//        //{
            
//        //}
//    }
//}
public enum RelaxPointType
{
    none,relax, work
}
[System.Serializable]
public class PointConnection
{
    public List<Point> allPoints = new();
    [Header("Dont assign, Auto assign")]
    public List<Point> connectedPoints = new(), relaxPoints = new();
}
