using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public float walkSpeed,runSpeed;
    public Place togoPlace;
    public Point currentPoint;
    public List<Point> togoWaypoints = new();


    public Statemachine statemachine;
    private void Awake()
    {
        statemachine = new(this);
    }
    public void Move(Place placeToGo)
    {
        togoPlace = placeToGo;
        togoWaypoints.Clear();
        // for going relax to connected waypoint which is close to next place
        if (currentPoint.pointType != PointType.wayPoint)
        {
            var dest = currentPoint.pointConnection.connectedPoints.OrderBy(x=>Vector3.Distance(placeToGo.transform.position,x.transform.position))?.First();
            togoWaypoints.AddRange(new List<Point> {dest});
            currentPoint = dest;
        }
        togoWaypoints.AddRange( PathManager._instance.GetPath(currentPoint, placeToGo));
        StartCoroutine(MoveByTransforms(placeToGo.RelaxPointType != RelaxPointType.none));
    }

    //public void MoveToRelaxPoint()
    //{
    //    if (togoPlace != null && togoPlace.HaveEmptyRelaxPoint())
    //    {

    //        if (togoPlace.RelaxPointType == RelaxPointType.relax)
    //        {

    //            //var pathWithRelaxWaypoint = togoPlace.GetPathToRelaxPoint();
    //            //if (pathWithRelaxWaypoint.Item2 != null)
    //            //{
    //            //    destinationType = PointType.relaxPoint;
    //            //    transformsTogo = pathWithRelaxWaypoint.Item1;
    //            //    StartCoroutine(MoveByTransforms());
    //            //}
    //        }
    //        else if (togoPlace.RelaxPointType == RelaxPointType.work)
    //        {
    //            //destinationType = PointType.workPoint;

    //            //var pathWithRelaxWaypoint = togoPlace.GetPathToWorkRelaxPoint();
    //            //transformsTogo = pathWithRelaxWaypoint.Select(x => x.transform).ToList();
    //            //StartCoroutine(MoveByTransforms());
    //        }


    //        //var pathWithRelaxWaypoint = togoPlace.GetPathToWorkRelaxPoint();
    //        //transformsTogo = pathWithRelaxWaypoint.Select(x => x.transform).ToList();
    //        //StartCoroutine(MoveByTransforms());
    //    }
    //    else
    //    {
    //        statemachine.SwitchState(new IdleState(this));
    //    }
    //}


    public IEnumerator MoveByTransforms(bool checkForRelax = false)
    {
        if (togoWaypoints != null)
        {
            while (togoWaypoints.Count > 0)
            {

                while (Vector3.Distance(transform.position, togoWaypoints[0].transform.position) > 0.1f)
                {
                    while (togoWaypoints[0].equipedNpc != null)
                    {
                        yield return null;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, togoWaypoints[0].transform.position, walkSpeed * Time.deltaTime);
                    yield return null;
                }


                currentPoint = togoWaypoints[0];


                togoWaypoints.RemoveAt(0);

            }
            togoWaypoints.Clear();
        }

        if (checkForRelax && togoPlace.HaveEmptyRelaxPoint() )
        {

            togoWaypoints.AddRange( togoPlace.GetPathFromPlaceToRelax(togoPlace,currentPoint));
            StartCoroutine(MoveByTransforms());
        }
        else if (currentPoint.pointType == PointType.workPoint)
        {
            currentPoint.DoWork(this);

        }
        else
        {
            statemachine.SwitchState(new IdleState(this));
        }

      

    }
  
}
//[CustomEditor(typeof(Npc))]
//public class NpcEditor : Editor {



    
//    string[] places;
//    int placeIndex = 0;
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
     

//        //if ( Application.isPlaying)
//        //{
           
//        //    Npc npc = (Npc)target;
            
//        //        places = PathManager._instance.placeManager.places.Select(x => x.name).ToArray();

//        //        placeIndex = EditorGUILayout.Popup(placeIndex, places);
//        //        npc.togoPlace = PathManager._instance.placeManager.places[placeIndex];


//        //    EditorUtility.SetDirty(target);
//        //    if (GUILayout.Button("Move"))
//        //    {
                

              
//        //        npc.Move();
//        //    }

//        //    //if (GUILayout.Button("Go Relax"))
//        //    //{
//        //    //    npc.MoveRelax();
//        //    //}
//        //    //if (GUILayout.Button(" Relax To Place"))
//        //    //{
//        //    //    npc.RelaxToPlace();
//        //    //}
//        //}




//    }
//}

