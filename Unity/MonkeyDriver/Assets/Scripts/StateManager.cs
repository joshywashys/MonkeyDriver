using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum gameState
{
    PLAY,
    FINISHED
}


public class StateManager : MonoBehaviour
{
    public static StateManager i = null;
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI scoreText;


    gameState curGameState = gameState.PLAY;

    void Start()
    {
        if (i == null)
        {
            i = this;
        }
    }

    public void setGameState(gameState flag)
    {
        if (flag == gameState.FINISHED)
        {
            curGameState = gameState.FINISHED;
            scoreText.text = "Your final score: " + ScoreManager.i.getScore();
            gameOverPanel.SetActive(true);
        }
    }

    public gameState getGameState()
    {
        return curGameState;
    }
}
