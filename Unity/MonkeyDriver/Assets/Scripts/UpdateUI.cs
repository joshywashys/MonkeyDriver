using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    public GameObject bluePassenger, greenPassenger, pinkPassenger, redPassenger;
    public Transform passengerHolder;
    // Update is called once per frame
    void Start()
    {
        foreach (Passenger person in BusControls.passengers)
        {
            string destination = person.getDestination();
            switch (destination)
            {
                case "blue":
                    Instantiate(bluePassenger, passengerHolder);
                    break;
                case "green":
                    Instantiate(greenPassenger, passengerHolder);
                    break;
                case "pink":
                    Instantiate(pinkPassenger, passengerHolder);
                    break;
                case "red":
                    Instantiate(redPassenger, passengerHolder);
                    break;
            }
        }
    }
}
