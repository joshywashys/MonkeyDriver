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
        1 = Stop
        2 = Obstacle
    */
    public int type;

    public Intersection()
    {
        numIntersections += 1;
        id = numIntersections;
    }
}