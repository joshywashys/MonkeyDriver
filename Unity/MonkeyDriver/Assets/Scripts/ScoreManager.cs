using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
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
