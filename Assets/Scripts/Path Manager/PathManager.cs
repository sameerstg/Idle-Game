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
    void Initialize()
    {
        waypointSystem = GetComponentInChildren<WaypointSystem>();
        placeManager = GetComponentInChildren<PlaceManager>();
    }
    public void Set(bool bi = false)
    {
        Initialize();
        waypointSystem.Set(bi);
        placeManager.Set();
    }
   
   
    public Place GetPlace(PlaceName placeName,bool withEmptyRelax = false)
    {
        if (withEmptyRelax )
        {
            return placeManager.places.Find(x => x.placeName == placeName && x.HaveEmptyRelaxPoint());
        }
        return  placeManager.places.Find(x => x.placeName == placeName ); 
    }
    public List<Point> GetPath(Point _currentWaypoint, Place place)
    {
        Debug.Log(place);
        Point togoWaypoint = place.pointConnection.connectedPoints.OrderBy(x => Vector3.Distance(x.transform.position, _currentWaypoint.transform.position))?.First();

        togoWaypoint ??= place.pointConnection.indirectConnectedPoints.OrderBy(x => Vector3.Distance(x.transform.position, _currentWaypoint.transform.position))?.First();

            Debug.Log(togoWaypoint);
        
        return GetPath(_currentWaypoint, togoWaypoint)??new List<Point>();
    }
    public List<Point> GetPath(Point _currentWaypoint, Point togoWaypoint)
    {
        if (_currentWaypoint == togoWaypoint)
        {
            Debug.LogError("current post = final pos");
            return null;
        }
        if (_currentWaypoint == null || togoWaypoint == null)
        {
            Debug.LogError("current waypoint is null");
            return null;
        }
        if (!waypointSystem.waypoints.Contains(_currentWaypoint))
        {
            Debug.LogError("current waypoint is not in global list");
            return null;
        }
        if (_currentWaypoint.pointConnection.connectedPoints.Count == 0)
        {
            Debug.LogError("current waypoint is not connected with any waypoint");

            return null;
        }
        if (_currentWaypoint.pointConnection.connectedPoints.Contains(togoWaypoint))
        {
            return new List<Point> { _currentWaypoint, togoWaypoint };
        }

        
        // first one is chile then second one is parent
        List<Tuple<Point, Point>> tobeVisitedWithParrent = new() { new Tuple<Point, Point>(_currentWaypoint, null) }, visited = new() { };
        while (tobeVisitedWithParrent.Count > 0)
        {
            var i = tobeVisitedWithParrent.OrderBy(x=>Vector3.Distance( x.Item1.transform.position,_currentWaypoint.transform.position)).First();
            visited.Add(i);
            tobeVisitedWithParrent.Remove(i);
            if (i.Item1 == togoWaypoint)
            {
                break;
            }
            foreach (var item in i.Item1.pointConnection.connectedPoints)
            {
                if (!visited.Exists(x => x.Item1 == item))
                {
                    tobeVisitedWithParrent.Add(new Tuple<Point, Point>(item, i.Item1));
                }
            }

        }
       
        List<Point> ans = new();
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
