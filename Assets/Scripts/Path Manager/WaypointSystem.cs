using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{

    public List<Point> waypoints = new();
    public LineRenderer line;
    public GameObject lineParrent;
    
    //private void Awake()
    //{
    //    Set();
    //}


   
    public void Set(bool makeBi = false)
    {
        waypoints.Clear();
        foreach (var item in ((Point[])FindObjectsOfType(typeof(Point))))
        {
            if (!waypoints.Contains(item))
            {
                if (item.pointType == PointType.wayPoint)
                {

                    waypoints.Add(item);
                }
                item.pointConnection.OrganizeAllPoints(item, makeBi);
            }
        }
        Visualize();
    }

    private void Visualize()
    {
        if (lineParrent != null)
        {

            DestroyImmediate(lineParrent);
        }
        lineParrent = new GameObject("lines");
         List<List<Point>> traversed = new();
        foreach (var way in waypoints)
        {



            foreach (var item in way.pointConnection.connectedPoints)
            {
                if (traversed.Exists(x=>x.Contains(item) && x.Contains(way)))
                {
                    continue;
                }
                traversed.Add(new List<Point> { way, item });
                var newLine = Instantiate(line, lineParrent.transform);
                newLine.SetPositions(new Vector3[] { way.transform.position, item.transform.position});
            }

        }
    }

    //public void MakeAllBiDirectional()
    //{
    //    foreach (var point in waypoints)
    //    {
    //        point.pointConnection.connectedPoints.Clear();
    //        point.pointConnection.relaxPoints.Clear();
    //        // if point contain itself it will delete it
    //        if (point.pointConnection.allPoints.Contains(point))
    //        {
    //            point.pointConnection.allPoints.RemoveAll(x => x == point);
               
    //        }
    //        else
    //        {
    //            foreach (var connectedPoint in point.pointConnection.allPoints)
    //            {
    //                if (connectedPoint.positionType == PointType.wayPoint)
    //                {

    //                    if (!point.pointConnection.connectedPoints.Contains(connectedPoint))
    //                    {

    //                        point.pointConnection.connectedPoints.Add(connectedPoint);
                            
                            
    //                    }
    //                    // for making bi
    //                    if (!connectedPoint.pointConnection.allPoints.Contains(point))
    //                    {
    //                        connectedPoint.pointConnection.allPoints.Add(point);
    //                    }
    //                }
    //                else if (connectedPoint.positionType == PointType.relaxPoint ||  connectedPoint.positionType == PointType.workPoint)
    //                {
    //                    if (!point.pointConnection.relaxPoints.Contains(connectedPoint))
    //                    {
    //                        point.pointConnection.relaxPoints.Add(connectedPoint);
    //                    }
    //                    // for making bi
    //                    if (!connectedPoint.pointConnection.connectedPoints.Contains(point))
    //                    {
    //                        connectedPoint.pointConnection.connectedPoints.Add(point);
    //                    }
    //                }
    //            }
    //        }
    //        //foreach (var item in point.pointConnection.connectedPoints)
    //        //{
    //        //    if (!item.pointConnection.connectedPoints.Contains(point))
    //        //    {
    //        //        item.pointConnection.connectedPoints.Add(point);
    //        //    }
    //        //}
    //    }
    //}
   

}

