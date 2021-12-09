using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyButtonPress : MonoBehaviour
{
	public static MonkeyButtonPress i = null;
    public GameObject MonkeyFinger;
    public Transform HandHolder;

    private void Start()
    {
		if (i == null)
		{
			i = this;
		}
	}
    public void PressButton(float yPos)
    {
        Instantiate(MonkeyFinger, new Vector3(0, yPos, 0), Quaternion.identity, HandHolder);
    }
}
