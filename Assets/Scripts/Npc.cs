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
    public Transform currentTransform;
    public Transform currenRelaxWaypoint;
    public List<Waypoint> togoWaypoints = new();

    public List<Transform> transformsTogo =new();

    public Statemachine statemachine;
    public PositionType destinationType,positionType;
    private void Awake()
    {
        statemachine = new(this);
    }
    public void Move(Place placeToGo)
    {
        togoPlace = placeToGo;
        transformsTogo = PathManager._instance.GetPath(currentTransform.GetComponent<Waypoint>(), placeToGo).Select(x=>x.transform).ToList();
        destinationType = PositionType.place;
        StartCoroutine(MoveByTransforms());
    }

    public void MoveToRelaxPoint()
    {
        if (togoPlace != null && togoPlace.HaveEmptyRelaxPoint())
        {
            
            if (togoPlace.RelaxPointType == RelaxPointType.relax)
            {

                var pathWithRelaxWaypoint = togoPlace.GetPathToRelaxPoint();
                if (pathWithRelaxWaypoint.Item2 != null)
                {
                    destinationType = PositionType.relaxPoint;
                    transformsTogo = pathWithRelaxWaypoint.Item1;
                    StartCoroutine(MoveByTransforms());
                }
            }
            else if (togoPlace.RelaxPointType == RelaxPointType.work)
            {
                destinationType = PositionType.workPoint;

                var pathWithRelaxWaypoint = togoPlace.GetPathToWorkRelaxPoint();
                transformsTogo = pathWithRelaxWaypoint.Select(x => x.transform).ToList();
                StartCoroutine(MoveByTransforms());
            }

        }
        else
        {
            statemachine.SwitchState(new IdleState(this));
        }
    }


    public IEnumerator MoveByTransforms()
    {
        if (transformsTogo != null)
        {
            while (transformsTogo.Count > 0)
            {

                while (Vector3.Distance(transform.position, transformsTogo[0].position) > 0.1f)
                {

                    transform.position = Vector3.MoveTowards(transform.position, transformsTogo[0].transform.position, walkSpeed * Time.deltaTime);
                    yield return null;
                }


                if (destinationType == PositionType.place || destinationType == PositionType.wayPoint)
                {
                    currentTransform = transformsTogo[0];
                    positionType = PositionType.wayPoint;
                }
                else
                {
                    currenRelaxWaypoint = transformsTogo[0];
                    positionType = PositionType.relaxPoint;

                }

                transformsTogo.RemoveAt(0);

            }
            transformsTogo.Clear();
        }
            

        if (destinationType == PositionType.place)
        {
            destinationType = PositionType.none;
            positionType = destinationType;
            MoveToRelaxPoint();
        }
        else if (destinationType == PositionType.workPoint || destinationType == PositionType.relaxPoint)
        {
            destinationType = PositionType.none;
            positionType = destinationType;
            currenRelaxWaypoint.GetComponent<RelaxPoint>().DoWork(this);
        }
      



        //if (workPlace)
        //{

        //    currentTransform.gameObject.GetComponent<RelaxPoint>().DoWork(this);
        //}
        //else
        //{
        //    statemachine.SwitchState(new IdleState(this));
        //}

    }
    //public IEnumerator MoveByRelaxPoint(List<RelaxPoint> relaxPoints)
    //{
    //    RelaxPoint prev = null;
    //    if (relaxPoints != null)
    //    {
    //        while (relaxPoints.Count > 0)
    //        {
    //            yield return new WaitUntil(()=>relaxPoints[0].equipedNpc != null);
    //            if (prev != null)
    //            {
    //                prev.equipedNpc = null;
    //            }
    //            while (Vector3.Distance(transform.position, relaxPoints[0].transform.position) > 0.1f)
    //            {

    //                transform.position = Vector3.MoveTowards(transform.position, relaxPoints[0].transform.position, walkSpeed * Time.deltaTime);
    //                yield return null;
    //            }
    //            relaxPoints[0].equipedNpc = this;
    //            prev = relaxPoints[0];
    //            if (relaxPoints.Count == 1)
    //            {
    //                relaxPoints[0].DoWork(this);
    //            }

    //            currentTransform = relaxPoints[0].transform;
    //            relaxPoints.RemoveAt(0);

    //        }
    //        relaxPoints.Clear();
    //    }
    //    //currentTransform.gameObject.GetComponent<RelaxPoint>().DoWork(this);

    //}
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

