using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    public List<Place> places = new();
    
    public void Set()
    {
        places = GetComponentsInChildren<Place>().ToList();
    }
    
}
