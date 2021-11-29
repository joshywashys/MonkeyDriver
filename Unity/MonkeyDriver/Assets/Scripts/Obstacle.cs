using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    bool isCrushed = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bus" && !isCrushed)
        {
            //if (hasPlow)
            //{
            //    //ScoreManager.i.subScore(50);
            //}
            //else
            //{
            //    GetComponent<CameraShake>().camShake(2);
            //    ScoreManager.i.subScore(50);
            //}

            GetComponent<AudioSource>().Play();
            FindObjectOfType<ShakeEffect>().shakeCamera();
            ScoreManager.i.subScore(50);
            Destroy(gameObject, 0.7f);
            GetComponent<SpriteRenderer>().forceRenderingOff = true;
            isCrushed = true;
        }
    }
}
