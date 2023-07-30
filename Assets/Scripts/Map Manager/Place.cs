
using System.Collections.Generic;
using UnityEngine;

public class Place 
{
    public string name;
    public Transform point;
    public List<Transform> neighbouringWaypoints = new();
    public List<Place> neighbouringPlaces = new();
}
