using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    MapMatrix map;
    float targetX, targetY;
    public float padding;

    //have only the four closest stop indicators
    //spawn them at start and set them not visible
    //visible when off screen

    public GameObject tester;

    Camera cam;
    float camHeight;
    float camWidth;

    List<GameObject> stopPointers;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        map = FindObjectOfType<MapMatrix>();
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        foreach (GameObject stop in map.stopDict.Values)
        {
            //instantiate a pointer of the correct colour
        }
    }

    void Update()
    {
        foreach (GameObject stop in map.stopDict.Values)
        {
            if (!stop.GetComponent<SpriteRenderer>().isVisible)
            {
                if (cam.transform.position.x < stop.transform.position.x)
                {
                    //right
                    targetX = cam.transform.position.x + camWidth - padding;
                    targetY = ((stop.transform.position.y - cam.transform.position.y) / (stop.transform.position.x - cam.transform.position.x))
                        * (targetX - stop.transform.position.x) + stop.transform.position.y;

                    if (targetY > cam.transform.position.y - camHeight && targetY < cam.transform.position.y + camHeight)
                    {
                        //get stop colour
                        //turn off pointer of the same colour
                    }
                    else
                    {
                        if (cam.transform.position.y < stop.transform.position.y)
                        {
                            //up
                            targetY = cam.transform.position.y + camHeight - padding;
                        }
                        else
                        {
                            //down
                            targetY = cam.transform.position.y - camHeight + padding;
                        }
                        targetX = (targetY - stop.transform.position.y) / ((stop.transform.position.y - cam.transform.position.y) / (stop.transform.position.x - cam.transform.position.x))
                                + stop.transform.position.x;
                        //get stop colour
                        //turn off pointer of the same colour
                    }

                }
                else if (cam.transform.position.x > stop.transform.position.x)
                {
                    Debug.Log("stop left");
                }
            }
            else
            {
                //hide it
            }
        }
    }
}
