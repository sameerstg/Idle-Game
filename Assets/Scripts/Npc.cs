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
        StartCoroutine(MoveByOneWaypoints());
    }

    public void MoveToRelaxPoint()
    {
        if (togoPlace != null && togoPlace.HaveEmptyRelaxPoint())
        {
            var pathWithRelaxWaypoint = togoPlace.GetPathRelaxPoint();
            if (pathWithRelaxWaypoint.Item2 != null)
            {
                transformsTogo = pathWithRelaxWaypoint.Item1;
                StartCoroutine(MoveByOneTransforms());
            }
        }
        statemachine.SwitchState(new IdleState(this));
    }
    public IEnumerator MoveByOneWaypoints()
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

    public IEnumerator MoveByOneTransforms()
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
        statemachine.currentState.Exit();

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
