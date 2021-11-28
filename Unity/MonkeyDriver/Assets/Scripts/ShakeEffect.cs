using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeEffect : MonoBehaviour
{
    private float shakeMagnitude = 1.0f;
    private float shakeDuration = 0.0f;
    private float timer = 0f;
    private Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Camera.main.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator screenShake(float duration)
    {
        Vector3 curCamTransform = Camera.main.transform.position;
        for (int i =0; i <duration; i++)
        {
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void shakeCamera(float duration)
    {
        StartCoroutine(screenShake(duration));
    }
}
