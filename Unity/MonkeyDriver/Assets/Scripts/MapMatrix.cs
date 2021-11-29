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

    //map gen variables + data
    public int numStops;
    public int numObstacles;
    public Dictionary<Vector2,string> destinations;

    public float MAP_SCALAR = 1.0f; //altered for debugging/visualisation purposes

    //populates a 2d array we feed it with bus stops and obstacles.
    void GenerateMap(Intersection[,] mapMatrix, int numStops)
    {
        int matrixWidth = mapMatrix.GetLength(0);
        int matrixHeight = mapMatrix.GetLength(1);

        //populate map with intersections
        for (int i = 0; i < matrixWidth; i++)
        {
            for (int j = 0; j < matrixHeight; j++)
            {
                mapMatrix[i, j] = new Intersection();
            }
        }

        //populate map with stops
        for (int i = 0; i < numStops; i++)
        {
            int randX;
            int randY;
            int randColour;
            do
            {
                randX = Random.Range(0, matrixWidth);
                randY = Random.Range(0, matrixHeight);
                randColour = Random.Range(1,4);
            }
            while (mapMatrix[randX, randY].type != 0 && (randX + randY) != 0);

            mapMatrix[randX, randY].type = randColour;
            
        }

        //populate map with obstacles
        for (int i = 0; i < numObstacles; i++)
        {
            int randX;
            int randY;
            do
            {
                randX = Random.Range(0, matrixWidth);
                randY = Random.Range(0, matrixHeight);
            }
            while (mapMatrix[randX, randY].type != 0 && (randX+randY) != 0);

            mapMatrix[randX, randY].type = 5;
        }

        /*
         * DEPRECATED
        //populate map with X, Y many times
        void populateMap(int numToGenerate, int typeNum)
        {
            for (int i = 0; i < numToGenerate; i++)
            {
                int randX;
                int randY;
                do
                {
                    randX = Random.Range(0, matrixWidth);
                    randY = Random.Range(0, matrixHeight);
                }
                while (mapMatrix[randX, randY].type != 0);

                mapMatrix[randX, randY].type = typeNum;
            }
        }
        populateMap(numStops, 1);
        populateMap(numObstacles, 2);
        */


    }

    //instantiate everything in the right places
    void DrawMap(Intersection[,] mapMatrix)
    {
        int matrixWidth = mapMatrix.GetLength(0);
        int matrixHeight = mapMatrix.GetLength(1);

        float mapWidth = matrixWidth * MAP_SCALAR;
        float mapHeight = matrixHeight * MAP_SCALAR;

        for (int i = 0; i < matrixWidth; i++)
        {
            for (int j = 0; j < matrixHeight; j++)
            {
                //draw intersections
                Instantiate(intersection, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, 1), Quaternion.identity, generationLocation);
                switch (mapMatrix[i, j].type)
                {
                    case 0:
                        break;

                    case 1:
                        //creates a blue bus stop at [i,j] and adds it to the list of possible destinations

                        Instantiate(blueBusStop, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                        destinations.Add(new Vector2(i, j), "blue");
                        break;

                    case 2:
                        //creates a green bus stop at [i,j] and adds it to the list of possible destinations

                        Instantiate(greenBusStop, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                        destinations.Add(new Vector2(i, j), "green");
                        break;

                    case 3:
                        //creates an orange bus stop at [i,j] and adds it to the list of possible destinations

                        Instantiate(pinkBusStop, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                        destinations.Add(new Vector2(i, j), "pink");
                        break;

                    case 4:
                        //creates a purple bus stop at [i,j] and adds it to the list of possible destinations

                        Instantiate(redBusStop, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                        destinations.Add(new Vector2(i, j), "red");
                        break;

                    case 5:
                        Instantiate(obstacle, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                        break;

                    default:
                        break;
                }
            }
        }

        //center map at origin
        generationLocation.Translate(new Vector3(-(mapWidth - 1) /2, -(mapHeight - 1)/2, -6));
    }

    void Awake()
    {
        mapMatrix = new Intersection[width, height];
        //dictionary has stop coordinates, and a string colour
        //passengers get a colour and can be dropped off at that colour stop
        //have to generate stops of different colour still
        destinations = new Dictionary<Vector2, string>();

        GenerateMap(mapMatrix, numStops);
        DrawMap(mapMatrix);
    }

}
