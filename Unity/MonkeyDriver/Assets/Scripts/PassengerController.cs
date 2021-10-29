using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This script is responsible for:
-mood
-destination
-time spent on bus
*/

enum Mood {VeryUpset, Upset, Neutral, Content, Happy }
public class PassengerController : MonoBehaviour
{
	Mood currentMood;
	float commuteTime;
	bool onBus = false;
	int moodMeter = 1000;
    public int moodDecrease = 50;

    IEnumerator commuteTooLong()
	{
		while (onBus)
		{
			yield return new WaitForSeconds(90);
			moodMeter -= moodDecrease;
		}
	}
	void Start()
	{
		currentMood = Mood.Happy;
	}

	// Update is called once per frame
	void Update()
	{
		updateMood();
	}
	void updateMood()
	{
		switch (moodMeter)
		{
			case int mood when (mood>= 1000 && mood > 800):
				currentMood = Mood.Happy;
				break;
			case int mood when (mood >= 800 && mood > 600):
				currentMood = Mood.Content;
				break;
			case int mood when (mood >= 600 && mood > 400):
				currentMood = Mood.Neutral;
				break;
			case int mood when (mood >= 400 && mood > 200):
				currentMood = Mood.Upset;
				break;
			case int mood when (mood > 200):
				currentMood = Mood.VeryUpset;
				break;
		}
	}

	void calcScore()
	{
		ScoreManager.i.addScore(moodMeter);
	}
}
