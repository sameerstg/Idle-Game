using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public float walkSpeed,runSpeed;
    public Waypoint currentWaypoint;
    public Place togoPlace;
    public RelaxWaypoint relaxWaypoint;
    public Transform currentTransform;
    public List<Waypoint> togoWaypoints = new();

    public List<Transform> transformsTogo =new();

    public Statemachine statemachine;
    private void Awake()
    {
        statemachine = new(this);
    }
    public void Move(Place placeToGo)
    {
        togoPlace = placeToGo;
        togoWaypoints = PathManager._instance.GetPath(currentWaypoint, placeToGo);
        StartCoroutine(MoveByWaypoints());
    }

    public void MoveToRelaxPoint()
    {
        if (togoPlace != null && togoPlace.HaveEmptyRelaxPoint())
        {
            if (togoPlace.RelaxPointType == RelaxPointType.relax)
            {
                var pathWithRelaxWaypoint = togoPlace.GetPathToRelaxPoint();
                if (pathWithRelaxWaypoint.Item2 != null)
                {
                    transformsTogo = pathWithRelaxWaypoint.Item1;
                    relaxWaypoint = pathWithRelaxWaypoint.Item2;
                    StartCoroutine(MoveByTransforms());
                }
            }
            else if (togoPlace.RelaxPointType == RelaxPointType.work)
            {
                var pathWithRelaxWaypoint = togoPlace.GetPathToWorkRelaxPoint();
            }
            
        }
        statemachine.SwitchState(new IdleState(this));
    }
    public IEnumerator MoveByWaypoints()
    {
        if (togoWaypoints != null)
        {
            while (togoWaypoints.Count > 0)
            {

                while (Vector3.Distance(transform.position, togoWaypoints[0].transform.position) > 0.1f)
                {

                    transform.position = Vector3.MoveTowards(transform.position, togoWaypoints[0].transform.position, walkSpeed * Time.deltaTime);
                    yield return null;
                }
                currentWaypoint = togoWaypoints[0];
                togoWaypoints.RemoveAt(0);

            }
            togoWaypoints.Clear();
        }
        MoveToRelaxPoint();


    }

    public IEnumerator MoveByTransforms()
    {
        if (transformsTogo != null)
        {
            while (transformsTogo.Count > 0)
            {

                while (Vector3.Distance(transform.position, transformsTogo[0].position) > 0.1f)
                {

                    transform.position = Vector3.MoveTowards(transform.position, transformsTogo[0].transform.position, walkSpeed * Time.deltaTime);
                    yield return null;
                }
                

                currentTransform = transformsTogo[0];
                transformsTogo.RemoveAt(0);

            }
            transformsTogo.Clear();
        }
        currentTransform.gameObject.GetComponent<RelaxPoint>().DoWork(this);

    }
    public IEnumerator MoveByRelaxPoint(List<RelaxPoint> relaxPoints)
    {
        RelaxPoint prev = null;
        if (relaxPoints != null)
        {
            while (relaxPoints.Count > 0)
            {
                yield return new WaitUntil(()=>relaxPoints[0].equipedNpc != null);
                if (prev != null)
                {
                    prev.equipedNpc = null;
                }
                while (Vector3.Distance(transform.position, relaxPoints[0].transform.position) > 0.1f)
                {

                    transform.position = Vector3.MoveTowards(transform.position, relaxPoints[0].transform.position, walkSpeed * Time.deltaTime);
                    yield return null;
                }
                relaxPoints[0].equipedNpc = this;
                prev = relaxPoints[0];
                if (relaxPoints.Count == 1)
                {
                    relaxPoints[0].DoWork(this);
                }
                
                currentTransform = relaxPoints[0].transform;
                relaxPoints.RemoveAt(0);
                
            }
            relaxPoints.Clear();
        }
        //currentTransform.gameObject.GetComponent<RelaxPoint>().DoWork(this);

    }
}
[CustomEditor(typeof(Npc))]
public class NpcEditor : Editor {



    
    string[] places;
    int placeIndex = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
     

        //if ( Application.isPlaying)
        //{
           
        //    Npc npc = (Npc)target;
            
        //        places = PathManager._instance.placeManager.places.Select(x => x.name).ToArray();

        //        placeIndex = EditorGUILayout.Popup(placeIndex, places);
        //        npc.togoPlace = PathManager._instance.placeManager.places[placeIndex];


        //    EditorUtility.SetDirty(target);
        //    if (GUILayout.Button("Move"))
        //    {
                

              
        //        npc.Move();
        //    }

        //    //if (GUILayout.Button("Go Relax"))
        //    //{
        //    //    npc.MoveRelax();
        //    //}
        //    //if (GUILayout.Button(" Relax To Place"))
        //    //{
        //    //    npc.RelaxToPlace();
        //    //}
        //}




    }
}
