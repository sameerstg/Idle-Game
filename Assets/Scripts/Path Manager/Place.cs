using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Place : MonoBehaviour
{

    public List<Waypoint> points = new();
    public List<RelaxWaypoint> relaxWaypoints;

    private void Awake()
    {
        relaxWaypoints = GetComponentsInChildren<RelaxWaypoint>().ToList();
    }
    public List<Transform> GetPathRelaxPoint()
    {
        List<Transform> path = new();
        if (relaxWaypoints == null || relaxWaypoints.Count==0)
        {
            Debug.Log("hehere");
            return path;
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

            return path;
        }
        path.Add(index.Item1.relaxpoints.Find(x => x.equipedNpc == null).transform);
        while (index.Item2 != null)
        {
            path.Add(index.Item1.transform);
            var j = visited.IndexOf(visited.Find(x => x.Item1 == index.Item2));
            index = visited[j];
        }
        path.Add(index.Item1.transform);
        path.Reverse();
        return path;
    }
}
