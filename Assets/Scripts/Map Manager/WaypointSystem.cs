using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{

    public static WaypointSystem _instance;
    public List<Waypoint> waypoints = new();
    public LineRenderer line;
    public GameObject lineParrent;
    private void Awake()
    {
        _instance = this; 
        Set();
    }


    public void Initialize()
    {
        waypoints = GetComponentsInChildren<Waypoint>().ToList();
    }
    public void Set()
    {
        Initialize();
        FixError();
        Visualize();
    }

    private void Visualize()
    {
        if (lineParrent != null)
        {

            DestroyImmediate(lineParrent);
        }
        lineParrent = new GameObject("lines");
         List<Waypoint> traversed = new();
        foreach (var way in waypoints)
        {


                traversed.Add(way);
            foreach (var item in way.connectedWaypoints)
            {
                if (traversed.Contains(item))
                {
                    continue;
                }
                var newLine = Instantiate(line, lineParrent.transform);
                newLine.SetPositions(new Vector3[] { way.transform.position, item.transform.position});
            }

        }
    }

    public void FixError()
    {
        foreach (var point in waypoints)
        {
            // if point contain itself it will delete it
            if (point.connectedWaypoints.Contains(point))
            {
                point.connectedWaypoints.RemoveAll(x => x == point);
            }
            foreach (var item in point.connectedWaypoints)
            {
                if (!item.connectedWaypoints.Contains(point))
                {
                    item.connectedWaypoints.Add(point);
                }
            }
        }
    }
    public List<Waypoint> GetWaypoint(Waypoint _currentWaypoint, Waypoint togoWaypoint)
    {
        if (_currentWaypoint == togoWaypoint)
        {
            Debug.LogError("current post = final pos");
            return null;
        }
        if (_currentWaypoint == null)
        {
            Debug.LogError("current waypoint is null");
            return null;
        }
        if (!waypoints.Contains(_currentWaypoint))
        {
            Debug.LogError("current waypoint is not in global list");
            return null;
        }
        if (_currentWaypoint.connectedWaypoints.Count == 0)
        {
            Debug.LogError("current waypoint is not connected with any waypoint");

            return null;
        }
        if (_currentWaypoint.connectedWaypoints.Contains(togoWaypoint))
        {
            return new List<Waypoint> { _currentWaypoint, togoWaypoint };
        }


        List<Tuple<Waypoint, Waypoint>> tobeVisitedWithParrent = new() { new Tuple<Waypoint, Waypoint>(_currentWaypoint, null) }, visited = new() { };
        while (tobeVisitedWithParrent.Count > 0)
        {
            var i = tobeVisitedWithParrent[0];
            visited.Add(i);
            tobeVisitedWithParrent.Remove(i);
            if (i.Item1 == togoWaypoint)
            {
                break;
            }
            foreach (var item in i.Item1.connectedWaypoints)
            {
                if (!visited.Exists(x => x.Item1 == item))
                {
                    tobeVisitedWithParrent.Add(new Tuple<Waypoint, Waypoint>(item, i.Item1));
                }
            }

        }
        List<Waypoint> ans = new();
        var index = visited[^1];
        while (index.Item2 != null)
        {
            ans.Add(index.Item1);
            var j = visited.IndexOf(visited.Find(x => x.Item1 == index.Item2));
            index = visited[j];
        }
        ans.Reverse();
        return ans;


    }

}
[CustomEditor(typeof(WaypointSystem))]
public class WaypointSystemEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WaypointSystem waypointSystem = (WaypointSystem)target;

        if (GUILayout.Button("Set"))
        {
            waypointSystem.Set();
        }

    }
}
