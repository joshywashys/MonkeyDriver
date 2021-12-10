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
	private float speedBonus = 0.1f;
	private float defaultSpeed = 0.5f;
	public static int numControls = 4;
	public static int numPassengers = 20;

	public GameObject controls;
	Dictionary<int, RectTransform> availableCtrlNums = new Dictionary<int, RectTransform>();
	List <Controls> activeControls = new List<Controls>();

	public static List<GameObject> passengers = new List<GameObject>();
	MapMatrix map;
	private int monkeyChoice;
	private Controls lastChoice;
	private Controls lastChoiceDir;

	IEnumerator RestTime(float time)
	{
		float lastSpeed = lerpSpeed;
		lerpSpeed -= lerpSpeed;
		yield return new WaitForSeconds(time);
		lerpSpeed += lastSpeed;
        executeAction();
    }

	IEnumerator SpeedUp()
	{
        if (lerpSpeed > 0.2)
        {
            lerpSpeed -= speedBonus;
            yield return new WaitForSeconds(1);
            lerpSpeed += speedBonus;
        }

	}

	IEnumerator Drive(float startX, float startY, float endX, float endY)
	{
		float DriveTime = 0.0f;
        float speed = lerpSpeed;
        if (speed < 0)
        {
            speed = 0;
        }
        

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
        print("checked bounds");
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
        print("set controls");
		Transform[] ctrlsList = new Transform[6];
		for (int i = 0; i < 6; i++)
		{
			ctrlsList[i] = controls.transform.GetChild(i);
		}

		availableCtrlNums.Clear();
		foreach (RectTransform currCtrl in ctrlsList)
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
        if (lastChoice == Controls.Accelerate) { activeControls.Remove(Controls.Accelerate);} //activeControls.Remove(Controls.Rest);

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
		StartCoroutine(RestTime(1.5f));
    }

	public void Accelerate()
	{
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
					lastChoice = Controls.Up;
                    lastChoiceDir = Controls.Up;
                    break;
				case Controls.Down:
					MonkeyButtonPress.i.PressButton(availableCtrlNums[(int)Controls.Down].position.y);
					Down();
					lastChoice = Controls.Down;
                    lastChoiceDir = Controls.Down;
                    break;
				case Controls.Left:
					MonkeyButtonPress.i.PressButton(availableCtrlNums[(int)Controls.Left].position.y);
					Left();
					lastChoice = Controls.Left;
                    lastChoiceDir = Controls.Left;
                    break;
				case Controls.Right:
					MonkeyButtonPress.i.PressButton(availableCtrlNums[(int)Controls.Right].position.y);
					Right();
					lastChoice = Controls.Right;
                    lastChoiceDir = Controls.Right;
                    break;
				case Controls.Rest:
					MonkeyButtonPress.i.PressButton(availableCtrlNums[(int)Controls.Rest].position.y);
					Rest();
					lastChoice = Controls.Rest;
					break;
				case Controls.Accelerate:
					MonkeyButtonPress.i.PressButton(availableCtrlNums[(int)Controls.Accelerate].position.y);
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
