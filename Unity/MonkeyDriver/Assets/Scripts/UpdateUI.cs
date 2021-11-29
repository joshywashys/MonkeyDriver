using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    public GameObject bluePassenger, greenPassenger, pinkPassenger, redPassenger;
    public Transform passengerHolder;

    private int tilingWidth = 40;
    private int tilingHeight = 30;
    private int tileWidth = 5;
    private int tileHeight = 4;

    int tileXPos;
    int tileYPos;

    List<GameObject> passengerIcons = new List<GameObject>();

    //new Vector3(i, j, 1)
    void Start()
    {
        foreach (Passenger person in BusControls.passengers)
        {
            string destination = person.getDestination();
            switch (destination)
            {
                case "blue":
                    passengerIcons.Add(Instantiate(bluePassenger, passengerHolder));
                    break;
                case "green":
                    passengerIcons.Add(Instantiate(greenPassenger, passengerHolder));
                    break;
                case "pink":
                    passengerIcons.Add(Instantiate(pinkPassenger, passengerHolder));
                    break;
                case "red":
                    passengerIcons.Add(Instantiate(redPassenger, passengerHolder));
                    break;
            }
        }

        for (int i =0; i < BusControls.numPassengers; i++)
        {
            tileXPos = i / tileHeight;
            tileYPos = i % tileHeight;

            passengerIcons[i].transform.localPosition = new Vector3(tileXPos * tilingWidth, tileYPos * tilingHeight, 0);
        }
    }
}
