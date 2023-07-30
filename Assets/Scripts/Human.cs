using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    /// <summary>
    /// speed = unity units/seconds
    /// </summary>
    public float walkSpeed;
    public float runningSpeed;
    
    //public int index;
    //[ContextMenu("Go")]
    //public void SetDestination()
    //{
    //    StartCoroutine(SetDestinationCoroutine());
    //}
    //public IEnumerator SetDestinationCoroutine()
    //{
    //    float tempTime = 0,timeToReach = Vector3.Distance(transform.position, WaypointsManager._instance.waypoints[index].position)/walkSpeed;
    //    Debug.Log(Time.time);
    //    Debug.Log(timeToReach);
    //    while(tempTime < timeToReach)
    //    {
    //        transform.position = Vector3.Lerp(transform.position,WaypointsManager._instance.waypoints[index].position,tempTime/ timeToReach);
    //        tempTime += Time.deltaTime;
    //        yield return null;
            
    //    }
    //    Debug.Log(Time.time);

    //}

}
