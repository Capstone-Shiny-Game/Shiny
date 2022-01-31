using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialRing : MonoBehaviour
{
    public delegate void RingCollision(GameObject ring);
    public static event RingCollision OnRingCollision;

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
