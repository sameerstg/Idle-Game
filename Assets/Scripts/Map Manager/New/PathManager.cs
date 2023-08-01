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

        Dictionary<Transform, List<Path>> dict = new();

        foreach (var path in paths)
        {
            path.connectedPath.Clear();
            foreach (var trans in path.transforms)
            {
                if (!dict.Keys.Contains(trans))
                {
                    dict[trans] = new();

                }
                dict[trans].Add(path);
                
            }
        }
        foreach (var item in dict)
        {
            if (item.Value.Count>1)
            {
                foreach (var path in item.Value)
                {
                    
                    path.connectedPath.AddRange(item.Value);
                    path.connectedPath.Remove(path);
                }
            }
        }



    }
}

