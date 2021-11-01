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
    Intersection[,] mapMatrix;
    public int height;
    public int width;

    public int numStops;
    public int numObstacles;
    public Vector2[] stopCoordinates;

    public Transform generationLocation;
    public GameObject intersection;
    public GameObject busStop;
    public GameObject obstacle;

    public const float MAP_SCALAR = 1.0f; //mainly used for debugging/visualisation purposes

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
            do
            {
                randX = Random.Range(0, matrixWidth);
                randY = Random.Range(0, matrixHeight);
            }
            while (mapMatrix[randX, randY].type != 0);

            mapMatrix[randX, randY].type = 1;
            stopCoordinates[i] = new Vector2(randX, randY);
        }

        //populate map with stops
        for (int i = 0; i < numObstacles; i++)
        {
            int randX;
            int randY;
            do
            {
                randX = Random.Range(0, matrixWidth);
                randY = Random.Range(0, matrixHeight);
            }
            while (mapMatrix[randX, randY].type != 0);

            mapMatrix[randX, randY].type = 2;
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
                Instantiate(intersection, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, 0), Quaternion.identity, generationLocation);
                switch (mapMatrix[i, j].type)
                {
                    case 0:
                        break;
                    case 1:
                        Instantiate(busStop, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                        break;
                    case 2:
                        Instantiate(obstacle, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                        break;
                }
            }
        }

        //center map at origin
        generationLocation.Translate(new Vector3(-(mapWidth - 1) /2, -(mapHeight - 1)/2, 0));

    }

    void Awake()
    {
        mapMatrix = new Intersection[width, height];
        stopCoordinates = new Vector2[numStops];
    }

    // Start is called before the first frame update
    void Start()
    {
        //needs to be negative and position values
        GenerateMap(mapMatrix, numStops);
        DrawMap(mapMatrix);
        for (int i = 0; i < stopCoordinates.Length; i++)
        {
            Debug.Log(stopCoordinates[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
