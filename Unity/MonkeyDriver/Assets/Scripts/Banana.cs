using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    Vector3 startPos;
    public GameObject banana;
    void Awake()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
        if (BananaController.curBananas > 0)
        {
            Instantiate(banana, startPos, Quaternion.identity);
        }
        BananaController.i.useBanana();
        //drag banana
    }
}
