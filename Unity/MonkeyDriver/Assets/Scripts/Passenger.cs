using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enum Mood { VeryUpset, Upset, Neutral, Content, Happy }
public class Passenger
{
    //Mood m_currentMood;
    float m_commuteTime;
    //bool m_onBus = true;
    int m_moodMeter = 1000;
    public int m_moodDecrease;

    Vector2 m_destination;
    private bool onBus = true;

    public Passenger(Vector2 dest)
    {
		m_destination = dest;
		m_moodMeter = 1000;
		m_moodDecrease = 50;
    }

    #region getters and setters
    public int getMood()
    {
        return m_moodMeter;
    }

    public void setCommuteTime()
    {
		m_commuteTime += Time.deltaTime;
    }
	public Vector2 getDestination()
    {
		return (m_destination);
    }

    public bool getOnBus()
    {
        return onBus;
    }
    #endregion
    public void updateMood(string moodEvent)
	{
		if (moodEvent == "long commute")
        {
			m_moodMeter -= m_moodDecrease;
        }
		else if (moodEvent == "hit obstacle")
        {
			m_moodMeter -= m_moodDecrease * 3;
        }
        else if (moodEvent == "hit with plow")
        {
            m_moodMeter -= m_moodDecrease / 2;
        }
    }
	public void calcScore(Vector2 busPos)
	{
        float distanceToDest = Mathf.Sqrt(Mathf.Pow((busPos.x + m_destination.x), 2) + Mathf.Pow((busPos.y + m_destination.y), 2));
		Mathf.Round(distanceToDest);
        m_moodMeter -= m_moodDecrease * (int)distanceToDest;
        onBus = false;
        BananaController.i.addBananas(1);
        ScoreManager.i.addScore(m_moodMeter);
    }
}
