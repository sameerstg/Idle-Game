//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;

//public class PlaceManager : MonoBehaviour
//{
//    public List<PlaceGo> allPlaceGos = new List<PlaceGo>();
//    private void Start()
//    {
//        allPlaceGos = GetComponentsInChildren<PlaceGo>().ToList();
//        //SetNeighboursForAll();


//        //SetAllConnectedPlaces();
//    }
//    //public void SetNeighboursForAll()
//    //{
//    //    foreach (var place1 in allPlaceGos)
//    //    {
//    //        foreach (var place2 in allPlaceGos)
//    //        {

//    //            if (place1.place.name != place2.place.name && !place1.place.neighbouringPlaces.Contains(place2)
//    //                && !place2.place.neighbouringPlaces.Contains(place1))
//    //            {
//    //                if (GetCommonPath(place2.place, place1.place) != null)
//    //                {
//    //                    place1.place.neighbouringPlaces.Add(place2);
//    //                    place2.place.neighbouringPlaces.Add(place1);
//    //                }
                           
                    
//    //            }
//    //        }

//    //    }
//    //}
//    public Transform GetCommonPath(Place place1,Place place2)
//    {
//        foreach (var way in place1.neighbouringWaypoints)
//        {
//            if (place2.neighbouringWaypoints.Contains(way))
//            {
//                return way;
//            }
//        }
//        return null;
//    }
//    public void SetAllConnectedPlaces()
//    {
//        foreach (var place1 in allPlaceGos)
//        {
//            foreach (var place2 in allPlaceGos)
//            {


//                if (place1.place.name != place2.place.name &&
//                    !place1.place.connectionBetweenPlaces.Exists(x=>x.name==place2.place.name)&& 
//                    !place2.place.connectionBetweenPlaces.Exists(x => x.name == place1.place.name))
//                {
//                    ConnectionBetweenPlace connectionBetweenPlace = new ConnectionBetweenPlace(new System.Tuple<Place, Place>(place1.place, place2.place));

//                    ConnectionBetweenPlace connectionBetweenPlace1 = new(SolveConectionBetweePlace(connectionBetweenPlace));
//                    if (connectionBetweenPlace1 == null)
//                    {
//                        continue;
//                    }
//                    if (connectionBetweenPlace1.solved)
//                    {
//                        connectionBetweenPlace1.name = connectionBetweenPlace1.inBetweenPlaces[^1].name;
//                        place1.place.connectionBetweenPlaces.Add(connectionBetweenPlace1);

//                        ConnectionBetweenPlace connectionBetweenPlace2 = new ConnectionBetweenPlace(connectionBetweenPlace1);

//                        connectionBetweenPlace2.Reverse();
//                        connectionBetweenPlace2.name = connectionBetweenPlace2.inBetweenPlaces[^1].name;

//                        place2.place.connectionBetweenPlaces.Add(connectionBetweenPlace2);





//                    }
//                }
//            }

//        }
//    }
  
//    public ConnectionBetweenPlace SolveConectionBetweePlace(ConnectionBetweenPlace connectionBetweenPlace)
//    {
        

//        List<ConnectionBetweenPlace> connectionBetweenPlaceList = new();
//        ConnectionBetweenPlace connectionBetweenPlaceCopy = new(connectionBetweenPlace);
       
//        foreach (var item in connectionBetweenPlaceCopy.inBetweenPlaces[^1].neighbouringPlaces)
//        {
//            ConnectionBetweenPlace newConnection = new(connectionBetweenPlace);
          
//            if (connectionBetweenPlaceCopy.inBetweenPlaces.Contains(item.place))
//            {
//                continue;
//            }
//            else if (item.name == connectionBetweenPlaceCopy.tail.name)
//            {

//                newConnection.solved = true;
//                newConnection.distance += Vector3.Distance(item.place.point.position, newConnection.inBetweenPlaces[^1].point.position);

//                newConnection.inBetweenPlaces.Add(item.place);
//                newConnection.inBetweenPlacesName.Add(item.place.name);
                
//                connectionBetweenPlaceList.Add(newConnection);
//                continue;
             
//            }
//            else 
//            {
//                newConnection.distance += Vector3.Distance(item.place.point.position, newConnection.inBetweenPlaces[^1].point.position);

//                newConnection.inBetweenPlaces.Add(item.place);
//                newConnection.inBetweenPlacesName.Add(item.place.name);

//                newConnection = SolveConectionBetweePlace(newConnection);
//                if (newConnection == null)
//                {
                    
//                }
//                else if(newConnection.solved)
//                {

                    
//                    connectionBetweenPlaceList.Add(newConnection);
//                }
              

//            }
//        }



//        float dist = 999999;
//        ConnectionBetweenPlace shortestConnection = null;
//        foreach (var item in connectionBetweenPlaceList)
//        {
          
//            if (item.distance < dist && item.solved)
//            {
//                shortestConnection =new(item);
//                dist = item.distance;
//            }
//        }

//        //if (shortestConnection != null && shortestConnection.solved)
//        //{
//        //    Debug.LogError(shortestConnection.distance);
//        //    foreach (var i in shortestConnection.inBetweenPlaces)
//        //    {
//        //        Debug.LogError(i.name);
//        //    }
//        //}

//        return shortestConnection;

//    }

//}
//public class PlacesGraph
//{
//    public List<Place> nodes = new();


//    public List<PlacesEdge> edges = new();

//    public void AddEdge(PlacesEdge edge)
//    {
//        edges.Add(edge);
//        if (!nodes.Contains(edge.places[0]))
//        {
//            nodes.Add(edge.places[0]);


//        }
//        if (!nodes.Contains(edge.places[1]))
//        {
//            nodes.Add(edge.places[1]);
//        }

//    }
//    void Weight()
//    {

//    }
//}
//public class PlacesEdge
//{
//    public List<Place> places;
//    public PlacesEdge(Place place1, Place place2)
//    {
//        places = new List<Place> { place1, place2 };
//    }
//}