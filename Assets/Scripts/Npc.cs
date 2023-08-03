using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public float walkSpeed,runSpeed;
    public Waypoint currentWaypoint, togoWaypoint;
    public List<Waypoint> togoWaypoints = new();

    public void Move()
    {
        togoWaypoints = WaypointSystem._instance.GetWaypoint(currentWaypoint,togoWaypoint);
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



    string[] _choices;
        int _choiceIndex = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
     

        if ( Application.isPlaying)
        {
            Npc npc = (Npc)target;
            _choices = WaypointSystem._instance.waypoints.Select(x => x.name).ToArray();
            _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
                npc.togoWaypoint = WaypointSystem._instance.waypoints[_choiceIndex];
            EditorUtility.SetDirty(target);
            if (GUILayout.Button("Move"))
            {
              
                npc.Move();
            }

        }




    }
}
