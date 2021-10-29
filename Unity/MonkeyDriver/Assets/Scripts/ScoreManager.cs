using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager i = null;
    public int score = 0;
    public int baseScore = 100;

    void Start()
    {
        if (i == null)
        {
            i = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addScore(int addition)
    {
        score += addition;
    }

}
