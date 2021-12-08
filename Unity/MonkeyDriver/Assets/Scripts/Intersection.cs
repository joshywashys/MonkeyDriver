using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I couldve just used a struct... Whoops, too late now!!!
public class Intersection
{
    private static int numIntersections = 0;
    public int id;
    public bool hasBus;
    private Vector2Int pos;

    bool atBoundUp_m;
    bool atBoundRight_m;
    bool atBoundDown_m;
    bool atBoundLeft_m;

    //what is on the intersection
    /*
        0 = Empty
        1 = Blue Stop
        2 = Green
        3 = Pink
        4 = Red
        5 = Obstacle
    */
    public int type;

    #region contructors
    public Intersection()
    {
        numIntersections += 1;
        id = numIntersections;

        atBoundUp_m = false;
        atBoundRight_m = false;
        atBoundDown_m = false;
        atBoundLeft_m = false;
    }

    public Intersection(int x, int y)
    {
        numIntersections += 1;
        id = numIntersections;
        
        atBoundUp_m = false;
        atBoundRight_m = false;
        atBoundDown_m = false;
        atBoundLeft_m = false;

        pos = new Vector2Int(x, y);
    }
    #endregion

    #region getters/setters
    public bool atBoundUp()
    {
        return atBoundUp_m;
    }
    public bool atBoundRight()
    {
        return atBoundRight_m;
    }
    public bool atBoundDown()
    {
        return atBoundDown_m;
    }
    public bool atBoundLeft()
    {
        return atBoundLeft_m;
    }
    public void setBoundUp(bool newVal)
    {
        atBoundUp_m = newVal;
    }
    public void setBoundRight(bool newVal)
    {
        atBoundRight_m = newVal;
    }
    public void setBoundDown(bool newVal)
    {
        atBoundDown_m = newVal;
    }
    public void setBoundLeft(bool newVal)
    {
        atBoundLeft_m = newVal;
    }
    public Vector2Int getPos()
    {
        return pos;
    }

    public string getColour()
    {
        switch (type)
        {
            case 1:
                return ("blue");
            case 2:
                return ("green");
            case 3:
                return ("pink");
            case 4:
                return ("red");
            default:
                return ("");
        }
    }
    #endregion
}