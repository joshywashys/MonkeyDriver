using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    MapMatrix map;
    float objectDistance;
    public float minDistanceThreshold;
    public float maxDistanceThreshold;
    float targetX, targetY;

    //have only the four closest stop indicators
    //spawn them at start and set them not visible
    //visible when off screen

    public GameObject tester;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        map = FindObjectOfType<MapMatrix>();
        Debug.Log(cam.aspect);

        foreach (GameObject stop in map.stopDict.Values)
        {
            if (!stop.GetComponent<SpriteRenderer>().isVisible)
            {
                if (cam.transform.position.x < stop.transform.position.x)
                {
                    //right
                    targetX = cam.transform.position.x + cam.orthographicSize * cam.aspect;
                    targetY = ((stop.transform.position.y - cam.transform.position.y) / (stop.transform.position.x - cam.transform.position.x)) 
                        * (targetX - stop.transform.position.x) + stop.transform.position.y;

                    if (targetY > cam.transform.position.y - cam.orthographicSize && targetY < cam.transform.position.y + cam.orthographicSize)
                    {
                        Instantiate(tester, new Vector3(targetX, targetY, 1), Quaternion.identity);
                    }
                    else
                    {
                        if (cam.transform.position.y < stop.transform.position.y)
                        {
                            //up
                            targetY = cam.transform.position.y + cam.orthographicSize;
                        }
                        else
                        {
                            //down
                            targetY = cam.transform.position.y - cam.orthographicSize;
                        }
                        targetX = (targetY - stop.transform.position.y) / ((stop.transform.position.y - cam.transform.position.y) / (stop.transform.position.x - cam.transform.position.x))
                                + stop.transform.position.x;
                        Instantiate(tester, new Vector3(targetX, targetY, 1), Quaternion.identity);
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

    void Update()
    {

    }
}
