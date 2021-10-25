using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script is responsible for:
-generate a simple matrix map
-generating the map (further tech goal)
-generate stops and any other locations
-keep track of entity locations (bus)
*/
public class MapMatrix : MonoBehaviour
{
    GameObject[,] mapMatrix;

    void generateMap(int height,int width)
    {
        mapMatrix = new GameObject[height, width];
    }

    //once we have sprites and etc this is how it will visualize
    void drawMap()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //generateMap(5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
