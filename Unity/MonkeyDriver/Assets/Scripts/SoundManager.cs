using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource monkeySource;
    public AudioClip monkeyLaugh;
    void Start()
    {
        StartCoroutine(MonkeyChatter());
    }

    IEnumerator MonkeyChatter()
    {
        while (true)
        {
            monkeySource.PlayOneShot(monkeyLaugh);
            yield return new WaitForSeconds(10.0f);
        }
    }
}
