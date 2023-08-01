using System.Collections.Generic;
using UnityEngine;

public class PlaceGo : MonoBehaviour
{


    public Transform pointOfEntrance;
    public Place place;
    private void Awake()
    {

        place.name = gameObject.name;
        place.point = transform;
    }


}
[System.Serializable]
public class Place
{

    
    public string name;
    public Transform point;
    public List<Transform> neighbouringWaypoints = new();
    public List<PlaceGo> neighbouringPlaces = new();
    public List<ConnectionBetweenPlace> connectionBetweenPlaces = new() ;
}