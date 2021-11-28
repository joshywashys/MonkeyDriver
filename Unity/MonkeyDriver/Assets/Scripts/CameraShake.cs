using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeMagnitude = 1.0f;
    private float shakeDuration = 0.0f;
    private float timer = 0f;
    private Vector3 originPos;

    private void Update()
    {
        if (shakeDuration < 0)
        {
            float randx = Random.Range(-0.5f, 0.5f);
            float randy = Random.Range(-0.5f, 0.5f);
            transform.localPosition = new Vector3(originPos.x + randx, originPos.y + randy, transform.position.z);
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0;
            transform.position = originPos;
        }
    }

    public void camShake(float duration)
    {
        originPos = transform.localPosition;
        shakeDuration = duration;
    }
}
