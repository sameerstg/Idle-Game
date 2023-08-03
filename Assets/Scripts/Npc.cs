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
        StartCoroutine(MoveByOne());
    }
    public IEnumerator MoveByOne()
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

        }




    }
}
