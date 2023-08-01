using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{

    public static WaypointSystem _instance;
    public PathManager pathManager;
    public PlaceMananger placeMananger;
    Transform linerenderersParrent;
public LineRenderer lineRenderer;

    private void Awake()
    {
        _instance = this;
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
    public void Show()
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
        linerenderersParrent= new GameObject("Line renderers").transform;
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
    public List<Transform> GetPathToPlace(PlaceGo placeToGo,Transform currentTransform)
    {
        List<Transform> transformsInOrder = new();
        var currentPath = pathManager.paths.Find(x => x.transforms.Find(x => x.transform == currentTransform));
        if (currentPath == null)
        {
            return transformsInOrder;

        }
        if (currentTransform == placeToGo.pointOfEntrance)
        {

            return transformsInOrder;            
        }
        if (currentPath.connectedPlaces.Contains(placeToGo))
        {

            return GetTransformInOrder(currentPath.transforms, placeToGo.pointOfEntrance, currentTransform);
        }
        else
        {
            //do
            //{

            //} while (true);

            return null;
        }
        
    }
    public List<Transform> GetTransformInOrder(List<Transform> transforms,Transform togoTransform, Transform currentTransform)
    {
        if (!transforms.Contains(togoTransform)|| !transforms.Contains(currentTransform))
        {
            return null;
        }
        if (togoTransform == currentTransform)
        {
            return null;
        }
        int currentIndex = transforms.IndexOf(currentTransform) ,togo = transforms.IndexOf(togoTransform);
      
        if (currentIndex< togo)
        {
           
            return transforms.GetRange(currentIndex + 1, togo - currentIndex);
            
        }
        else
        {
            List<Transform> trans = new();
            trans.AddRange(transforms);
            trans.Reverse();
            currentIndex = trans.IndexOf(currentTransform); togo = trans.IndexOf(togoTransform);
           
            return trans.GetRange(currentIndex+1 , togo - currentIndex );

        }

    }
}
