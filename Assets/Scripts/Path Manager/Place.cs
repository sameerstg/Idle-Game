using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Place : MonoBehaviour
{
    public PlaceName placeName;

    public RelaxPointType RelaxPointType;
    public PointConnectionForPlace pointConnection;
    public float relaxpointWaitTime;
    
    public void Set()
    {
        pointConnection.OrganizeAllPointsForPlace();   
    }
    public bool HaveEmptyRelaxPoint()
    {
        return pointConnection.relaxPoints.Exists(x => x.equipedNpc == null) || pointConnection.indirectRelaxPoints.Exists(x => x.equipedNpc == null);
    }
    public Point GetNearestPointWithEmptyRelax(Point point)
    {
        List<Point> allConnectedPointWithEmptyReplax = pointConnection.relaxPoints.FindAll(x=>x.equipedNpc == null);
        allConnectedPointWithEmptyReplax.AddRange(pointConnection.indirectRelaxPoints.FindAll(x => x.equipedNpc == null));
        return allConnectedPointWithEmptyReplax.OrderBy(x => Vector3.Distance(point.transform.position, x.transform.position))?.First();

    }
    public List<Point> GetPathFromPlaceToRelax(Place place,Point currentPointOfNpcInPlace)
    {
        List<Point> points = new();
        // ensuring place have empty space
        if (!place.HaveEmptyRelaxPoint())
        {
            return points;
        }
        // ensuring current position is directly connected to the same place
        if (!place.pointConnection.connectedPoints.Contains(currentPointOfNpcInPlace))
        {
            return points;
        }
        var nearest = GetNearestPointWithEmptyRelax(currentPointOfNpcInPlace);
        var destPoint = nearest.pointConnection.connectedPoints[0];

        points.AddRange(PathManager._instance.GetPath(currentPointOfNpcInPlace, destPoint));
        points.Add(nearest);
        return points;

    }
    //public Tuple<List<Transform>, Point> GetPathToRelaxPoint()
    //{
    //    List<Transform> path = new();
    //    if (pointConnection.connectedPoints == null || pointConnection.connectedPoints.Count==0)
    //    {
    //        Debug.Log("hehere");
    //        return null;
    //    }
       
    //    List<Tuple<Point, Point>> tobeVisited = new(),visited = new() {  };
    //    foreach (var item in pointConnection.connectedPoints)
    //    {
    //        tobeVisited.Add(new Tuple<Point, Point>(item, null));
    //    }
    //    while (tobeVisited.Count > 0)
    //    {
    //        var i = tobeVisited[0];
    //        visited.Add(i);
    //        tobeVisited.Remove(i);

    //        if (i.Item1.pointConnection.relaxPoints.Exists(x => x.equipedNpc == null))
    //        {
    //            break;
    //        }
    //        foreach (var item in i.Item1.pointConnection.connectedPoints)
    //        {
    //            if (!visited.Exists(x => x.Item1 == item) && !tobeVisited.Exists(x=>x.Item1==item))
    //            {
    //                tobeVisited.Add(new Tuple<Point, Point>(item, i.Item1));
    //            }     
    //        }
          
    //    }
    //    var index = visited[^1];
    //    if (!index.Item1.pointConnection.relaxPoints.Exists(x=>x.equipedNpc == null))
    //    {
    //        Debug.Log("hehere");

    //        return null;
    //    }
    //    Point relax = index.Item1;
    //    path.Add(index.Item1.pointConnection.relaxPoints.Find(x => x.equipedNpc == null).transform);
    //    while (index.Item2 != null)
    //    {
    //        path.Add(index.Item1.transform);
    //        var j = visited.IndexOf(visited.Find(x => x.Item1 == index.Item2));
    //        index = visited[j];
    //    }
    //    path.Add(index.Item1.transform);
    //    path.Reverse();
    //    return new Tuple<List<Transform>, Point>(path, relax);
    //}
    //public List<Transform> GetPathRelaxPoint(Point start)
    //{
    //    List<Transform> path = new();
    //    if (pointConnection.connectedPoints == null || pointConnection.connectedPoints.Count == 0)
    //    {
    //        Debug.Log("hehere");
    //        return null;
    //    }

    //    List<Tuple<Point, Point>> tobeVisited = new(), visited = new() { };
    //    foreach (var item in pointConnection.connectedPoints)
    //    {
    //        tobeVisited.Add(new Tuple<Point, Point>(item, null));
    //    }
    //    while (tobeVisited.Count > 0)
    //    {
    //        var i = tobeVisited[0];
    //        visited.Add(i);
    //        tobeVisited.Remove(i);

    //        if (i.Item1 == start)
    //        {
    //            break;
    //        }
    //        foreach (var item in i.Item1.pointConnection.connectedPoints)
    //        {
    //            if (!visited.Exists(x => x.Item1 == item) && !tobeVisited.Exists(x => x.Item1 == item))
    //            {
    //                tobeVisited.Add(new Tuple<Point, Point>(item, i.Item1));
    //            }
    //        }

    //    }
    //    var index = visited[^1];
    //    if (!index.Item1 == start)
    //    {
    //        Debug.Log("hehere");

    //        return null;
    //    }
        
    //    while (index.Item2 != null)
    //    {
    //        path.Add(index.Item1.transform);
    //        var j = visited.IndexOf(visited.Find(x => x.Item1 == index.Item2));
    //        index = visited[j];
    //    }
    //    path.Add(index.Item1.transform);
        
    //    return path;
    //}
}

public enum PlaceName
{
    OuterEntrance,Entrance,Cell,Entertainment,FoodRoom,Bathroom,FoodPrepartaionRoom,ElectricSupply
}
public enum PointType
{
    none,place,relaxPoint,workPoint,wayPoint
}
