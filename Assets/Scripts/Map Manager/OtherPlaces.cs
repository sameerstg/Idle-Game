using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class ConnectionBetweenPlace 
{
    public string name;
    public Place head, tail;
    internal List<Place> inBetweenPlaces = new();
    public List<string> inBetweenPlacesName = new();
    public float distance;
    public bool solved;
    
    public ConnectionBetweenPlace(Tuple<Place, Place> placeHeadAndTail)
    {
        head = placeHeadAndTail.Item1;
        tail = placeHeadAndTail.Item2;
        name = placeHeadAndTail.Item2.name;
        inBetweenPlaces = new();
        inBetweenPlaces.Add(head);
    }
    public ConnectionBetweenPlace(ConnectionBetweenPlace connection)
    {
        head = connection.head;
        tail = connection.tail;
        inBetweenPlaces = new();
        inBetweenPlacesName = new();
        foreach (var item in connection.inBetweenPlaces)
        {
            inBetweenPlaces.Add(item);
            inBetweenPlacesName.Add(item.name);
        }
        distance = connection.distance;
        solved = connection.solved;
    }

    public void Reverse()
    {

        var temp =head;
        head = tail;
        tail = temp;
        inBetweenPlaces.Reverse();
        inBetweenPlacesName.Reverse();

    }
}
