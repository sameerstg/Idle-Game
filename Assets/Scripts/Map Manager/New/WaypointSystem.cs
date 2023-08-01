using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{

    public PathManager pathManager;
    public PlaceMananger placeMananger;
    Transform linerenderersParrent;
public LineRenderer lineRenderer;

    private void Awake()
    {
        Initiate();
        Visualize();
    }
    void Initiate()
    {
        pathManager = GetComponentInChildren<PathManager>();
        placeMananger = GetComponentInChildren<PlaceMananger>();
        pathManager.Set();
        placeMananger.Set();
    }
    [ContextMenu("Debug")]
    public void Debug()
    {
        Initiate();
        Visualize();
    }
    public void Visualize()
    {
        if (linerenderersParrent != null)
        {
             DestroyImmediate(linerenderersParrent.gameObject);

        }
        linerenderersParrent= Instantiate(new GameObject("Lines"), linerenderersParrent).transform;
        foreach (var path in pathManager.paths)
        {
            var line = Instantiate(lineRenderer,linerenderersParrent);
            line.positionCount = path.transforms.Count;
            for (int i = 0;i<path.transforms.Count;i++)
            {
                line.SetPosition(i, path.transforms[i].position+Vector3.up);
               
            }
        }
    }
}
