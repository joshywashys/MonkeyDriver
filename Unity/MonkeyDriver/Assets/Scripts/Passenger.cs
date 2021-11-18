using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum Mood { VeryUpset, Upset, Neutral, Content, Happy }
public class Passenger
{
    //Mood m_currentMood;
    //float m_commuteTime;

    string m_destination;
    private bool onBus = true;

    public Passenger(string dest)
    {
		m_destination = dest;
    }

    #region getters and setters
  //  public void setCommuteTime()
  //  {
		//m_commuteTime += Time.deltaTime;
  //  }
	public string getDestination()
    {
		return (m_destination);
    }

    public bool getOnBus()
    {
        return onBus;
    }
    #endregion

	public void calcScore(Vector2 busPos)
	{
        float distanceToDest = Mathf.Sqrt(Mathf.Pow((busPos.x + m_destination.x), 2) + Mathf.Pow((busPos.y + m_destination.y), 2));
		Mathf.Round(distanceToDest);
        onBus = false;
        //do something for the score

        //ScoreManager.i.addScore(score);
    }
}
