using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script is responsible for:
-generate a 2D matrix map
-generating the map (further tech goal)
-generate stops and any other locations
-keep track of entity locations
*/

public class MapMatrix : MonoBehaviour
{
    //Unity objects
    public Transform generationLocation;
    public GameObject intersection2;
    public GameObject debugSprite;
    public GameObject intersection, intsectStraight, intsectT, intsectL;
    public GameObject intersectionGround;
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
    struct Region
    {
        public Vector2Int pos;
        public int width;
        public int height;
        public List<Intersection> intersections;

        public Region(int x, int y, int width, int height)
        {
            pos = new Vector2Int(x,y);
            this.width = width;
            this.height = height;
            intersections = new List<Intersection>();
        }

        public Region(Vector2Int xy, int width, int height)
        {
            pos = xy;
            this.width = width;
            this.height = height;
            intersections = new List<Intersection>();
        }
    }

    private List<Region> regionList = new List<Region>();
    public int minRegionSize;
    public int maxRegionSize;
    public float regionChance;

    //map gen variables + data
    public int numStops;
    public int numObstacles;

    public Vector2 stopOffset;
    public float buildingOffset;
    public float randombuildingOffset;

    public List<GameObject> foliageList;
    public List<GameObject> buildingsList;
    public List<GameObject> buildingsList2;
    public float rareRegionChance;

    public int MAP_SCALAR = 1; //alter only for debugging/visualisation purposes
    

    //populates a 2d array we feed it with bus stops and obstacles.
    void GenerateMap(Intersection[,] mapMatrix, int numStops)
    {
        int matrixWidth = mapMatrix.GetLength(0);
        int matrixHeight = mapMatrix.GetLength(1);

        //generate a region
        void generateRegion(int startX, int startY, int maxWidth)
        {
            int regionWidth = Random.Range(minRegionSize, maxRegionSize);
            int regionHeight = Random.Range(minRegionSize, maxRegionSize);

            for (int i = startX; i < startX + regionWidth; i++)
            {
                for (int j = startY; j < startY + regionHeight; j++)
                {
                    //print(i + ", " + j);
                    Intersection newIntersection = new Intersection(i, j);
                    mapMatrix[i, j] = newIntersection;
                    intersectionList.Add(newIntersection);
                }
            }

            regionList.Add(new Region(startX, startY, regionWidth, regionHeight));
        }

        //cycle through all tiles (i & j swapped for testing)
        for (int j = 0; j < matrixWidth - minRegionSize; j++)
        {
            for (int i = 0; i < matrixHeight - minRegionSize - 1; i++)
            {
                //if tile doesn't have an intersection
                if (mapMatrix[i, j] == null && Random.Range(0f,1f) < regionChance)
                {
                    bool hasSpace = true;
                    int availableSpace = 1;
                    while (hasSpace && availableSpace < maxRegionSize)
                    {
                        if (i + availableSpace < mapMatrix.GetLength(0) - 1)
                        {
                            if (mapMatrix[i + availableSpace, j] == null)
                            {
                                availableSpace += 1;
                            }
                            else
                            {
                                hasSpace = false;
                            }

                        }
                        else
                        {
                            hasSpace = false;
                        }

                    }
                    if (availableSpace > minRegionSize && j < mapMatrix.GetLength(1) - minRegionSize - 1)
                    {
                        generateRegion(i, j, availableSpace);
                    }
                    
                }

            }

        }

        //register intersection bounds
        for (int i = 0; i < matrixWidth; i++)
        {
            for (int j = 0; j < matrixHeight; j++)
            {
                if (mapMatrix[i,j] != null)
                {
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
            int colVal = (i % 4) + 1;
            do
            {
                randVal = Random.Range(0, intersectionList.Count);
            }
            while (intersectionList[randVal].type != 0);

            intersectionList[randVal].type = colVal;
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

        //region testing! just temp (checkerboard)
        /*
        int currIndex = 0;
        foreach (Region region in regionList)
        {
            print(region.pos.x + ", " + region.pos.y);
            for (int i = 0; i < region.width; i++)
            {
                for (int j = 0; j < region.height; j++)
                {
                    if (currIndex % 2 == 0)
                    {
                        print("even");
                        Instantiate(intersection, new Vector3((region.pos.x + i) * MAP_SCALAR, (region.pos.y + j) * MAP_SCALAR, 1), Quaternion.identity, generationLocation);
                    }
                    else
                    {
                        print("odd");
                        Instantiate(intersection2, new Vector3((region.pos.x + i) * MAP_SCALAR, (region.pos.y + j) * MAP_SCALAR, 1), Quaternion.identity, generationLocation);
                    }

                }
            }
            currIndex += 1;
        }
        */

        //For every intersection...
        for (int i = 0; i < intersectionList.Count; i++)
        {
            
            Vector2Int pos = intersectionList[i].getPos();
            int x = pos.x;
            int y = pos.y;

            //TEMP: remove once road junctions are implemented
            /*
            Vector2Int intPos = intersectionList[i].getPos();
            if ((intPos.x + intPos.y) % 2 == 0)
            {
                //print("even");
                Instantiate(intersection, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, 1), Quaternion.identity, generationLocation);
            }
            else
            {
                //print("odd");
                Instantiate(intersection2, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, 1), Quaternion.identity, generationLocation);
            }
            */

            //junction making
            GameObject toInstantiate = debugSprite;
            Quaternion rotation = Quaternion.identity;
            int connections = 4;
            if (intersectionList[i].atBoundUp()) { connections -= 1; }
            if (intersectionList[i].atBoundRight()) { connections -= 1; }
            if (intersectionList[i].atBoundDown()) { connections -= 1; }
            if (intersectionList[i].atBoundLeft()) { connections -= 1; }

            switch (connections)
            {
                case 0:
                    print("0 connections?!?!");
                    break;
                case 1:
                    print("1 connection?!?!");
                    break;
                case 2:
                    toInstantiate = intsectL;
                    if (intersectionList[i].atBoundUp() && intersectionList[i].atBoundLeft()) { rotation = Quaternion.Euler(0, 0, 0); }
                    if (intersectionList[i].atBoundUp() && intersectionList[i].atBoundRight()) { rotation = Quaternion.Euler(0, 0, -90); }
                    if (intersectionList[i].atBoundDown() && intersectionList[i].atBoundLeft()) { rotation = Quaternion.Euler(0, 0, -270); }
                    if (intersectionList[i].atBoundDown() && intersectionList[i].atBoundRight()) { rotation = Quaternion.Euler(0, 0, -180); }
                    break;
                case 3:
                    toInstantiate = intsectT;
                    if (intersectionList[i].atBoundUp()) { rotation = Quaternion.Euler(0,0,-270); }
                    if (intersectionList[i].atBoundRight()) { rotation = Quaternion.Euler(0, 0, 0); }
                    if (intersectionList[i].atBoundDown()) { rotation = Quaternion.Euler(0, 0, -90); }
                    if (intersectionList[i].atBoundLeft()) { rotation = Quaternion.Euler(0, 0, -180); }
                    break;
                case 4: toInstantiate = intersection;
                    
                    break;
            }
            
            Instantiate(toInstantiate, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, 1), rotation, generationLocation);

            //draw to the intersection based on type
            switch (intersectionList[i].type)
            {
                case 0:
                    break;

                case 1:
                    stopDict.Add(intersectionList[i], Instantiate(blueBusStop, new Vector3(x * MAP_SCALAR + stopOffset.x, y * MAP_SCALAR + stopOffset.y, -1), Quaternion.identity, generationLocation));
                    break;

                case 2:
                    stopDict.Add(intersectionList[i], Instantiate(greenBusStop, new Vector3(x * MAP_SCALAR + stopOffset.x, y * MAP_SCALAR + stopOffset.y, -1), Quaternion.identity, generationLocation));
                    break;

                case 3:
                    stopDict.Add(intersectionList[i], Instantiate(pinkBusStop, new Vector3(x * MAP_SCALAR + stopOffset.x, y * MAP_SCALAR + stopOffset.y, -1), Quaternion.identity, generationLocation));
                    break;

                case 4:
                    stopDict.Add(intersectionList[i], Instantiate(redBusStop, new Vector3(x * MAP_SCALAR + stopOffset.x, y * MAP_SCALAR + stopOffset.y, -1), Quaternion.identity, generationLocation));
                    break;

                case 5:
                    Instantiate(obstacle, new Vector3(x * MAP_SCALAR, y * MAP_SCALAR, -1), Quaternion.identity, generationLocation);
                    break;

                default:
                    break;
            }

        }

        //fill 'er up with SHRUBBERY! 
        int boundFoliageWidth = 3;
        for (int i = 0; i < mapMatrix.GetLength(0) + boundFoliageWidth*2; i++)
        {
            for (int j = 0; j < mapMatrix.GetLength(1) + boundFoliageWidth * 2; j++)
            {
                int foliageIndex = 0;

                //outside map
                if (i < boundFoliageWidth || j < boundFoliageWidth || i >= mapMatrix.GetLength(0) + boundFoliageWidth || j >= mapMatrix.GetLength(0) + boundFoliageWidth) 
                {
                    foliageIndex = Random.Range(0, foliageList.Count);
                    float randomOffsetScalar = 0.2f;
                    float randomOffsetX = Random.Range(-randomOffsetScalar, randomOffsetScalar);
                    float randomOffsetY = Random.Range(-randomOffsetScalar, randomOffsetScalar);
                    Instantiate(foliageList[foliageIndex], new Vector3(i - boundFoliageWidth + randomOffsetX, j - boundFoliageWidth + randomOffsetY, 0), Quaternion.identity, generationLocation);
                }

                //inside map
                else
                {
                    if (mapMatrix[i - boundFoliageWidth, j - boundFoliageWidth] == null)  //&& !isAlone(new Vector2Int(i, j)
                    {
                        foliageIndex = Random.Range(0, foliageList.Count);
                        float randomOffsetScalar = 0.2f;
                        float randomOffsetX = Random.Range(-randomOffsetScalar, randomOffsetScalar);
                        float randomOffsetY = Random.Range(-randomOffsetScalar, randomOffsetScalar);
                        Instantiate(foliageList[foliageIndex], new Vector3(i - boundFoliageWidth + randomOffsetX, j - boundFoliageWidth + randomOffsetY, 0), Quaternion.identity, generationLocation);
                    }
                }
            }
        }

        //districts + buildingzzz
        for (int i = 0; i < regionList.Count; i++)
        {
            Region region = regionList[i];
            int x = region.pos.x;
            int y = region.pos.y;

            List<GameObject> chosenList;

            //choose region type
            if (rareRegionChance > Random.Range(0.0f, 1.0f))
            {
                chosenList = buildingsList2;
            }
            else
            {
                chosenList = buildingsList;
            }

            //for every intersection in the region...
            for (int j = 0; j < region.width - 1; j++)
            {
                for (int k = 0; k < region.height - 1; k++)
                {
                    int buildingIndex = Random.Range(0, chosenList.Count); ;
                    Instantiate(chosenList[buildingIndex], new Vector3(j + x + 0.5f, k + y + 0.35f, 1), Quaternion.identity, generationLocation);

                    /*
                     * 4 BUILDINGS (deprecated)
                    float randomOffsetX;
                    void randomize()
                    {
                        randomOffsetX = Random.Range(-randombuildingOffset, randombuildingOffset);
                        buildingIndex = Random.Range(0, chosenList.Count);
                    }

                    randomize();
                    Instantiate(chosenList[buildingIndex], new Vector3(j + x + buildingOffset + randomOffsetX, k + y + buildingOffset, 1), Quaternion.identity, generationLocation);
                    randomize();
                    Instantiate(chosenList[buildingIndex], new Vector3(j + x + buildingOffset + randomOffsetX, k + y - buildingOffset, 1), Quaternion.identity, generationLocation);
                    randomize();
                    Instantiate(chosenList[buildingIndex], new Vector3(j + x - buildingOffset + randomOffsetX, k + y - buildingOffset, 1), Quaternion.identity, generationLocation);
                    randomize();
                    Instantiate(chosenList[buildingIndex], new Vector3(j + x - buildingOffset + randomOffsetX, k + y + buildingOffset, 1), Quaternion.identity, generationLocation);
                    */
                }
            }
        }

    }

    #region bound checking
    public bool hasIntersectionAbove(Vector2Int toCheck)
    {
        if (toCheck.y - 1 > 0)
        {
            if (mapMatrix[toCheck.x, toCheck.y - 1] == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool hasIntersectionRight(Vector2Int toCheck)
    {
        if (toCheck.x + 1 < mapMatrix.GetLength(0))
        {
            if (mapMatrix[toCheck.x + 1, toCheck.y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool hasIntersectionBelow(Vector2Int toCheck)
    {
        if (toCheck.y + 1 < mapMatrix.GetLength(1))
        {
            if (mapMatrix[toCheck.x, toCheck.y + 1] == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool hasIntersectionLeft(Vector2Int toCheck)
    {
        if (toCheck.x - 1 > 0)
        {
            if (mapMatrix[toCheck.x - 1, toCheck.y] == null)
            {
                return false;
            }
        }
        return true;
    }

    public bool isAlone(Vector2Int toCheck)
    {
        if (!hasIntersectionAbove(toCheck) && !hasIntersectionRight(toCheck) && !hasIntersectionBelow(toCheck) && !hasIntersectionLeft(toCheck))
        {
            return true;
        }
        return false;
    }
    #endregion

    void Awake()
    {
        mapMatrix = new Intersection[width, height];

        GenerateMap(mapMatrix, numStops);
        DrawMap(mapMatrix);
    }

}
