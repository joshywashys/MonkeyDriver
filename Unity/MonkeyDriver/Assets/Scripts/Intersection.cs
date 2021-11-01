using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I couldve just used a struct... Whoops, too late now!!!
public class Intersection
{
    public int m_id;
    public bool hasMonkey;

    //what is on the intersection
    /*
        0 = Empty
        1 = Stop
        2 = Obstacle
         
    */
    public int type;

    public Intersection()
    {

    }

}