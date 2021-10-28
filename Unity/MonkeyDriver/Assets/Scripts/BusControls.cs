using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script is responsible for:
-each control that the bus can take (to be used by the monkey)
-collision with obstacles
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

    bool atBoundUp, atBoundLeft, atBoundRight, atBoundDown;
    bool hasPlow = false;
    public float speed;
    Controls[] activeControls = new Controls[3];
    public void Up()
    {
        if (!atBoundUp)
        {
            //move up
        }
    }

    public void Down()
    {

    }

    public void Left()
    {

    }

    public void Right()
    {

    }

    public void Plow()
    {

    }

    public void Rest()
    {

    }

    public void Accelerate()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
