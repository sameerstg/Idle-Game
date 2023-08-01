using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public List<Path> paths;
    public void Set()
    {


        paths = GetComponentsInChildren<Path>().ToList();
       
    }
}

