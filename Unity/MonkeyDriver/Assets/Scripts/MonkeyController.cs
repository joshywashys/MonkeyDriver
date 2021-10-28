using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script is responsible for:
-Monkey's random choice
-slots for the current controls
-monkey moving itself, checking for obstacles etc
*/
public class MonkeyController : MonoBehaviour
{
    //SET THIS TO THE REAL NUMBER FROM THE BUS
    int numControls = 3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if at an intersection, make a control choice
    }

    int chooseControl()
    {
        //check for banana
        //check for if a control would result in out of bounds
        //eg. can't go up
        //then the control set must be reduced to the number of viable options
        int monkeyChoice = Random.Range(0, numControls);
        return (monkeyChoice);
    }
}
