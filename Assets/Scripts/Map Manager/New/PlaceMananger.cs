using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceMananger : MonoBehaviour
{
    public List<PlaceGo> places;
    public void Set()
    {

        places = GetComponentsInChildren<PlaceGo>().ToList();

    }
}
