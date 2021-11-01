using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Mood { VeryUpset, Upset, Neutral, Content, Happy }
public class Passenger
{
    Mood m_currentMood;
    float m_commuteTime;
    bool m_onBus = true;
    int m_moodMeter = 1000;
    public int m_moodDecrease;

    Vector2 m_destination;
    public Passenger(Vector2 dest)
    {
		m_destination = dest;
		m_moodMeter = 1000;
		m_moodDecrease = 50;
    }

    #region gettersSetters
  //  public Mood getMood()
  //  {
		//return m_currentMood;
  //  }

	public void setCommuteTime()
    {
		m_commuteTime += Time.deltaTime;
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
	void calcScore(Vector2 position)
	{
		//float distanceToDest;// = position - m_destination;
		//Mathf.Round(distanceToDest);
		//m_moodMeter -= m_moodDecrease * (int)distanceToDest;
		//ScoreManager.i.addScore(m_moodMeter);
	}
}
