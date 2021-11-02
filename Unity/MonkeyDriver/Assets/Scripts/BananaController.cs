using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaController : MonoBehaviour
{
    public static BananaController i = null;
    public static int curBananas = 0;

    void Start()
    {
        if (i == null)
        {
            i = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addBananas(int numBananas)
    {
        curBananas += numBananas;
    }

    public void useBanana()
    {
        curBananas -= 1;
    }
}
