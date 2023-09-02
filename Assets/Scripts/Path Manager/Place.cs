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
        return pointConnection.relaxPoints.Exists(x =>x.pointConnection.relaxPoints.Count==0 && x.pointConnection.relaxPoints.Count==0 ||x.equipedNpc == null) || pointConnection.indirectRelaxPoints.Exists(x => x.equipedNpc == null);
    }
    /// <summary>
    /// 1= parent 2= relax
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Tuple<Point,Point> GetNearestConnecetedPointAndItsEmptyRelax(Point point)
    {
        List<Point> allConnectedPointWithEmptyReplax = pointConnection.connectedPoints.FindAll(x => x.pointConnection.relaxPoints.Exists(x => x.equipedNpc == null));
        allConnectedPointWithEmptyReplax.AddRange(pointConnection.indirectConnectedPoints.FindAll(x => x.pointConnection.relaxPoints.Exists(x=>x.equipedNpc == null)));
        var closeConnected = allConnectedPointWithEmptyReplax.OrderBy(x => Vector3.Distance(point.transform.position, x.transform.position))?.First();
        return new Tuple<Point,Point>(closeConnected,  closeConnected.pointConnection.relaxPoints.Find(x=>x.equipedNpc == null));

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
        var nearestConnecetedPointAndItsEmptyRelax = GetNearestConnecetedPointAndItsEmptyRelax(currentPointOfNpcInPlace);

        if (currentPointOfNpcInPlace != nearestConnecetedPointAndItsEmptyRelax.Item1)
        {
            points.AddRange(PathManager._instance.GetPath(currentPointOfNpcInPlace, nearestConnecetedPointAndItsEmptyRelax.Item1));

        }
        if (RelaxPointType == RelaxPointType.work)
        {
            var allRelax = nearestConnecetedPointAndItsEmptyRelax.Item1.pointConnection.relaxPoints.ToList();
            allRelax.Reverse();
            points.AddRange(allRelax);
        }
        else
        {
            points.Add(nearestConnecetedPointAndItsEmptyRelax.Item2);
        }
        return points;

    }
   
}

public enum PlaceName
{
   None, OuterEntrance,Entrance,Cell,Entertainment,FoodRoom,Bathroom,FoodPrepartaionRoom,ElectricSupply
}
public enum PointType
{
    none,place,relaxPoint,workPoint,wayPoint
}
