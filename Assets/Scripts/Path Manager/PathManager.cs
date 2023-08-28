using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager _instance;
    public WaypointSystem waypointSystem;
    public PlaceManager placeManager;
    private void Awake()
    {
        _instance = this;
    }

    public void Set(bool bi = false)
    {
        Initialize();
        waypointSystem.Set(bi);
        placeManager.Set();
    }
    void Initialize()
    {
        waypointSystem = GetComponentInChildren<WaypointSystem>();
        placeManager = GetComponentInChildren<PlaceManager>();
    }
   
    public Place GetPlace(PlaceName placeName)
    {
        return  placeManager.places.Find(x => x.placeName == placeName && x.HaveEmptyRelaxPoint()); 
    }
    public List<Waypoint> GetPath(Waypoint _currentWaypoint, Place place)
    {
        
        Waypoint togoWaypoint = place.connectedWaypoints.OrderBy(x => Vector3.Distance(x.transform.position, _currentWaypoint.transform.position)).First();
        return GetPath(_currentWaypoint, togoWaypoint);
    }
    public List<Waypoint> GetPath(Waypoint _currentWaypoint, Waypoint togoWaypoint)
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
        if (!waypointSystem.waypoints.Contains(_currentWaypoint))
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
        if (index.Item1 != togoWaypoint)
        {
            return null;
        }
        while (index.Item2 != null)
        {
            ans.Add(index.Item1);
            var j = visited.IndexOf(visited.Find(x => x.Item1 == index.Item2));
            index = visited[j];
        }
        ans.Add(index.Item1);
        ans.Reverse();
        Debug.Log(ans.Count);
        return ans;


    }
}
[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PathManager pathManager = (PathManager)target;

        if (GUILayout.Button("Set"))
        {
            pathManager.Set();
        }
        if (GUILayout.Button("Set and Make Bi"))
        {
            pathManager.Set(true);
        }

    }
}
