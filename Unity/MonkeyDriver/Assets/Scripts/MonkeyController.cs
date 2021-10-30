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
    IEnumerator testMonkey()
    {
        while (true)
        { //remove from loop to begin making decisions at start of game
            BusControls.bus.executeAction(chooseControl());
            yield return new WaitForSeconds(5);
        }
    }
    void Start()
    {
        StartCoroutine(testMonkey());
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
        Debug.Log("choosing control " + monkeyChoice);
        return (monkeyChoice);
    }
    private void OnTriggerEnter2D(Collider2D collision) //if the monkey in the bus pulls up to an intersection
    {
        if (collision.gameObject.tag == "Intersection")
        {
            BusControls.bus.executeAction(chooseControl());
        }
    }
}
