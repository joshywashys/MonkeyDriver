using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
	MapMatrix map;
	float targetX, targetY;
	public float padding = 0.25f;
	private float UIHeight = 1f;
	private float bottomPadding;


	//have only the four closest stop indicators
	//spawn them at start and set them not visible
	//visible when off screen

	public GameObject blueMini, greenMini, pinkMini, redMini;
	public Transform miniHolder;

	Camera cam;
	float camHalfHeight;
	float camHalfWidth;
	float slope;

	Dictionary<string, GameObject> stopPointers = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
	{
		cam = Camera.main;
		map = FindObjectOfType<MapMatrix>();
		camHalfHeight = cam.orthographicSize;
		camHalfWidth = camHalfHeight * cam.aspect;
		bottomPadding = UIHeight + padding;

		foreach (Intersection stop in map.stopDict.Keys)
		{
			switch (stop.getColour())
			{
				case "blue":
					stopPointers.Add("blue", Instantiate(blueMini, miniHolder));
					break;
				case "green":
					stopPointers.Add("green", Instantiate(greenMini, miniHolder));
					break;
				case "pink":
					stopPointers.Add("pink", Instantiate(pinkMini, miniHolder));
					break;
				case "red":
					stopPointers.Add("red", Instantiate(redMini, miniHolder));
					break;
			}
			stopPointers[stop.getColour()].SetActive(false);
		}
	}

	void Update()
	{
		foreach (KeyValuePair<Intersection, GameObject> stop in map.stopDict)
		{
			//if not visible by camera or if it is below the UI
			if (!stop.Value.GetComponent<SpriteRenderer>().isVisible || stop.Value.transform.position.y < cam.transform.position.y - camHalfHeight + UIHeight )
			{

				slope = (stop.Value.transform.position.y - cam.transform.position.y) / (stop.Value.transform.position.x - cam.transform.position.x);
				stopPointers[stop.Key.getColour()].SetActive(true);

				if (cam.transform.position.x < stop.Value.transform.position.x)
				{
					//right
					targetX = cam.transform.position.x + camHalfWidth - padding;
					targetY = slope * (targetX - stop.Value.transform.position.x) + stop.Value.transform.position.y;

					if (targetY > cam.transform.position.y - camHalfHeight && targetY < cam.transform.position.y + camHalfHeight)
					{
						//draw the mini pointer
						stopPointers[stop.Key.getColour()].transform.position = new Vector3(targetX,targetY, 1);
                    }
					else
					{
						if (cam.transform.position.y < stop.Value.transform.position.y)
						{
							//up
							targetY = cam.transform.position.y + camHalfHeight - padding;
						}
						else
						{
							//down
							targetY = cam.transform.position.y - camHalfHeight + bottomPadding;
						}
						targetX = (targetY - stop.Value.transform.position.y) / slope + stop.Value.transform.position.x;
						//draw mini pointer

						stopPointers[stop.Key.getColour()].transform.position = new Vector3(targetX, targetY, 1);
					}
				}
				else if (cam.transform.position.x >= stop.Value.transform.position.x)
				{
					//left
					targetX = cam.transform.position.x - camHalfWidth + padding;
					targetY = slope * (targetX - stop.Value.transform.position.x) + stop.Value.transform.position.y;

					if (targetY > cam.transform.position.y - camHalfHeight && targetY < cam.transform.position.y + camHalfHeight)
					{
						//get stop colour
						//move pointer of the same colour
						stopPointers[stop.Key.getColour()].transform.position = new Vector3(targetX, targetY, 1);
					}
					else
					{
						if (cam.transform.position.y < stop.Value.transform.position.y)
						{
							//up
							targetY = cam.transform.position.y + camHalfHeight - padding;
						}
						else
						{
							//down
							targetY = cam.transform.position.y - camHalfHeight + bottomPadding;
						}

						targetX = (targetY - stop.Value.transform.position.y) / slope + stop.Value.transform.position.x;

                        //move pointer of the same colour
                        stopPointers[stop.Key.getColour()].transform.position = new Vector3(targetX, targetY, 1);

						//rotation
						//stopPointers[stop.Key.getColour()].transform.rotation = Quaternion.AngleAxis(
						//	Mathf.Atan2(stop.Value.transform.position.x - cam.transform.position.x, stop.Value.transform.position.y - cam.transform.position.y)
						//	* 180 / Mathf.PI + 90, Vector3.forward);

						//                 stopPointers[stop.Key.getColour()].transform.rotation = Quaternion.AngleAxis(
						//Mathf.Atan2(stop.Value.transform.position.y - cam.transform.position.y, stop.Value.transform.position.x - cam.transform.position.x)
						//* 180 / Mathf.PI - 180, Vector3.forward);
					}
                }
			}
			else
			{
				stopPointers[stop.Key.getColour()].SetActive(false);
			}
		}
	}
}
