using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //score strats at zero
    //if not at 0, it goes down over time or if an obstacle is hit
    //score goes up if the passengers are dropped off
    public static ScoreManager i = null;
    public int score = 0;
    public int baseScore = 100;
    public TMPro.TextMeshProUGUI scoreCounter;
    void Start()
    {
        if (i == null)
        {
            i = this;
        }
    }
    public void addScore(int addition)
    {
        score += addition;
        scoreCounter.text = "Score: " + score;
    }

}
