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

    private Transform busTransform;
    public static bool atBoundUp, atBoundLeft, atBoundRight, atBoundDown; //map changes these
    bool hasPlow = false;
    public float curSpeed, maxSpeed, speedIncrement;
    public static int numControls = 3;

    List <Controls> activeControls = new List<Controls>();
    List<Passenger> passengers = new List<Passenger>();
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


    void Awake()
    {
        if (bus == null)
        {
            bus = this;
        }
    }
    private void Start()
    {
        map = FindObjectOfType<MapMatrix>();
        //populate the first controls
        activeControls.Add(Controls.Up);
        activeControls.Add(Controls.Right);
        activeControls.Add(Controls.Left);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Passenger person in passengers){
            person.updateMood("none");
        }
    }

    bool destinationInBounds(Transform busPos)
    {
        Vector2 currPos = new Vector2(busPos.position.x, busPos.position.y);

        return true;
    }

#region control methods
    public void Up()
    {
        if (!atBoundUp)//this check will become obsolete since the control should never be called if it would result in out of bounds
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Debug.Log("up");
        }
    }

    public void Down()
    {
        if (!atBoundDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            Debug.Log("down");
        }
    }

    public void Left()
    {
        if (!atBoundLeft)
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            Debug.Log("left");
        }
    }

    public void Right()
    {
        if (!atBoundRight)
        {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
            Debug.Log("right");
        }
    }

    public void Plow()
    {
        hasPlow = !hasPlow;
    }

    public void Rest()
    {
        //set speed to zero?
        //need a way for the monkey to make a decision again after 
    }

    public void Accelerate()
    {
        if (curSpeed < maxSpeed)
        {
            curSpeed += speedIncrement;//add whatever factor we determine is justifiable
        }
    }
#endregion

    public void executeAction(int control)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "obstacle")
        {
            foreach (Passenger person in passengers)
            {
                person.updateMood("hit obstacle");
            }
        }
    }

    void ejection()
    {
        float shortestDistance = 1000.0f;
        float distance;
        Vector2 closestStop = new Vector2(-45,-45);
        Vector2 busPos = new Vector2(transform.position.x, transform.position.y);
        foreach (Vector2 stop in map.stopCoordinates)
        {
            distance = Mathf.Sqrt(Mathf.Pow((busPos.x + stop.x),2) + Mathf.Pow((busPos.y + stop.y),2));
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
                passengers.Remove(person);
            }
        }
    }
}
