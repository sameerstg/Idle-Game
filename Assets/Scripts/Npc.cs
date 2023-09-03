using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public void Move(PlaceName placeToGoName)
    {
        //if (placeToGoName == togoPlace.placeName)
        //{
        //    return;
        //}
        Debug.Log(placeToGoName);
        if (placeToGoName == PlaceName.None)
        {
            Debug.Log(placeToGoName+ " null");
            return;
        }
        togoPlace = PathManager._instance.GetPlace(placeToGoName,true);


        togoWaypoints.Clear();
        // for going relax to connected waypoint which is close to next place
        if (currentPoint.pointType != PointType.wayPoint)
        {
            var dest = currentPoint.pointConnection.connectedPoints.OrderBy(x=>Vector3.Distance(togoPlace.transform.position,x.transform.position))?.First();
            Debug.Log(dest);
            togoWaypoints.Add(dest);
            togoWaypoints.AddRange(PathManager._instance.GetPath(dest, togoPlace));

        }
        else
        {
            togoWaypoints.AddRange(PathManager._instance.GetPath(currentPoint, togoPlace));

        }

        StartCoroutine(MoveByTransforms(togoPlace.RelaxPointType != RelaxPointType.none));
    }

    public IEnumerator MoveByTransforms(bool checkForRelax = false)
    {
        if (togoWaypoints != null)
        {
            while (togoWaypoints.Count > 0)
            {
                if (togoWaypoints[0] != currentPoint)
                {
                    while (togoWaypoints[0].equipedNpc != null)
                    {

                        yield return null;
                    }
                    currentPoint.equipedNpc = null;
                    togoWaypoints[0].equipedNpc = this;
                    while (Vector3.Distance(transform.position, togoWaypoints[0].transform.position) > 0.1f)
                    {

                        transform.position = Vector3.MoveTowards(transform.position, togoWaypoints[0].transform.position, walkSpeed * Time.deltaTime);
                        yield return null;
                    }


                    currentPoint = togoWaypoints[0];
                }
               



                togoWaypoints.RemoveAt(0);


            }
            currentPoint.equipedNpc = null;

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

