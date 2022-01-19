using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float Speed;
    public float movementRange;
    void StartRespawn()
    {
        if (this.gameObject.activeInHierarchy)//prevents exception when game is closed
        {
            StartCoroutine("Respawn");
        }
    }
    public float UpDown() {
        return movementRange * Mathf.Sin(Speed * (Time.time));
    }
}
