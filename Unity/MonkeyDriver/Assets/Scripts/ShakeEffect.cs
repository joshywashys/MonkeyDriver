using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    public float shakeDuration;
    public float shakeDelay;
    public float maxHorizontalOffset;
    public float maxVerticalOffset;

    IEnumerator Shake()
    {
        Vector3 curCamPosition = Camera.main.transform.localPosition;
        Debug.Log("origin: " + Camera.main.transform.localPosition);

        for (int i = 0; i < shakeDuration; i++)
        {
            float horizontalOffset = Random.Range(-maxHorizontalOffset, maxHorizontalOffset);
            float verticalOffset = Random.Range(-maxVerticalOffset, maxVerticalOffset);

            Vector3 offsetDirection = Camera.main.transform.right.normalized * horizontalOffset +
                Camera.main.transform.up.normalized * verticalOffset;

            Camera.main.transform.localPosition += offsetDirection;

            yield return new WaitForSeconds(shakeDelay);
        }

        Camera.main.transform.localPosition = curCamPosition;
    }
    public void shakeCamera()
    {
        StartCoroutine(Shake());
    }
}
