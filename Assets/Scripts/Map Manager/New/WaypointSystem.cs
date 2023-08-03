using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{

    public static WaypointSystem _instance;
    public PathManager pathManager;
    public PlaceManager placeMananger;
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
        placeMananger = GetComponentInChildren<PlaceManager>();
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
            line.name = path.gameObject.name;
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
        var _currentPath = GetAllPathByTransform(currentTransform)[0];
        if (_currentPath == null)
        {
            return transformsInOrder;

        }
        if (currentTransform == placeToGo.pointOfEntrance)
        {

            return transformsInOrder;            
        }
        if (_currentPath.connectedPlaces.Contains(placeToGo))
        {

            return GetTransformInOrderInPath(_currentPath, currentTransform, placeToGo.pointOfEntrance );
        }
        else
        {
            return GetPathToPlaceInTransforms(_currentPath, placeToGo, currentTransform);
            //foreach (var connectedPaths in currentPath.connectedPaths)
            //{
            //    foreach (var path in connectedPaths.paths)
            //    {
            //        if (!visitedPaths.Contains(path))
            //        {
            //            toBeVisited.Add(path);
            //        }
            //    }
            //}


            //do
            //{
            //    // listOfPotentialPath : adding list of path which may and may not have place where we want to go
            //    foreach (var listOfPotentialPath in listOfListOfPathToPlace)
            //    {
            //        // pathContainingPotentalConnectedPath : adding path  which may and may not have place where we want to go

            //        foreach (var pathContaininPotentalConnectedPath in listOfPotentialPath.Item1)
            //        {
            //            // connectedPaths : connected paths containing connected paths which may or maynot contain place
            //            foreach (var connectedPath in pathContaininPotentalConnectedPath.connectedPaths)
            //            {
            //                // path : we will check if we have or havent visited it and if it contains our place
            //                // if it does we will add in list
            //                foreach (var path in connectedPath.paths)
            //                {
            //                    if (!visitedPaths.Contains(path))
            //                    {
            //                        if (path.connectedPlaces.Contains(placeToGo))
            //                        {

            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //} while (true);

            return null;
        }
        
    }
    public List<Transform> GetPathToPlaceInTransforms(Path currentPath,PlaceGo place,Transform currentTransform)
    {
        List<Tuple<List<Path>, bool>> listOfListOfPathToPlace = new() { new Tuple<List<Path>, bool>(new List<Path>() { currentPath }, false) },toAdd= new(),toDelete=  new();
        List<Path> placePaths = GetAllPathContainingPlace(place);
        
        if (placePaths == null)
        {
            Debug.LogError("Place path not found");
            return null;
        }
        
        for(int i = 0;i< listOfListOfPathToPlace.Count;i++)
        {
            if (listOfListOfPathToPlace[i].Item2 )
            {
                continue;
            }
            else
            {
                foreach (var connectedPath in listOfListOfPathToPlace[i].Item1[^1].directlyConnectedPaths)
                {
                    foreach (var path in connectedPath.paths)
                    {
                        if (listOfListOfPathToPlace[i].Item1.Contains(path))
                        {
                            continue;
                        }
                        if (placePaths.Contains(path))
                        {

                            var paths = listOfListOfPathToPlace[i].Item1.ToList();
                            paths.Add(path);
                            listOfListOfPathToPlace.Add(new Tuple<List<Path>, bool>(paths, true));

                        }
                        else
                        {
                            var paths = listOfListOfPathToPlace[i].Item1.ToList();
                            paths.Add(path);
                            listOfListOfPathToPlace.Add(new Tuple<List<Path>, bool>(paths, false));

                        }
                    }
                }
                listOfListOfPathToPlace.Remove(listOfListOfPathToPlace[i]);

                i = -1;
            }
        }
        //foreach (var listOfPath in listOfListOfPathToPlace)
        //{
        //    if (!listOfPath.Item2)
        //    {
        //        Debug.Log("wrong entry");
        //        continue;
        //    }
        //    foreach (var item in listOfPath.Item1)
        //    {
        //        Debug.Log(item.gameObject.name);
        //    }
        //    Debug.Log(".....");
        //}
        Debug.Log(listOfListOfPathToPlace.Count);
        //listOfListOfPathToPlace.RemoveAll(x => !x.Item2);
        //Debug.Log(listOfListOfPathToPlace.Count);
        int ansI = 0;
        //for (int i = 1; i < listOfListOfPathToPlace.Count; i++)
        //{
        //    if (listOfListOfPathToPlace[i].Item1.Count< listOfListOfPathToPlace[ansI].Item1.Count)
        //    {
        //        ansI = i;
        //    }
        //}
        return GetTransformInOrderInPathsWithDistance(listOfListOfPathToPlace[ansI].Item1, place.pointOfEntrance, currentTransform).Item1;

    }
    
    public List<Path> GetAllPathContainingPlace(PlaceGo place)
    {
        return pathManager.paths.FindAll(x => x.connectedPlaces.Contains(place));
    }

    public Tuple<List<Transform>,float> GetTransformInOrderInPathsWithDistance(List<Path> pathsInOrder, Transform togoTransform, Transform _currentTransform)
    {
        Tuple<List<Transform>, float> transformInOrderInPathsWithDistance = new(new List<Transform>(),0);
        if (pathsInOrder.Count < 2)
        {
            Debug.LogError("not enought paths");
            return transformInOrderInPathsWithDistance;
        }
        if (!pathsInOrder[0].transforms.Contains(_currentTransform))
        {
            Debug.LogError("currentTransform is not correct");
            return transformInOrderInPathsWithDistance;

        }
        if (!pathsInOrder[^1].transforms.Contains(togoTransform))
        {
            Debug.LogError("togoTransform is not correct");
            return transformInOrderInPathsWithDistance;
        }
        List<Path> visitedPaths = new();
        var currentTransform = _currentTransform;
        for (int i = 1; i < pathsInOrder.Count; i++)
        {
            if (visitedPaths.Contains(pathsInOrder[i]))
            {
                return transformInOrderInPathsWithDistance;
            }
            visitedPaths.Add(pathsInOrder[i]);
            visitedPaths.Add(pathsInOrder[i - 1]);
            var commonTransformInTwoPath = pathManager.GetCommonTransform(new List<Path> { pathsInOrder[i-1], pathsInOrder[i] });
            transformInOrderInPathsWithDistance.Item1.AddRange(GetTransformInOrderInPath(pathsInOrder[i - 1], currentTransform, commonTransformInTwoPath));
          
            currentTransform = transformInOrderInPathsWithDistance.Item1[^1];
           
            if (i==pathsInOrder.Count-1)
            {
                transformInOrderInPathsWithDistance.Item1.AddRange(GetTransformInOrderInPath(pathsInOrder[i], commonTransformInTwoPath,togoTransform));
            }
        }
        return transformInOrderInPathsWithDistance;
    }
    public List<Transform> GetTransformInOrderInPath(Path path, Transform currentTransform,Transform togoTransform)
    {
        if (!path.transforms.Contains(togoTransform)|| !path.transforms.Contains(currentTransform))
        {
            Debug.Log("incorrect togo and current");
            return null;
        }
        if (togoTransform == currentTransform)
        {
            Debug.Log("incorrect togo and current");

            return null;
        }
        int currentIndex = path.transforms.IndexOf(currentTransform) ,togo = path.transforms.IndexOf(togoTransform);
      
        if (currentIndex< togo)
        {
           
            return path.transforms.GetRange(currentIndex + 1, togo - currentIndex);
            
        }
        else
        {
            var trans = path.transforms.GetRange(togo ,  currentIndex-togo);

            trans.Reverse();
            return trans;

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
