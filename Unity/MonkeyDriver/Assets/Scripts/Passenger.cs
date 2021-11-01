using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Mood { VeryUpset, Upset, Neutral, Content, Happy }
public class Passenger
{
    Mood m_currentMood;
    float m_commuteTime;
    //bool m_onBus = true;
    int m_moodMeter = 1000;
    public int m_moodDecrease;

    Vector2 m_destination;
    public Passenger(Vector2 dest)
    {
		m_destination = dest;
		m_moodMeter = 1000;
		m_moodDecrease = 50;
    }

    #region getters and setters
  //  public Mood getMood()
  //  {
		//return m_currentMood;
  //  }

	public void setCommuteTime()
    {
		m_commuteTime += Time.deltaTime;
    }
	public Vector2 getDestination()
    {
		return (m_destination);
    }
	public void setDestination()
    {

    }
    #endregion
    public void updateMood(string moodEvent)
	{
		if (moodEvent == "long commute")
        {
			m_moodMeter -= m_moodDecrease;
        }
		else if (moodEvent == "hit obtacle")
        {
			m_moodMeter -= m_moodDecrease * 4;
        }

		switch (m_moodMeter)
		{
			case int mood when (mood >= 1000 && mood > 800):
				m_currentMood = Mood.Happy;
				break;
			case int mood when (mood >= 800 && mood > 600):
				m_currentMood = Mood.Content;
				break;
			case int mood when (mood >= 600 && mood > 400):
				m_currentMood = Mood.Neutral;
				break;
			case int mood when (mood >= 400 && mood > 200):
				m_currentMood = Mood.Upset;
				break;
			case int mood when (mood > 200):
				m_currentMood = Mood.VeryUpset;
				break;
		}
	}
	public void calcScore(Vector2 busPos)
	{
        float distanceToDest = Mathf.Sqrt(Mathf.Pow((busPos.x + m_destination.x), 2) + Mathf.Pow((busPos.y + m_destination.y), 2));
		Mathf.Round(distanceToDest);
        m_moodMeter -= m_moodDecrease * (int)distanceToDest;
        ScoreManager.i.addScore(m_moodMeter);
    }
}
