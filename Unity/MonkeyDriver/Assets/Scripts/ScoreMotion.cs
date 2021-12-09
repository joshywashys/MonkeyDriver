using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMotion : MonoBehaviour
{
    public float startYpos, endYPos;

    // Start is called before the first frame update
    public void ScoreMove()
    {
		gameObject.SetActive(true);
		StartCoroutine(MoveScore());
    }

	IEnumerator MoveScore()
	{
		yield return new WaitForSeconds(0.01f);
		float moveTime = 0.0f;
		float moveDuration = 0.5f;

		while (moveTime < moveDuration)
		{
			moveTime += Time.deltaTime;

			transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(startYpos, endYPos, moveTime / moveDuration), transform.localPosition.z);
			yield return null;
		}
		if (moveTime >= moveDuration)
		{
			transform.localPosition = Vector3.zero;

			gameObject.SetActive(false);
		}
	}
}
