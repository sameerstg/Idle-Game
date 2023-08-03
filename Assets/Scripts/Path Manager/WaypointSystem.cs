using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{

    public List<Waypoint> waypoints = new();
    public LineRenderer line;
    public GameObject lineParrent;
    
    private void Awake()
    {
        Set();
    }


    public void Initialize()
    {
        waypoints = GetComponentsInChildren<Waypoint>().ToList();
    }
    public void Set(bool makeBi = false)
    {
        Initialize();
        if (makeBi)
        {
            MakeAllBiDirectional();
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
         List<List<Waypoint>> traversed = new();
        foreach (var way in waypoints)
        {



            foreach (var item in way.connectedWaypoints)
            {
                if (traversed.Exists(x=>x.Contains(item) && x.Contains(way)))
                {
                    continue;
                }
                traversed.Add(new List<Waypoint> { way, item });
                var newLine = Instantiate(line, lineParrent.transform);
                newLine.SetPositions(new Vector3[] { way.transform.position, item.transform.position});
            }

        }
    }

    public void MakeAllBiDirectional()
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
   

}

