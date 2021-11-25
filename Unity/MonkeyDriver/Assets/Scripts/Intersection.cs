using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I couldve just used a struct... Whoops, too late now!!!
public class Intersection
{
    private static int numIntersections = 0;
    public int id;
    public bool hasBus;

    //what is on the intersection
    /*
        0 = Empty
        1 = Blue Stop
        2 = Green
        3 = Orange
        4 = Purple
        5 = Red
        6 = Obstacle
    */
    public int type;

    public Intersection()
    {
        numIntersections += 1;
        id = numIntersections;
    }
}