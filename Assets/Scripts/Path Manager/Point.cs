using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Point : MonoBehaviour
{

    public PointConnection pointConnection;

    public PointType positionType;
    public Npc equipedNpc;
    public float waitTime;
    public virtual void DoWork(Npc npc)
    {
        equipedNpc = npc;
        npc.statemachine.SwitchState(new WaitState(npc));
        StartCoroutine(Wait());

    }
    public IEnumerator Wait()
    {
        Debug.Log($"waiting time = {waitTime}");
        yield return new WaitForSeconds(waitTime);
        equipedNpc.statemachine.SwitchState(new IdleState(equipedNpc));
        equipedNpc = null;
    }
}
//[CustomEditor(typeof(Point))]
//public class PointEditor :Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();
//        //DrawDefaultInspector();

//        //if (Application.isPlaying)
//        //{

//        //}
//    }
//}
public enum RelaxPointType
{
    none, relax, work
}
[System.Serializable]
public class PointConnection
{
    public List<Point> allPoints = new();
    [Header("Dont assign, Auto assign")]
    public List<Point> connectedPoints = new(), relaxPoints = new();
    /// <summary>
    /// For All Waypoints
    /// </summary>
    /// <param name="self"></param>
    public void OrganizeAllPoints(Point self, bool makeBi = false)
    {
        relaxPoints.Clear();
        connectedPoints.Clear();

        foreach (var point in allPoints)
        {
            AddPoint(self, point, makeBi);
        }

    }
    public void AddPoint(Point self, Point point, bool makeBi = false)
    {
        if (!allPoints.Contains(point))
        {
            allPoints.Add(point);
        }
        if (point.positionType == PointType.relaxPoint || point.positionType == PointType.workPoint)
        {
            if (!relaxPoints.Contains(point))
            {
                relaxPoints.Add(point);
            }
        }
        else if (point.positionType == PointType.wayPoint)
        {
            if (!connectedPoints.Contains(point))
            {
                connectedPoints.Add(point);
            }
           
        }
        // make 2 point bi directional
        if (makeBi && !point.pointConnection.allPoints.Contains(self))
        {
            point.pointConnection.AddPoint(point, self, makeBi);
        }
    }
    
    
}
[System.Serializable]
public class PointConnectionForPlace : PointConnection
{
    public List<Point> indirectAllPoints = new();
    [Header("Dont assign, Auto assign")]
    public List<Point> indirectConnectedPoints = new(), indirectRelaxPoints = new();
    /// <summary>
    /// For Place Organizer
    /// </summary>
    public void OrganizeAllPointsForPlace()
    {
        relaxPoints.Clear();
        connectedPoints.Clear();
        indirectAllPoints.Clear();
        indirectConnectedPoints.Clear();
        indirectRelaxPoints.Clear();
        foreach (var point in allPoints)
        {
            AddPointForPlace(point);
        }

    }
    /// <summary>
    /// for place only
    /// </summary>
    /// <param name="point"></param>
    public void AddPointForPlace(Point point)
    {

       
        if (point.positionType == PointType.wayPoint  && !connectedPoints.Contains(point))
        {
            connectedPoints.Add(point);
            CheckIndirectPointWithRelaxForPlace(point);
           
        }
        else if (point.positionType == PointType.relaxPoint  || point.positionType == PointType.workPoint)
        {
            if (!relaxPoints.Contains(point))
            {
                relaxPoints.Add(point);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="point">Parent</param>
    public void CheckIndirectPointWithRelaxForPlace(Point parrent)
    {
            

            // for checking if contains relaxpoints

            var pointWithRelax = parrent.pointConnection.allPoints.FindAll(x => x.positionType == PointType.workPoint || x.positionType == PointType.relaxPoint);
            
            if (pointWithRelax.Count>0)
            {
                if (!connectedPoints.Contains(parrent))
                {
                    // Adding in indirect connected bc it contains relax points
                    if (!indirectAllPoints.Contains(parrent))
                    {
                        indirectAllPoints.Add(parrent);
                    }
                    if (!indirectConnectedPoints.Contains(parrent))
                    {
                        indirectConnectedPoints.Add(parrent);
                    }
                }
                foreach (var item in pointWithRelax)
                {
                    AddIndirectForPlace(item);
                }
                
            }
            var connectedWaypointContainRelaxPoint = parrent.pointConnection.allPoints.FindAll(x => x.positionType == PointType.wayPoint && !connectedPoints.Contains(x) 
            && !indirectConnectedPoints.Contains(x) && x.pointConnection.allPoints.Exists(xx=>xx.positionType == PointType.workPoint || xx.positionType == PointType.relaxPoint) );
        foreach (var item in connectedWaypointContainRelaxPoint)
        {
            CheckIndirectPointWithRelaxForPlace(item);
        }


    }
    void AddIndirectForPlace(Point point)
    {
        if (point.positionType == PointType.relaxPoint || point.positionType == PointType.workPoint)
        {
            if (!indirectAllPoints.Contains(point))
            {
                indirectAllPoints.Add(point);
            }
            
            if (!indirectRelaxPoints.Contains(point))
            {
                indirectRelaxPoints.Add(point);
            }
        }
    }
}
