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

    public PlaceGo placeToGo;
    public Transform currentTransform;
    public List<Transform> goingTransforms = new();
    [ContextMenu("Go")]
    public void GoToPlace()
    {
        StartCoroutine(Move());
       
    }
    public IEnumerator Move()
    {

        goingTransforms = WaypointSystem._instance.GetPathToPlace(placeToGo, currentTransform);
      
        while (goingTransforms.Count>0)
        {

            while(Vector3.Distance(transform.position,goingTransforms[0].position)>0.1f)
            {

               transform.position =  Vector3.MoveTowards(transform.position, goingTransforms[0].position,walkSpeed*Time.deltaTime);
                yield return null;
            }
            currentTransform = goingTransforms[0];
            goingTransforms.RemoveAt(0);

        }
        goingTransforms.Clear();
    }

}
