using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> connectedWaypoints = new();
}
[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor {
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Waypoint way = (Waypoint)target;
        EditorUtility.SetDirty(target);

        //if (way.containsRelaxPoints)
        //{

        //}

    }
}
