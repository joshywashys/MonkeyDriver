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
    public TMPro.TextMeshProUGUI scoreCounter, scoreBoost, scoreBad;
    void Start()
    {
        if (i == null)
        {
            i = this;
        }
        StartCoroutine(ScoreTickDown());
    }
    public void addScore(float passengerDistance)
    {
        int boost;
        if (passengerDistance < 3.0f)
        {
            boost = baseScore + 400;
            score += boost;
        }
        else
        {
            boost = baseScore;
            score += boost;
        }
        scoreBoost.text = "+" + boost;
        //gotta put this shit in a coroutine huh
        scoreCounter.text = "Score: " + score;
    }
    public void subScore(int subtraction)
    {
        score -= subtraction;
        if (score < 0)
        {
            score = 0;
        }
        scoreCounter.text = "Score: " + score;
    }

    IEnumerator ScoreTickDown()
    {
        while (true)
        {
            subScore(50);
            yield return new WaitForSeconds(1);
        }
    }

    public int getScore()
    {
        return score;
    }
}
