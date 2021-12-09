using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyButtonPress : MonoBehaviour
{
	public static MonkeyButtonPress i = null;
    public GameObject MonkeyFinger;
    public Transform HandHolder;
    private GameObject finger;
    public float offset;

    private void Start()
    {
		if (i == null)
		{
			i = this;
		}
	}
    public void PressButton(float yPos)
    {
        finger = Instantiate(MonkeyFinger);
        finger.transform.SetParent(HandHolder, true);
        finger.transform.localPosition = Vector3.zero;
        finger.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        finger.transform.position = new Vector3(0, yPos - offset, finger.transform.position.z);
    }
}
