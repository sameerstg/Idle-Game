using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        var currentPath = GetAllPathByTransform(currentTransform)[0];
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

            return GetTransformInOrder(currentPath, placeToGo.pointOfEntrance, currentTransform);
        }
        else
        {
            
            //do
            //{

            //} while (true);

            return null;
        }
        
    }
    public List<Transform> GetTransformInOrder(Path path,Transform togoTransform, Transform currentTransform)
    {
        if (!path.transforms.Contains(togoTransform)|| !path.transforms.Contains(currentTransform))
        {
            return null;
        }
        if (togoTransform == currentTransform)
        {
            return null;
        }
        int currentIndex = path.transforms.IndexOf(currentTransform) ,togo = path.transforms.IndexOf(togoTransform);
      
        if (currentIndex< togo)
        {
           
            return path.transforms.GetRange(currentIndex + 1, togo - currentIndex);
            
        }
        else
        {
            List<Transform> trans = new();
            trans.AddRange(path.transforms);
            trans.Reverse();
            currentIndex = trans.IndexOf(currentTransform); togo = trans.IndexOf(togoTransform);
           
            return trans.GetRange(currentIndex+1 , togo - currentIndex );

        }

    }
    List<Path> GetAllPathByTransform(Transform transform)
    {

        return pathManager.paths.FindAll(x => x.transforms.Find(x => x.transform == transform));
    }
}
[CustomEditor(typeof(WaypointSystem))]
public class WaypointSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WaypointSystem myScript = (WaypointSystem)target;
        if (GUILayout.Button("Show Lines"))
        {
            myScript.Show();
        }
    }
}
