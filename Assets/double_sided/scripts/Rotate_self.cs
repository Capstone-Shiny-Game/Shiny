﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate_self : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    float num = 0;
    // Update is called once per frame
    void Update()
    {
        num += Time.deltaTime * 40;
        if (num > 360)
            num = 0;
        this.transform.rotation = Quaternion.Euler(0, num, 0);
    }
}
