using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger
{
    string m_destColour;
    private bool onBus = true;

    public Passenger(string dest)
    {
		m_destColour = dest;
    }

    #region getters and setters
    public string getDestination()
    {
		return (m_destColour);
    }

    public bool getOnBus()
    {
        return onBus;
    }

    public void setOnBus(bool busStatus)
    {
        onBus = busStatus;   
    }
    #endregion
}
