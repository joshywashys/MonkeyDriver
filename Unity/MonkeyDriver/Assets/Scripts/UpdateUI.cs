using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUI : MonoBehaviour
{
    string curMood;
    public TMPro.TextMeshProUGUI busMood;
    public TMPro.TextMeshProUGUI passengersRemaining;
    public TMPro.TextMeshProUGUI bananaCount;

    // Update is called once per frame
    void Update()
    {
        passengersRemaining.text = "Passengers Remaining: " + BusControls.bus.getNumPassengers();
	}
}
