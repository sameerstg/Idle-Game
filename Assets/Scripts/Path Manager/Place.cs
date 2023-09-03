using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
        return pointConnection.relaxPoints.Count == 0 && pointConnection.indirectRelaxPoints.Count == 0 ||
            pointConnection.relaxPoints.Exists(x => x.equipedNpc == null) || pointConnection.indirectRelaxPoints.Exists(x => x.equipedNpc == null);
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
    public List<Point> GetPathFromPlaceToRelax(Place place,Point currentPointOfNpcInPlace,bool nearest = false)
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
        if (RelaxPointType == RelaxPointType.work)
        {
           List<Point> listOfContainingEmpthRelax = pointConnection.connectedPoints.FindAll(x=>x.pointConnection.relaxPoints.Exists(x=>x.equipedNpc ==null));
            listOfContainingEmpthRelax.AddRange(pointConnection.indirectConnectedPoints.FindAll(x => x.pointConnection.relaxPoints.Exists(x => x.equipedNpc == null)));
            int i = 0;
            while (listOfContainingEmpthRelax.Count>0)
            {
                foreach (var pointWithRelax in listOfContainingEmpthRelax)
                {
                    if (pointWithRelax.pointConnection.relaxPoints.Count-1<i)
                    {
                        listOfContainingEmpthRelax.Remove(pointWithRelax);
                    }
                    else if (pointWithRelax.pointConnection.relaxPoints[i].equipedNpc == null)
                    {
                        points = pointWithRelax.pointConnection.relaxPoints.ToList(); 
                        points.Reverse();
                        return points;
                    }
                }
                i++;
            }
            return points;
        }
        else if (RelaxPointType == RelaxPointType.relax)
        {
            if (nearest)
            {
                var nearestConnecetedPointAndItsEmptyRelax = GetNearestConnecetedPointAndItsEmptyRelax(currentPointOfNpcInPlace);

                if (currentPointOfNpcInPlace != nearestConnecetedPointAndItsEmptyRelax.Item1)
                {
                    points.AddRange(PathManager._instance.GetPath(currentPointOfNpcInPlace, nearestConnecetedPointAndItsEmptyRelax.Item1));

                }

                                    points.Add(nearestConnecetedPointAndItsEmptyRelax.Item2);
                
                return points;

            }
            else
            {
                List<Point> listOfContainingEmpthRelax = pointConnection.connectedPoints.FindAll(x => x.pointConnection.relaxPoints.Exists(x => x.equipedNpc == null));
                listOfContainingEmpthRelax.AddRange(pointConnection.indirectConnectedPoints.FindAll(x => x.pointConnection.relaxPoints.Exists(x => x.equipedNpc == null)));
                Point togo = listOfContainingEmpthRelax[UnityEngine.Random.Range(0, listOfContainingEmpthRelax.Count)];
                if (currentPointOfNpcInPlace != togo)
                {
                    points.AddRange(PathManager._instance.GetPath(currentPointOfNpcInPlace, togo));

                }

                List<Point> relaxes = togo.pointConnection.relaxPoints.FindAll(x => x.equipedNpc == null);
                points.Add(relaxes[UnityEngine.Random.Range(0, relaxes.Count)]);
                return points;
            }
        }
        return points;


    }

}

public enum PlaceName
{
   None, OuterEntrance,Entrance,Cell,Entertainment,FoodRoom,Bathroom,FoodPrepartaionRoom,ElectricSupply,DinningTable
}
public enum PointType
{
    none,place,relaxPoint,workPoint,wayPoint
}
