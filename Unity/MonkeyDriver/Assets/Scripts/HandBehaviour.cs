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
		yield return new WaitForSeconds(0.01f);
		float pushTime = 0.0f;
		float pushDuration = 0.5f;

		float firstYPos = transform.position.y;

		while (pushTime < pushDuration)
		{
			pushTime += Time.deltaTime;

			transform.position = new Vector3(Mathf.Lerp(transform.position.x, endXPos, pushTime / pushDuration), firstYPos, transform.position.z);
			yield return null;
		}
		if (pushTime >= pushDuration)
		{
			Destroy(gameObject);
		}
	}
}
