using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum Mood { VeryUpset, Upset, Neutral, Content, Happy }
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
    #endregion

	public void calcScore(Vector2 busPos)
	{
        onBus = false;
        //do something for the score

        //ScoreManager.i.addScore(score);
    }
}
