using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager i = null;
	public AudioSource monkeySource, screamSource, busSource;
	public AudioClip monkeyLaugh;
	public AudioClip[] passengerScreams;
	void Start()
	{
		if (i == null)
		{
			i = this;
		}
		StartCoroutine(MonkeyChatter());
	}

	public void PlayScreams(float delay)
	{
		if (!screamSource.isPlaying)
        {
			StartCoroutine(Screaming(delay));
		}

	}
	public void PlayBrake()
	{

		busSource.Play();

	}
	IEnumerator MonkeyChatter()
	{
		while (true)
		{
			monkeySource.PlayOneShot(monkeyLaugh);
			yield return new WaitForSeconds(10.0f);
		}
	}

	IEnumerator Screaming(float delay)
	{
		yield return new WaitForSeconds(delay);
		screamSource.pitch = Random.Range(0.9f, 1.4f);
		screamSource.PlayOneShot(passengerScreams[Random.Range(0, passengerScreams.Length)]);
	}
   
}
