using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerMood : MonoBehaviour
{
    string curMood;
    public TMPro.TextMeshProUGUI busMood;
    public TMPro.TextMeshProUGUI passengersRemaining;

    // Update is called once per frame
    void Update()
    {
        switch (BusControls.passengers[0].getMood())
        {
            case int mood when (mood >= 1000 && mood > 800):
                curMood = "Happy";
                break;
            case int mood when (mood >= 800 && mood > 600):
                curMood = "Content";
                break;
            case int mood when (mood >= 600 && mood > 400):
                curMood = "Neutral";
                break;
            case int mood when (mood >= 400 && mood > 200):
                curMood = "Upset";
                break;
            case int mood when (mood > 200):
                curMood = "Very Upset";
                break;
        }
        busMood.text = "Mood: " + curMood;
        passengersRemaining.text = "Passengers Remaining: " + BusControls.bus.getNumPassengers();
	}
}
