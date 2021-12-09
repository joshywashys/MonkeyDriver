using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
This script is responsible for:
-each control that the bus can take (to be used by the monkey)
-bus is maintaining it's own position
-bus is a singleton
-map pulls the bus location
-map checks if the bus is at the boundary and changes the property for the bus
*/
enum Controls
{
	Up,
	Down,
	Left,
	Right,
	Accelerate,
	Rest
}
public class BusControls : MonoBehaviour
{
	public static BusControls bus = null;

	private Vector2Int busPos;
	public static bool atBoundUp, atBoundLeft, atBoundRight, atBoundDown; //map changes these
	private float lerpSpeed = 0.5f;
	private float maxSpeed = 0.1f;
	private float defaultSpeed = 0.5f;
	public static int numControls = 4;
	public static int numPassengers = 20;

	public GameObject controls;
	Dictionary<int, Transform> availableCtrlNums = new Dictionary<int, Transform>();
	List <Controls> activeControls = new List<Controls>();
	public static List<GameObject> passengers = new List<GameObject>();
	MapMatrix map;
	private int monkeyChoice;
	private Controls lastChoice;
	private Controls lastDirection;

	IEnumerator RestTime(float time)
	{
		float lastSpeed = lerpSpeed;
		lerpSpeed = 0;
		yield return new WaitForSeconds(time);
		lerpSpeed = lastSpeed;
		
		executeAction();
	}

	IEnumerator SpeedUp()
	{
		lerpSpeed = maxSpeed;
		yield return new WaitForSeconds(5);
		lerpSpeed = defaultSpeed;
		//disable speed control?
	}

	IEnumerator Drive(float startX, float startY, float endX, float endY)
	{
		float DriveTime = 0.0f;

		while (DriveTime < lerpSpeed)
		{
			DriveTime += Time.deltaTime;
			
			transform.position = new Vector3(Mathf.Lerp(startX, endX, DriveTime / lerpSpeed), Mathf.Lerp(startY, endY, DriveTime / lerpSpeed), -6);
			yield return null;
		}
		
		executeAction();
	}

	void Awake()
	{
		if (bus == null)
		{
			bus = this;
		}

		map = FindObjectOfType<MapMatrix>();
	}

	void Start()
	{
		//set bus starting pos
		int randVal;
		int testVal = 0;
		do
		{
			randVal = Random.Range(0, map.intersectionList.Count);
			map.intersectionList[randVal].getPos();
			testVal++;
		}
		while (map.intersectionList[randVal].type != 0 && testVal < 10);

		busPos = map.intersectionList[randVal].getPos();
		Vector3 startPos = new Vector3(busPos.x, busPos.y, 0);
		transform.position = startPos;
	}

	private void OnEnable()
	{
		StartCoroutine(RestTime(3));
		//SetAvailableControls();
	}

	
	void CheckForBounds()
	{
		Vector2Int currPos = new Vector2Int(busPos.x, busPos.y);
		Intersection currIntersection = map.mapMatrix[busPos.x,busPos.y];

		int mapWidth = map.mapMatrix.GetLength(0) * map.MAP_SCALAR - 1;
		int mapHeight = map.mapMatrix.GetLength(1) * map.MAP_SCALAR - 1;

		atBoundUp = false;
		atBoundRight = false;
		atBoundDown = false;
		atBoundLeft = false;

        //if (currPos.y == mapHeight)
        //{
        //    atBoundUp = true;
        //}
        //if (currPos.x == mapWidth)
        //{
        //    atBoundRight = true;
        //}
        //if (currPos.y == 0)
        //{
        //    atBoundDown = true;
        //}
        //if (currPos.x == 0)
        //{
        //    atBoundLeft = true;
        //}

        //new version
        if (currIntersection.atBoundUp())
		{
			atBoundUp = true;
		}
		if (currIntersection.atBoundRight())
		{
			atBoundRight = true;
		}
		if (currIntersection.atBoundDown())
		{
			atBoundDown = true;
		}
		if (currIntersection.atBoundLeft())
		{
			atBoundLeft = true;
		}
	}
	

	//Up,Down,Left,Right,Rest,Accelerate
	void SetAvailableControls()
	{
		Transform[] ctrlsList = new Transform[6];
		for (int i = 0; i < 6; i++)
		{
			ctrlsList[i] = controls.transform.GetChild(i);
		}

		availableCtrlNums.Clear();
		foreach (Transform currCtrl in ctrlsList)
		{
			if (currCtrl.GetComponent<DragUI>().isEnabled)
			{
				availableCtrlNums.Add(currCtrl.GetComponent<DragUI>().ctrlNum, currCtrl);
			}
		}

		activeControls.Clear();
		//get ControlSlots enabled controls
		foreach (int ctrlNum in availableCtrlNums.Keys)
		{
			activeControls.Add((Controls)ctrlNum);
		}

		Intersection currIntersection = map.mapMatrix[busPos.x, busPos.y];

		if (currIntersection.atBoundUp()) { activeControls.Remove(Controls.Up); }
		if (currIntersection.atBoundRight()) { activeControls.Remove(Controls.Right); }
		if (currIntersection.atBoundDown()) { activeControls.Remove(Controls.Down); }
		if (currIntersection.atBoundLeft()) { activeControls.Remove(Controls.Left); }

	}

#region control methods
	public void Up()
	{
		if (!atBoundUp)//this check will become obsolete since the control should never be called if it would result in out of bounds
		{
			busPos.y += 1;
			StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x, transform.position.y +  map.MAP_SCALAR));
		}
		else
		{
			executeAction();
		}
	}

	public void Down()
	{
		if (!atBoundDown)
		{
			busPos.y -=  1;
			StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x, transform.position.y -  map.MAP_SCALAR));
		}
		else
		{
			executeAction();
		}
	}

	public void Left()
	{
		if (!atBoundLeft)
		{
			busPos.x -=  1;
			StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x -  map.MAP_SCALAR, transform.position.y));
		}
		else
		{	
			executeAction();
		}

	}

	public void Right()
	{
		if (!atBoundRight)
		{
			busPos.x +=  1;
			StartCoroutine(Drive(transform.position.x, transform.position.y, transform.position.x +  map.MAP_SCALAR, transform.position.y));
		}
		else
		{
			executeAction();
		}
	}
	
	public void Rest()
	{
		//if (!(lastChoice == Controls.Rest))
  //      {
			StartCoroutine(RestTime(1.5f));
  //      }
		//else
  //      {
			//executeAction();
//		}
	}

	public void Accelerate()
	{
		//have speed be additive
		//have speed move in the last direction
		//if it can't move in that direction, choose a direction that it can go in
		StartCoroutine(SpeedUp());

		executeAction();
	}
	#endregion

	private void executeAction()
	{
		monkeyChoice = Random.Range(0, numControls);
		SetAvailableControls();

		CheckForBounds();
		try
		{
			Debug.Log("choosing: " + activeControls[monkeyChoice]);
			switch (activeControls[monkeyChoice])
			{
				case Controls.Up:
					MonkeyButtonPress.i.PressButton(availableCtrlNums[(int)Controls.Up].position.y);
					Up();
					//Debug.Log("Up position: " + availableCtrlNums[(int)Controls.Up].localPosition);
					lastChoice = Controls.Up;
					break;
				case Controls.Down:
					Down();
					lastChoice = Controls.Down;
					break;
				case Controls.Left:
					Left();
					lastChoice = Controls.Left;
					break;
				case Controls.Right:
					Right();
					lastChoice = Controls.Right;
					break;
				case Controls.Rest:
					MonkeyButtonPress.i.PressButton(availableCtrlNums[(int)Controls.Rest].position.y);
					Rest();
					Debug.Log("Rest position: " + availableCtrlNums[(int)Controls.Rest].localPosition);
					lastChoice = Controls.Rest;
					break;
				case Controls.Accelerate:
					Accelerate();
					lastChoice = Controls.Accelerate;
					break;
			}
		}
		catch
		{
			executeAction();
		}
		
	}

	public void ejection()
	{
		float shortestDistance = 1000.0f;
		float distance;
		Intersection closestStop = map.stopDict.ElementAt(0).Key;

		foreach (Intersection stop in map.stopDict.Keys)
		{
			distance = Mathf.Sqrt(Mathf.Pow((busPos.x - stop.getPos().x),2) + Mathf.Pow((busPos.y - stop.getPos().y),2));
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				closestStop = stop;
			}
		}

		foreach(GameObject person in passengers)
		{
			person.GetComponent<PassengerBehaviour>().ejectPassenger(closestStop.getColour(), shortestDistance);
		}
		passengers.RemoveAll(person => person.GetComponent<PassengerBehaviour>().getOnBus() == false);

		if (passengers.Count == 0)
		{
			StateManager.i.setGameState(gameState.FINISHED);
		}
	}
}
