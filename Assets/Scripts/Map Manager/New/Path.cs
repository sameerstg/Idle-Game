using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> transforms = new();
    public List<PlaceGo> connectedPlaces = new();
    public List<ConnectedPath> connectedPaths = new();
    public float distance;
    
    public void SetDistance()
    {
        if (transforms.Count < 2)
        {
            return;
        }
        distance = 0;
        for (int i = 1; i < transforms.Count ; i++)
        {
            distance += Vector3.Distance(transforms[i - 1].position, transforms[i].position);
        }
    }
}
[System.Serializable]
public class ConnectedPath
{
    public List<Path> paths = new();
    public Transform commonPath;
    public float distance;
    public void SetDistance()
    {

        distance = 0;
        foreach (var path in paths)
        {
            distance += path.distance;
        }
    }
    public ConnectedPath(List<Path> paths, Transform commonPath)
    {
        this.paths = paths;
        this.commonPath = commonPath;
}
    public ConnectedPath()
    {

    }
}
