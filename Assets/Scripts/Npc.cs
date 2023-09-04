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
    private void Start()
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

        Debug.Log(togoPlace);
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


                if (currentPoint != togoWaypoints[0])
                {
                    if (togoWaypoints[0].pointType == PointType.workPoint)
                    //if (togoWaypoints[0].pointType == PointType.workPoint || togoWaypoints[0].pointType == PointType.relaxPoint)
                    {
                        while (togoWaypoints[0].equipedNpc != null && togoWaypoints[0].equipedNpc != this)
                        {
                            Debug.Log("sag");
                            yield return null;
                        }
                            togoWaypoints[0].equipedNpc = this;
                                                
                    }
                    currentPoint.equipedNpc = null;



                    while (Vector3.Distance(transform.position, togoWaypoints[0].transform.position) > 0.1f)
                    {

                        transform.position = Vector3.MoveTowards(transform.position, togoWaypoints[0].transform.position, walkSpeed * Time.deltaTime);
                        yield return null;
                    }
                    currentPoint = togoWaypoints[0];
                    
                }
                togoWaypoints.RemoveAt(0);
            }
            togoWaypoints.Clear();
        }


        if (checkForRelax && togoPlace.HaveEmptyRelaxPoint() )
        {

            togoWaypoints.AddRange( togoPlace.GetPathFromPlaceToRelax(togoPlace,currentPoint));
            StartCoroutine(MoveByTransforms());
        }
        else if (currentPoint.pointType == PointType.workPoint && currentPoint.equipedNpc == this)
        {
            currentPoint.DoWork(this);

        }
        else
        {
            statemachine.SwitchState(new IdleState(this));
        }



    }

}
[System.Serializable]
public class Attributes
{

    public int comfort;
    public int hunger;
    public int sanity;
    public int hapiness;
    public int health;
    public int security;

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

