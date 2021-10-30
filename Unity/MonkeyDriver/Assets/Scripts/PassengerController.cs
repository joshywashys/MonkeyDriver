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

	Vector3 destination = new Vector3(0,0,0); // UPDATE WITH RANDOM INTERSECTION DESTINATION
    private float distanceToDest;

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
		updateMood("none");
	}
	void updateMood(string condition)
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
		//distanceToDest = transform.position - destination;
		Mathf.Round(distanceToDest);
		moodMeter -= moodDecrease * (int)distanceToDest;
		ScoreManager.i.addScore(moodMeter);
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
		//THIS FUNCTION IS FOR WHEN THEY LEAVE THE BUS
        if (collision.gameObject.tag == "Bus")
        {
			calcScore();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //THIS FUNCTION IS FOR WHEN THEY HIT OBJECTS
    }
}
