using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RelaxWaypoint : MonoBehaviour
{
    public List<RelaxWaypoint> connectedRelaxWaypoints = new();
    [Header("no need to assign")]
    public List<RelaxPoint> relaxpoints = new();
    private void Awake()
    {
        Set();
    }
    public void Set()
    {
        relaxpoints = GetComponentsInChildren<RelaxPoint>().ToList();
    }
}
