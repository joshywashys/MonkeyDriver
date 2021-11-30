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
    //Unity objects
    public Transform generationLocation;
    public GameObject intersection;
    public GameObject blueBusStop, greenBusStop, pinkBusStop, redBusStop;
    public GameObject obstacle;

    //map properties
    public Intersection[,] mapMatrix;
    public int height;
    public int width;

    public List<Intersection> intersectionList = new List<Intersection>();
    public List<Intersection> obstacleList = new List<Intersection>();
    public Dictionary<Intersection, GameObject> stopDict = new Dictionary<Intersection, GameObject>();

    //region (PCG) properties
    private List<Vector2Int> regionPos = new List<Vector2Int>();
    public int minRegionSize;
    public int maxRegionSize;
    public int numRegions;

    //map gen variables + data
    public int numStops;
    public int numObstacles;
    public Dictionary<Vector2,string> destinations;

    public int MAP_SCALAR = 1; //altered for debugging/visualisation purposes

    //populates a 2d array we feed it with bus stops and obstacles.
    void GenerateMap(Intersection[,] mapMatrix, int numStops)
    {
        int matrixWidth = mapMatrix.GetLength(0);
        int matrixHeight = mapMatrix.GetLength(1);

        //generate regions
        Vector2Int currRegionPos = new Vector2Int(0, 0);
        for (int i = 0; i < numRegions; i++)
        {
            regionPos.Add(currRegionPos);
            int regionWidth = Random.Range(minRegionSize, maxRegionSize);
            int regionHeight = Random.Range(minRegionSize, maxRegionSize);

            for (int j = currRegionPos.x; j < regionWidth; j++)
            {
                for (int k = currRegionPos.y; k < regionHeight; k++)
                {
                    //Intersection newIntersection = new Intersection(j, k);
                    //mapMatrix[j, k] = newIntersection;
                    //intersectionList.Add(newIntersection);
                }
            }

        }

        //generate bridges between regions

        
        //populate map with intersections
        for (int i = 0; i < matrixWidth; i++)
        {
            for (int j = 0; j < matrixHeight; j++)
            {
                Intersection newIntersection = new Intersection(i, j);
                mapMatrix[i, j] = newIntersection;
                intersectionList.Add(newIntersection);
            }
        }
        

        //register intersection bounds
        for (int i = 0; i < matrixWidth; i++)
        {
            for (int j = 0; j < matrixHeight; j++)
            {
                if (mapMatrix[i,j] != null)
                {
                    //print(i + ", " + j);

                    //up
                    if (j < mapMatrix.GetLength(1) - 1)
                    {
                        if (mapMatrix[i, j + 1] == null)
                        {
                            mapMatrix[i, j].setBoundUp(true);
                        }
                    }
                    else
                    {
                        mapMatrix[i, j].setBoundUp(true);
                    }

                    //right
                    if (i < mapMatrix.GetLength(0) - 1)
                    {
                        if (mapMatrix[i + 1, j] == null)
                        {
                            mapMatrix[i, j].setBoundRight(true);
                        }
                    }
                    else
                    {
                        mapMatrix[i, j].setBoundRight(true);
                    }

                    //down
                    if (j > 0)
                    {
                        if (mapMatrix[i, j - 1] == null)
                        {
                            mapMatrix[i, j].setBoundDown(true);
                        }
                    }
                    else
                    {
                        mapMatrix[i, j].setBoundDown(true);
                    }

                    //left
                    if (i > 0)
                    {
                        if (mapMatrix[i - 1, j] == null)
                        {
                            mapMatrix[i, j].setBoundLeft(true);
                        }
                    }
                    else
                    {
                        mapMatrix[i, j].setBoundLeft(true);
                    }
                }
            }
        }

        //populate map with stops
        for (int i = 0; i < numStops; i++)
        {
            int randVal;
            int randColour;
            do
            {
                randVal = Random.Range(0, intersectionList.Count);
                randColour = Random.Range(1,5);
            }
            while (intersectionList[randVal].type != 0);

            intersectionList[randVal].type = randColour;

        }

        //populate map with obstacles
        for (int i = 0; i < numObstacles; i++)
        {
            int randVal;
            do
            {
                randVal = Random.Range(0, intersectionList.Count);
            }
            while (intersectionList[randVal].type != 0);

            intersectionList[randVal].type = 5;
            obstacleList.Add(intersectionList[randVal]);
        }

    }

    //instantiate everything in the right places
    void DrawMap(Intersection[,] mapMatrix)
    {
        for (int i = 0; i < intersectionList.Count; i++)
        {
            //for (int j = 0; j < matrixHeight; j++)
            //{
            //draw intersections
            Vector2Int pos = intersectionList[i].getPos();
            int x = pos.x;
            int y = pos.y;

            //print(i);
            //print(x + ", " + y);

            Instantiate(intersection, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, 1), Quaternion.identity, generationLocation);

            switch (intersectionList[i].type)
            {
                case 0:
                    break;

                case 1:
                    //creates a blue bus stop at [i,j] and adds it to the list of possible destinations

                    stopDict.Add(intersectionList[i], Instantiate(blueBusStop, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, -1), Quaternion.identity, generationLocation));
                    destinations.Add(pos, "blue");
                    break;

                case 2:
                    //creates a green bus stop at [i,j] and adds it to the list of possible destinations

                    stopDict.Add(intersectionList[i], Instantiate(greenBusStop, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, -1), Quaternion.identity, generationLocation));
                    destinations.Add(pos, "green");
                    break;

                case 3:
                    //creates an orange bus stop at [i,j] and adds it to the list of possible destinations

                    stopDict.Add(intersectionList[i], Instantiate(pinkBusStop, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, -1), Quaternion.identity, generationLocation));
                    destinations.Add(pos, "pink");
                    break;

                case 4:
                    //creates a purple bus stop at [i,j] and adds it to the list of possible destinations

                    stopDict.Add(intersectionList[i], Instantiate(redBusStop, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, -1), Quaternion.identity, generationLocation));
                    destinations.Add(pos, "red");
                    break;

                case 5:
                    Instantiate(obstacle, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                    break;

                default:
                    break;
            }

        }

        //center map at origin
        int matrixWidth = mapMatrix.GetLength(0);
        int matrixHeight = mapMatrix.GetLength(1);
        float mapWidth = matrixWidth * MAP_SCALAR;
        float mapHeight = matrixHeight * MAP_SCALAR;
        generationLocation.Translate(new Vector3(-(mapWidth - 1) /2, -(mapHeight - 1)/2, -6));
    }

    void Awake()
    {
        mapMatrix = new Intersection[width, height];
        //intersectionList = new List<Intersection>();
        //dictionary has stop coordinates, and a string colour
        //passengers get a colour and can be dropped off at that colour stop
        //have to generate stops of different colour still
        destinations = new Dictionary<Vector2, string>();

        GenerateMap(mapMatrix, numStops);
        DrawMap(mapMatrix);
    }

}
