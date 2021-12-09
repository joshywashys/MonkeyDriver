using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehaviour : MonoBehaviour
{
	public float startXPos, endXPos;

	void Start()
    {
		transform.position = new Vector3(startXPos, transform.position.y, transform.position.z);
        StartCoroutine(PushButton());
    }

	IEnumerator PushButton()
	{
		float pushTime = 0.0f;
		float pushDuration = 0.5f;

		while (pushTime < pushDuration)
		{
			pushTime += Time.deltaTime;

			transform.position = new Vector3(Mathf.Lerp(startXPos, endXPos, pushTime / pushDuration), transform.position.y, transform.position.z);
			yield return null;
		}
		if (pushTime >= pushDuration)
		{
			Destroy(gameObject);
		}
	}
}
