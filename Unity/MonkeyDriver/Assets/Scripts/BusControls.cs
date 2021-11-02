using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float lerpSpeed = 0.7f;
    public float minSpeed = 0.1f;
    public float speedDecrement = 0.1f;
    public static int numControls = 4;
    public static int numPassengers = 15;

    public GameObject controls;
    List <Controls> activeControls = new List<Controls>();
    public static List<Passenger> passengers = new List<Passenger>();
    MapMatrix map;

    IEnumerator commuteTooLong()
    {
        while (true)
        {
            yield return new WaitForSeconds(90);
            foreach (Passenger person in passengers)
            {
                person.updateMood("long commute");
            }
        }
    }

    IEnumerator restTime()
    {
        float lastSpeed = lerpSpeed;
        lerpSpeed = 0;
        Debug.Log("resting");
        yield return new WaitForSeconds(3);
        lerpSpeed = lastSpeed;
        executeAction(chooseControl());
    }
    IEnumerator Drive(float startX, float startY, float endX, float endY)
    {
        float DriveTime = 0.0f;

        while (DriveTime < lerpSpeed)
        {
            DriveTime += Time.deltaTime;
            
            transform.position = new Vector3(Mathf.Lerp(startX, endX, DriveTime / lerpSpeed), Mathf.Lerp(startY, endY, DriveTime / lerpSpeed), 0);
            yield return null;
        }
        executeAction(chooseControl());
    }

    void Awake()
    {
        if (bus == null)
        {
            bus = this;
        }

        map = FindObjectOfType<MapMatrix>();
        //populate the first controls
        //activeControls.Add(Controls.Up);
        //activeControls.Add(Controls.Right);
        //activeControls.Add(Controls.Left);

        busPos = new Vector2Int(0,0);
        
    }
    private void Start()
    {
        for (int i = 0; i < numPassengers; i++)
        {
            passengers.Add(new Passenger(map.stopCoordinates[Random.Range(0, map.numStops)]));
        }
        StartCoroutine(restTime());
        SetAvailableControls();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Passenger person in passengers){
            person.updateMood("none");
        }
    }

    void CheckForBounds()
    {
        int mapWidth = map.mapMatrix.GetLength(0) - 1;
        int mapHeight = map.mapMatrix.GetLength(1) - 1;
        Vector2Int currPos = new Vector2Int(busPos.x, busPos.y);

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
    }

    //Up,Down,Left,Right,Plow,Rest,Accelerate
    void SetAvailableControls()
    {
        Transform[] ctrlsList = new Transform[6];
        for (int i = 0; i < 6; i++)
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

        //if (atBoundUp) { activeControls.Remove(Controls.Up); }
        //if (atBoundRight) { activeControls.Remove(Controls.Right); }
        //if (atBoundDown) { activeControls.Remove(Controls.Down); }
        //if (atBoundLeft) { activeControls.Remove(Controls.Left); }
    }

#region control methods
    public void Up()
    {
        if (!atBoundUp)//this check will become obsolete since the control should never be called if it would result in out of bounds
        {
            busPos.y += 1;
            StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x, transform.position.y + 1));
        }
        else
        {
            executeAction(chooseControl());
        }
    }

    public void Down()
    {
        if (!atBoundDown)
        {
            busPos.y -= 1;
            StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x, transform.position.y - 1));
        }
        else
        {
            executeAction(chooseControl());
        }
    }

    public void Left()
    {
        if (!atBoundLeft)
        {
            busPos.x -= 1;
            StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x - 1, transform.position.y));
        }
        else
        {
            executeAction(chooseControl());
        }

    }

    public void Right()
    {
        if (!atBoundRight)
        {
            busPos.x += 1;
            StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x + 1, transform.position.y));
        }
        else
        {
            executeAction(chooseControl());
        }
    }

    public void Plow()
    {
        hasPlow = !hasPlow;
    }

    public void Rest()
    {
        Debug.Log("take a break!");
        StartCoroutine(restTime());
    }

    public void Accelerate()
    {
        if (lerpSpeed < minSpeed)
        {
            lerpSpeed -= speedDecrement;

        }
    }
    #endregion
    int chooseControl()
    {
        //check for banana
        //then the control set must be reduced to the number of viable options
        int monkeyChoice = Random.Range(0, numControls);
        Debug.Log("choosing control "+ monkeyChoice);
        return (monkeyChoice);
    }


    public void executeAction(int control)
    {
        SetAvailableControls();
        CheckForBounds();
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            if (hasPlow)
            {
                foreach (Passenger person in passengers)
                {
                    person.updateMood("hit with plow");
                }
            }
            else
            {
                foreach (Passenger person in passengers)
                {
                    person.updateMood("hit obstacle");
                }
            }
            Destroy(other.gameObject);
        }
    }

    public void ejection()
    {
        float shortestDistance = 1000.0f;
        float distance;
        Vector2 closestStop = new Vector2(-45,-45);
        foreach (Vector2 stop in map.stopCoordinates)
        {
            distance = Mathf.Sqrt(Mathf.Pow((busPos.x - stop.x),2) + Mathf.Pow((busPos.y - stop.y),2));
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestStop = stop;
            }
        }

        foreach(Passenger person in passengers)
        {
            if (person.getDestination() == closestStop)
            {
                person.calcScore(busPos);
                numPassengers -= 1;
            }
        }
        passengers.RemoveAll(person => person.getOnBus() == false);
    }
    public int getNumPassengers()
    {
        return (numPassengers);
    }
}
