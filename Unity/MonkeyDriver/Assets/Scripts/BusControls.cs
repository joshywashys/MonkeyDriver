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

    public static bool atBoundUp, atBoundLeft, atBoundRight, atBoundDown; //map changes these
    bool hasPlow = false;
    public float curSpeed, maxSpeed, speedIncrement;
    public static int numControls = 3;

    List <Controls> activeControls = new List<Controls>();

    void Start()
    {
        if (bus == null)
        {
            bus = this;
        }

        //populate the first controls
        activeControls.Add(Controls.Up);
        activeControls.Add(Controls.Right);
        activeControls.Add(Controls.Left);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
#region control methods
    public void Up()
    {
        if (!atBoundUp)//this check will become obsolete since the control should never be called if it would result in out of bounds
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
    }

    public void Down()
    {
        if (!atBoundDown)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }
    }

    public void Left()
    {
        if (!atBoundLeft)
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
        }
    }

    public void Right()
    {
        if (!atBoundRight)
        {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        }
    }

    public void Plow()
    {
        hasPlow = !hasPlow;
    }

    public void Rest()
    {
        //set speed to zero?
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


}
