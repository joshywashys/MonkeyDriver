using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
This script is responsible for:
-each control that the bus can take (to be used by the monkey)
-bus is maintaining it's own position
-bus is a singleton
-map pulls the bus location
-map checks if the bus is at the boundary and changes the property for the bus
*/
enum Controls
{
    Up,
    Down,
    Left,
    Right,
    Plow,
    Rest,
    Accelerate
}
public class BusControls : MonoBehaviour
{
    public static BusControls bus = null;

    private Vector2Int busPos;
    public static bool atBoundUp, atBoundLeft, atBoundRight, atBoundDown; //map changes these
    bool hasPlow = false;
    private float lerpSpeed = 0.7f;
    private float minSpeed = 0.1f;
    private float speedDecrement = 0.07f;
    public static int numControls = 4;
    public static int numPassengers = 20;

    public GameObject controls;
    List <Controls> activeControls = new List<Controls>();
    public static List<GameObject> passengers = new List<GameObject>();
    MapMatrix map;
    private int monkeyChoice;

    IEnumerator restTime(float time)
    {
        float lastSpeed = lerpSpeed;
        lerpSpeed = 0;
        yield return new WaitForSeconds(time);
        lerpSpeed = lastSpeed;
        monkeyChoice = Random.Range(0, numControls);
        executeAction(monkeyChoice);
    }
    IEnumerator Drive(float startX, float startY, float endX, float endY)
    {
        float DriveTime = 0.0f;

        while (DriveTime < lerpSpeed)
        {
            DriveTime += Time.deltaTime;
            
            transform.position = new Vector3(Mathf.Lerp(startX, endX, DriveTime / lerpSpeed), Mathf.Lerp(startY, endY, DriveTime / lerpSpeed), -6);
            yield return null;
        }
        monkeyChoice = Random.Range(0, numControls);
        executeAction(monkeyChoice);
    }

    void Awake()
    {
        if (bus == null)
        {
            bus = this;
        }

        map = FindObjectOfType<MapMatrix>();
    }

    void Start()
    {
        //set bus starting pos
        int randVal;
        int testVal = 0;
        do
        {
            randVal = Random.Range(0, map.intersectionList.Count);
            map.intersectionList[randVal].getPos();
            testVal++;
        }
        while (map.intersectionList[randVal].type != 0 && testVal < 10);

        busPos = map.intersectionList[randVal].getPos();
        Vector3 startPos = new Vector3(busPos.x, busPos.y, 0);
        transform.position = startPos;
    }

    private void OnEnable()
    {
        StartCoroutine(restTime(3));
        SetAvailableControls();
    }

    /*
    void CheckForBounds()
    {
        Vector2Int currPos = new Vector2Int(busPos.x, busPos.y);
        Intersection currIntersection = map.mapMatrix[busPos.x,busPos.y];

        int mapWidth = map.mapMatrix.GetLength(0) * map.MAP_SCALAR - 1;
        int mapHeight = map.mapMatrix.GetLength(1) * map.MAP_SCALAR - 1;

        atBoundUp = false;
        atBoundRight = false;
        atBoundDown = false;
        atBoundLeft = false;

        if (currPos.y == mapHeight)
        {
            atBoundUp = true;
        }
        if (currPos.x == mapWidth)
        {
            atBoundRight = true;
        }
        if (currPos.y == 0)
        {
            atBoundDown = true;
        }
        if (currPos.x == 0)
        {
            atBoundLeft = true;
        }

        //new version
        if (currIntersection.atBoundUp())
        {
            atBoundUp = true;
        }
        if (currPos.x == mapWidth)
        {
            atBoundRight = true;
        }
        if (currPos.y == 0)
        {
            atBoundDown = true;
        }
        if (currPos.x == 0)
        {
            atBoundLeft = true;
        }
    }
    */

    //Up,Down,Left,Right,Plow,Rest,Accelerate
    void SetAvailableControls()
    {
        Transform[] ctrlsList = new Transform[7];
        for (int i = 0; i < 7; i++)
        {
            ctrlsList[i] = controls.transform.GetChild(i);
        }

        List<int> availableCtrlNums = new List<int>();
        foreach (Transform currCtrl in ctrlsList)
        {
            if (currCtrl.GetComponent<DragUI>().isEnabled)
            {
                availableCtrlNums.Add(currCtrl.GetComponent<DragUI>().ctrlNum);
            }
        }

        activeControls.Clear();
        //get ControlSlots enabled controls
        foreach (int ctrlNum in availableCtrlNums)
        {
            activeControls.Add((Controls)ctrlNum);
        }

        Intersection currIntersection = map.mapMatrix[busPos.x, busPos.y];

        if (currIntersection.atBoundUp()) { activeControls.Remove(Controls.Up); }
        if (currIntersection.atBoundRight()) { activeControls.Remove(Controls.Right); }
        if (currIntersection.atBoundDown()) { activeControls.Remove(Controls.Down); }
        if (currIntersection.atBoundLeft()) { activeControls.Remove(Controls.Left); }
    }

#region control methods
    public void Up()
    {
        if (!atBoundUp)//this check will become obsolete since the control should never be called if it would result in out of bounds
        {
            busPos.y += 1;
            StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x, transform.position.y +  map.MAP_SCALAR));
        }
        else
        {
            monkeyChoice = Random.Range(0, numControls);
            executeAction(monkeyChoice);
        }
    }

    public void Down()
    {
        if (!atBoundDown)
        {
            busPos.y -=  1;
            StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x, transform.position.y -  map.MAP_SCALAR));
        }
        else
        {
            monkeyChoice = Random.Range(0, numControls);
            executeAction(monkeyChoice);
        }
    }

    public void Left()
    {
        if (!atBoundLeft)
        {
            busPos.x -=  1;
            StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x -  map.MAP_SCALAR, transform.position.y));
        }
        else
        {
            monkeyChoice = Random.Range(0, numControls);
            executeAction(monkeyChoice);
        }

    }

    public void Right()
    {
        if (!atBoundRight)
        {
            busPos.x +=  1;
            StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x +  map.MAP_SCALAR, transform.position.y));
        }
        else
        {
            monkeyChoice = Random.Range(0, numControls);
            executeAction(monkeyChoice);
        }
    }

    public void Plow()
    {
        hasPlow = !hasPlow;
        StartCoroutine(restTime(0.3f));
    }

    public void Rest()
    {
        StartCoroutine(restTime(3));
    }

    public void Accelerate()
    {
        if (lerpSpeed > minSpeed)
        {
            lerpSpeed -= speedDecrement;
        }
        monkeyChoice = Random.Range(0, numControls);
        executeAction(monkeyChoice);
    }
    #endregion

    private void executeAction(int control)
    {
        SetAvailableControls();
        //CheckForBounds();
        try
        {
            switch (activeControls[control])
            {
                case Controls.Up:
                    Up();
                    break;
                case Controls.Down:
                    Down();
                    break;
                case Controls.Left:
                    Left();
                    break;
                case Controls.Right:
                    Right();
                    break;
                case Controls.Plow:
                    Plow();
                    break;
                case Controls.Rest:
                    Rest();
                    break;
                case Controls.Accelerate:
                    Accelerate();
                    break;
            }
        }
        catch
        {
            monkeyChoice = Random.Range(0, numControls);
            executeAction(monkeyChoice);
        }
        
    }

    public void ejection()
    {
        float shortestDistance = 1000.0f;
        float distance;
        Vector2 closestStop = new Vector2(-100,-100);

        foreach (Vector2 stop in map.destinations.Keys)
        {
            distance = Mathf.Sqrt(Mathf.Pow((busPos.x - stop.x),2) + Mathf.Pow((busPos.y - stop.y),2));
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestStop = stop;
            }
        }

        foreach(GameObject person in passengers)
        {
            person.GetComponent<PassengerBehaviour>().ejectPassenger(map.destinations[closestStop], shortestDistance);
        }
        passengers.RemoveAll(person => person.GetComponent<PassengerBehaviour>().getOnBus() == false);
        if (passengers.Count == 0)
        {
            StateManager.i.setGameState(gameState.FINISHED);
        }
    }
}
