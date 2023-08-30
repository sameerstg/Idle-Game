using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Place : MonoBehaviour
{
    public PlaceName placeName;

    public RelaxPointType RelaxPointType;
    public List<Waypoint> connectedWaypoints = new();
    public List<RelaxWaypoint> relaxWaypoints;
    public float relaxpointWaitTime;
    [Header("Dont assign")]
    public List<RelaxPoint> relaxPoints = new();
    
    public void Set()
    {
        //relaxWaypoints = GetComponentsInChildren<RelaxWaypoint>().ToList();
        relaxPoints.Clear();
        foreach (var relaxWaypoint in relaxWaypoints)
        {

            foreach (var relaxPoint in relaxWaypoint.relaxpoints)
            {
                if (!relaxPoints.Contains(relaxPoint))
                {
                    relaxPoint.waitTime = relaxpointWaitTime;
                    relaxPoints.Add(relaxPoint);
                }
            }
        }
        
    }
    public bool HaveEmptyRelaxPoint()
    {
        return relaxPoints.Exists(x => x.equipedNpc == null);
    }
    public List<RelaxPoint> GetPathToWorkRelaxPoint()
    {
        var rev = relaxPoints.ToList();
        rev.Reverse();
        return rev;
    }
    public Tuple<List<Transform>, RelaxWaypoint> GetPathToRelaxPoint()
    {
        List<Transform> path = new();
        if (relaxWaypoints == null || relaxWaypoints.Count==0)
        {
            Debug.Log("hehere");
            return null;
        }
       
        List<Tuple<RelaxWaypoint, RelaxWaypoint>> tobeVisited = new(),visited = new() {  };
        foreach (var item in relaxWaypoints)
        {
            tobeVisited.Add(new Tuple<RelaxWaypoint, RelaxWaypoint>(item, null));
        }
        while (tobeVisited.Count > 0)
        {
            var i = tobeVisited[0];
            visited.Add(i);
            tobeVisited.Remove(i);

            if (i.Item1.relaxpoints.Exists(x => x.equipedNpc == null))
            {
                break;
            }
            foreach (var item in i.Item1.connectedRelaxWaypoints)
            {
                if (!visited.Exists(x => x.Item1 == item) && !tobeVisited.Exists(x=>x.Item1==item))
                {
                    tobeVisited.Add(new Tuple<RelaxWaypoint, RelaxWaypoint>(item, i.Item1));
                }     
            }
          
        }
        var index = visited[^1];
        if (!index.Item1.relaxpoints.Exists(x=>x.equipedNpc == null))
        {
            Debug.Log("hehere");

            return null;
        }
        RelaxWaypoint relax = index.Item1;
        path.Add(index.Item1.relaxpoints.Find(x => x.equipedNpc == null).transform);
        while (index.Item2 != null)
        {
            path.Add(index.Item1.transform);
            var j = visited.IndexOf(visited.Find(x => x.Item1 == index.Item2));
            index = visited[j];
        }
        path.Add(index.Item1.transform);
        path.Reverse();
        return new Tuple<List<Transform>, RelaxWaypoint>(path, relax);
    }
    public List<Transform> GetPathRelaxPoint(RelaxWaypoint start)
    {
        List<Transform> path = new();
        if (relaxWaypoints == null || relaxWaypoints.Count == 0)
        {
            Debug.Log("hehere");
            return null;
        }

        List<Tuple<RelaxWaypoint, RelaxWaypoint>> tobeVisited = new(), visited = new() { };
        foreach (var item in relaxWaypoints)
        {
            tobeVisited.Add(new Tuple<RelaxWaypoint, RelaxWaypoint>(item, null));
        }
        while (tobeVisited.Count > 0)
        {
            var i = tobeVisited[0];
            visited.Add(i);
            tobeVisited.Remove(i);

            if (i.Item1 == start)
            {
                break;
            }
            foreach (var item in i.Item1.connectedRelaxWaypoints)
            {
                if (!visited.Exists(x => x.Item1 == item) && !tobeVisited.Exists(x => x.Item1 == item))
                {
                    tobeVisited.Add(new Tuple<RelaxWaypoint, RelaxWaypoint>(item, i.Item1));
                }
            }

        }
        var index = visited[^1];
        if (!index.Item1 == start)
        {
            Debug.Log("hehere");

            return null;
        }
        
        while (index.Item2 != null)
        {
            path.Add(index.Item1.transform);
            var j = visited.IndexOf(visited.Find(x => x.Item1 == index.Item2));
            index = visited[j];
        }
        path.Add(index.Item1.transform);
        
        return path;
    }
}

public enum PlaceName
{
    OuterEntrance,Entrance,Cell,Entertainment,FoodRoom,Bathroom,FoodPrepartaionRoom,ElectricSupply
}
public enum DestinationType
{
    none,place,relaxPoint,workPoint,wayPoint
}
