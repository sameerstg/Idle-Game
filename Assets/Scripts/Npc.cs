using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Npc : MonoBehaviour
{
    /// <summary>
    /// speed = unity units/seconds
    /// </summary>
    public float walkSpeed;
    public float runningSpeed;

    public PlaceGo placeToGo;
    private PlaceGo currentPlace;
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
        if (goingTransforms != null)
        {
            while (goingTransforms.Count > 0)
            {

                while (Vector3.Distance(transform.position, goingTransforms[0].position) > 0.1f)
                {

                    transform.position = Vector3.MoveTowards(transform.position, goingTransforms[0].position, walkSpeed * Time.deltaTime);
                    yield return null;
                }
                currentTransform = goingTransforms[0];
                goingTransforms.RemoveAt(0);

            }
            goingTransforms.Clear();
            placeToGo = null;
            currentPlace = placeToGo;
        }
       
    }

}
[CustomEditor(typeof(Npc))]
public class NpcEditor : Editor
{
    string[] _choices ;
    int _choiceIndex = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (Application.isPlaying)
        {
            Npc humnan = (Npc)target;

            _choices = WaypointSystem._instance.placeMananger.places.Select(x => x.name).ToArray();
            _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
            EditorUtility.SetDirty(target);
            if (GUILayout.Button("Move"))
            {
                humnan.placeToGo = WaypointSystem._instance.placeMananger.places[_choiceIndex];
                humnan.GoToPlace();
            }

        }
       

    }
}
