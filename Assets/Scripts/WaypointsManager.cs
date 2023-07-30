using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsManager : MonoBehaviour
{

    public static WaypointsManager _instance;
    public  List<Transform> waypoints;


    private void Awake()
    {

        _instance = this;
    }

}
