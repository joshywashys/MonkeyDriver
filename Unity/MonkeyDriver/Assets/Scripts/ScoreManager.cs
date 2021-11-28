using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //score strats at zero
    //if not at 0, it goes down over time or if an obstacle is hit
    //score goes up if the passengers are dropped off
    public static ScoreManager i = null;
    private int score = 0;
    private int baseScore = 300;
    public TMPro.TextMeshProUGUI scoreCounter;
    void Start()
    {
        if (i == null)
        {
            i = this;
        }
    }
    public void addScore(float passengerDistance)
    {
        if (passengerDistance < 3.0f)
        {
            score += (baseScore + 400);
        }
        else
        {
            score += baseScore;
        }
        scoreCounter.text = "Score: " + score;
    }
    public void subScore(int subtraction)
    {
        if (score != 0)
        {
            score -= subtraction;
        }
    }

    IEnumerator ScoreTickDown()
    {
        while (true)
        {
            subScore(3);
            yield return new WaitForSeconds(5);
        }
    }
}
