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
    public float busStopChance;
    public float obstacleChance;

    public Transform generationLocation;
    public GameObject intersection;
    public GameObject busStop;
    public GameObject obstacle;

    private const float MAP_SCALAR = 1.2f;

    //populates a 2d array we feed it with bus stops and obstacles.
    //NOTE TO SELF: should probably generate a number instead of linearly going through the array. this is not true random right now.
    void GenerateMap(Intersection[,] mapMatrix, int numStops)
    {
        int matrixWidth = mapMatrix.GetLength(0);
        int matrixHeight = mapMatrix.GetLength(1);
        int generatedStops = 0;
        int generatedObstacles = 0;

        while (generatedStops < numStops)
        {
            for (int i = 0; i < matrixWidth; i++)
            {
                for (int j = 0; j < matrixHeight; j++)
                {
                    mapMatrix[i, j] = new Intersection();
                    if (busStopChance > Random.Range(0.0f, 1.0f) && mapMatrix[i, j].type == 0 && generatedStops < numStops)
                    {
                        mapMatrix[i, j].type = 1;
                        generatedStops += 1;
                        Debug.Log("created stop at index: " + i + ", " + j + "!");
                    }
                    if (obstacleChance > Random.Range(0.0f, 1.0f) && mapMatrix[i, j].type == 0 && generatedObstacles < numObstacles)
                    {
                        mapMatrix[i, j].type = 2;
                        generatedObstacles += 1;
                        Debug.Log("created obstacle at index: " + i + ", " + j + "!");
                    }

                }
            }
        }
        Debug.Log("generated map");
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
                        Debug.Log("drew stop");
                        break;
                    case 2:
                        Instantiate(obstacle, new Vector3(i * MAP_SCALAR, j * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                        Debug.Log("drew obstacle");
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
    }

    // Start is called before the first frame update
    void Start()
    {
        //needs to be negative and position values
        GenerateMap(mapMatrix, numStops);
        DrawMap(mapMatrix);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
