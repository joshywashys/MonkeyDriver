using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerManager : MonoBehaviour
{
    public GameObject passenger;
    public Transform passengerHolder;

    private int tilingWidth = 35;
    private int tilingHeight = 20;
    private int tileWidth = 5;
    private int tileHeight = 4;

    int tileXPos;
    int tileYPos;

    void Start()
    {
        createPassengers();
        tilePassengers();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void createPassengers()
    {
        for (int i = 0; i < BusControls.numPassengers; i++)
        {
            BusControls.passengers.Add(Instantiate(passenger, passengerHolder));

        }

        foreach(GameObject person in BusControls.passengers)
        {
            int destination = Random.Range(0, 4);
            switch (destination)
            {
                case 0:
                    person.GetComponent<PassengerBehaviour>().setDestination("blue");
                    break;
                case 1:
                    person.GetComponent<PassengerBehaviour>().setDestination("green");
                    break;
                case 2:
                    person.GetComponent<PassengerBehaviour>().setDestination("pink");
                    break;
                case 3:
                    person.GetComponent<PassengerBehaviour>().setDestination("red");
                    break;
            }
        }
    }

    void tilePassengers()
    {
        for (int i = 0; i < BusControls.numPassengers; i++)
        {
            tileXPos = i / tileHeight;
            tileYPos = i % tileHeight;

            BusControls.passengers[i].transform.localPosition = new Vector3(tileXPos * tilingWidth, tileYPos * tilingHeight, 0);
        }
    }
}
