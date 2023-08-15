using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    public List<Place> places = new();
    public LineRenderer line;
    public GameObject parrent;
    public void Set()
    {
        places.Clear();
        places = ((Place[])FindObjectsByType(typeof(Place),FindObjectsSortMode.None)).ToList();
        if (parrent != null)
        {
            DestroyImmediate(parrent);
            parrent = null;
        }
        parrent = new GameObject("Relax Lines");
            List<RelaxWaypoint> visited=new(), tobeVisited = new();
        foreach (var place in places)
        {
            place.Set();


            foreach (var relaxWaypoint in place.relaxWaypoints)
            {
                if (!tobeVisited.Contains(relaxWaypoint))
                {
                    tobeVisited.Add(relaxWaypoint);
                }
            }
        }
        do
        {
            var i = tobeVisited[0];
            tobeVisited.Remove(i);
            if (!visited.Contains(i))
            {
                visited.Add(i);
                tobeVisited.AddRange(i.connectedRelaxWaypoints);
                foreach (var item in i.relaxpoints)
                {
                    Instantiate(line, parrent.transform).SetPositions(new Vector3[] { i.transform.position,item.transform.position});
                }
            }
            
        } while (tobeVisited.Count>0);

    }
    
}
