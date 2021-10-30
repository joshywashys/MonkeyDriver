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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    int chooseControl()
    {
        //check for banana
        //then the control set must be reduced to the number of viable options
        int monkeyChoice = Random.Range(0, BusControls.numControls);
        return (monkeyChoice);
    }
    private void OnCollisionEnter2D(Collision2D collision) //if the monkey in the bus pulls up to an intersection
    {
        if (collision.gameObject.tag == "Intersection")
        {
            BusControls.bus.executeAction(chooseControl());
        }
    }
}
