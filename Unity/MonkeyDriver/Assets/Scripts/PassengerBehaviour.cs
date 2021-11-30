using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerBehaviour : MonoBehaviour
{
    private string m_destColour;
    private bool m_onBus = true;

    private SpriteRenderer renderer;
    public Sprite blueSprite, greenSprite, pinkSprite, redSprite;
    void Awake()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region getters and setters
    public string getDestination()
    {
        return (m_destColour);
    }

    public bool getOnBus()
    {
        return m_onBus;
    }

    public void setOnBus(bool busStatus)
    {
        m_onBus = busStatus;
    }

    public void setDestination(string destination)
    {
        m_destColour = destination;
        swapSprite(m_destColour);
    }
    #endregion

    public void ejectPassenger(string destinationColour, float distance)
    {
        //if the passenger's destination is the same colour as the closest stop then eject them
        if (getDestination() == destinationColour)
        {
            setOnBus(false);
            ScoreManager.i.addScore(distance);
        }
        
    }

    void swapSprite(string colour)
    {
        switch (colour)
        {
            case "blue":
                renderer.sprite = blueSprite;
                break;
            case "green":
                renderer.sprite = greenSprite;
                break;
            case "pink":
                renderer.sprite = pinkSprite;
                break;
            case "red":
                renderer.sprite = redSprite;
                break;
        }
    }
}