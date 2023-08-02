using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public List<Path> paths;
    
    public void Set()
    {


        paths = GetComponentsInChildren<Path>().ToList();
        // common transform is the key and all connected path is in list
        Dictionary<Transform, List<Path>> dict = new();

        foreach (var path in paths)
        {
            path.directlyConnectedPaths.Clear();

            var places = path.connectedPlaces;

           
            foreach (var trans in path.transforms)
            {

                foreach (var placeInAPath in places)
                {
                    if (placeInAPath.pointOfEntrance == null ||
                        Vector3.Distance(placeInAPath.pointOfEntrance.position,placeInAPath.transform.position)>
                         Vector3.Distance(placeInAPath.pointOfEntrance.position, trans.position))
                    {
                        placeInAPath.pointOfEntrance = trans;
                    }
                }
                if (!dict.Keys.Contains(trans))
                {
                    dict[trans] = new();

                }
                dict[trans].Add(path);
                
            }
        }
        // setting all connected path
        foreach (var item in dict)
        {
            if (item.Value.Count>1)
            {
                    ConnectedPath commonPath = new (item.Value, item.Key);
                commonPath.SetDistance();
                foreach (var path in item.Value)
                {
                    if (!path.directlyConnectedPaths.Contains(commonPath))
                    {
                        path.directlyConnectedPaths.Add(commonPath);                        
                        path.SetDistance();
                    }
                   
                }
            }
        }
        



    }
    public Transform GetCommonTransform(List<Path> paths)
    {
        List<Transform> path1Transforms = paths[0].transforms;
        foreach (var transform in path1Transforms)
        {



            for (int i = 0; i < paths.Count; i++)
            {
                if (!paths[i].transforms.Contains(transform))
                {

                    break;
                }
                if (i== paths.Count-1)
                {
                    return transform;
                }
            }
          

        }
        return null;
    }
}

