using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Place : MonoBehaviour
{
    public PlaceName placeName;

    public RelaxPointType RelaxPointType;
    public PointConnection pointConnection;
    public float relaxpointWaitTime;
    
    public void Set()
    {
        //relaxWaypoints = GetComponentsInChildren<RelaxWaypoint>().ToList();
        pointConnection. relaxPoints.Clear();
        pointConnection. connectedPoints.Clear();
        foreach (var point in pointConnection.allPoints)
        {


            if (point.positionType == PointType.relaxPoint || point.positionType == PointType.workPoint)
            {
                if (!pointConnection.relaxPoints.Contains(point))
                {
                    point.waitTime = relaxpointWaitTime;
                    pointConnection.relaxPoints.Add(point);
                }
            }
            else if(point.positionType == PointType.wayPoint)
            {
                if (!pointConnection.connectedPoints.Contains(point))
                {
                    point.waitTime = relaxpointWaitTime;
                    pointConnection.connectedPoints.Add(point);
                }

            }
           
               
           
        }
        
    }
    public bool HaveEmptyRelaxPoint()
    {
        return pointConnection.relaxPoints.Exists(x => x.equipedNpc == null);
    }
    public List<Point> GetPathToWorkRelaxPoint()
    {
        var rev = pointConnection.relaxPoints.ToList();
        rev.Reverse();
        return rev;
    }
    public Tuple<List<Transform>, Point> GetPathToRelaxPoint()
    {
        List<Transform> path = new();
        if (pointConnection.connectedPoints == null || pointConnection.connectedPoints.Count==0)
        {
            Debug.Log("hehere");
            return null;
        }
       
        List<Tuple<Point, Point>> tobeVisited = new(),visited = new() {  };
        foreach (var item in pointConnection.connectedPoints)
        {
            tobeVisited.Add(new Tuple<Point, Point>(item, null));
        }
        while (tobeVisited.Count > 0)
        {
            var i = tobeVisited[0];
            visited.Add(i);
            tobeVisited.Remove(i);

            if (i.Item1.pointConnection.relaxPoints.Exists(x => x.equipedNpc == null))
            {
                break;
            }
            foreach (var item in i.Item1.pointConnection.connectedPoints)
            {
                if (!visited.Exists(x => x.Item1 == item) && !tobeVisited.Exists(x=>x.Item1==item))
                {
                    tobeVisited.Add(new Tuple<Point, Point>(item, i.Item1));
                }     
            }
          
        }
        var index = visited[^1];
        if (!index.Item1.pointConnection.relaxPoints.Exists(x=>x.equipedNpc == null))
        {
            Debug.Log("hehere");

            return null;
        }
        Point relax = index.Item1;
        path.Add(index.Item1.pointConnection.relaxPoints.Find(x => x.equipedNpc == null).transform);
        while (index.Item2 != null)
        {
            path.Add(index.Item1.transform);
            var j = visited.IndexOf(visited.Find(x => x.Item1 == index.Item2));
            index = visited[j];
        }
        path.Add(index.Item1.transform);
        path.Reverse();
        return new Tuple<List<Transform>, Point>(path, relax);
    }
    public List<Transform> GetPathRelaxPoint(Point start)
    {
        List<Transform> path = new();
        if (pointConnection.connectedPoints == null || pointConnection.connectedPoints.Count == 0)
        {
            Debug.Log("hehere");
            return null;
        }

        List<Tuple<Point, Point>> tobeVisited = new(), visited = new() { };
        foreach (var item in pointConnection.connectedPoints)
        {
            tobeVisited.Add(new Tuple<Point, Point>(item, null));
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
            foreach (var item in i.Item1.pointConnection.connectedPoints)
            {
                if (!visited.Exists(x => x.Item1 == item) && !tobeVisited.Exists(x => x.Item1 == item))
                {
                    tobeVisited.Add(new Tuple<Point, Point>(item, i.Item1));
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
public enum PointType
{
    none,place,relaxPoint,workPoint,wayPoint
}
