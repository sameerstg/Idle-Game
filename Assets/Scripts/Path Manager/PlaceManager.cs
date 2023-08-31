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

        places = places.OrderBy(x => x.name).ToList();
        foreach (var place in places)
        {
            place.Set();
        }
        Visualize(); 
    }
    void Visualize()
    {
        if (parrent != null)
        {
            DestroyImmediate(parrent);
            parrent = null;
        }
        parrent = new GameObject("Relax Lines");
        List<Point> visited = new(), tobeVisited = new();
        foreach (var place in places)
        {
            //foreach (var relax in place.pointConnection.relaxPoints)
            //{
            //    if (!tobeVisited.Contains(relax))
            //    {
            //        tobeVisited.Add(relax);
            //    }
            //}
            tobeVisited.AddRange(place.pointConnection.relaxPoints);
            tobeVisited.AddRange(place.pointConnection.indirectRelaxPoints);
        }

        while (tobeVisited.Count > 0)
        {
            var i = tobeVisited[0];
            tobeVisited.Remove(i);
            if (!visited.Contains(i))
            {
                visited.Add(i);
                //tobeVisited.AddRange(i.pointConnection.connectedPoints);
                foreach (var item in i.pointConnection.connectedPoints)
                {
                    Instantiate(line, parrent.transform).SetPositions(new Vector3[] { i.transform.position, item.transform.position });
                }
            }
        }
    }
    
}
