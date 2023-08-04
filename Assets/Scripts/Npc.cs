using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public float walkSpeed,runSpeed;
    public Waypoint currentWaypoint, togoWaypoint;
    public Place togoPlace;
    public List<Waypoint> togoWaypoints = new();
    public bool useTogoWaypoint;

    public List<Transform> transformsTogo =new();
    public Transform currentTransform;
    public RelaxWaypoint relaxWaypoint;
    
    public void Move()
    {
        if (useTogoWaypoint)
        {
        togoWaypoints = PathManager._instance.GetPath(currentWaypoint,togoWaypoint);

        }
        else
        {
            togoWaypoints = PathManager._instance.GetPath(currentWaypoint, togoPlace);

        }
        StartCoroutine(MoveByOneWaypoints());
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

    }
    internal void MoveRelax()
    {
        var relaxPath = togoPlace.GetPathRelaxPoint();
        if (relaxPath == null||relaxPath.Item1 == null)
        {
            return;
        }
        transformsTogo = relaxPath.Item1;
        relaxWaypoint = relaxPath.Item2;
        StartCoroutine(MoveByOneTransforms());
    }
    internal void RelaxToPlace()
    {
        transformsTogo = togoPlace.GetPathRelaxPoint(relaxWaypoint);
        if (transformsTogo == null)
        {
            return;
        }
        StartCoroutine(MoveByOneTransforms());
    }
}
[CustomEditor(typeof(Npc))]
public class NpcEditor : Editor {



    string[] waypoints;
        int waypointIndex = 0;
    string[] places;
    int placeIndex = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
     

        if ( Application.isPlaying)
        {
           
            Npc npc = (Npc)target;
            if (npc.useTogoWaypoint)
            {
                waypoints = PathManager._instance.waypointSystem.waypoints.Select(x => x.name).ToArray();
                waypointIndex = EditorGUILayout.Popup(waypointIndex, waypoints);
                npc.togoWaypoint = PathManager._instance.waypointSystem.waypoints[waypointIndex];

            }

            else{
                places = PathManager._instance.placeManager.places.Select(x => x.name).ToArray();

                placeIndex = EditorGUILayout.Popup(placeIndex, places);
                npc.togoPlace = PathManager._instance.placeManager.places[placeIndex];

            }

            EditorUtility.SetDirty(target);
            if (GUILayout.Button("Move"))
            {
                

              
                npc.Move();
            }

            if (GUILayout.Button("Go Relax"))
            {
                npc.MoveRelax();
            }
            if (GUILayout.Button(" Relax To Place"))
            {
                npc.RelaxToPlace();
            }
        }




    }
}
