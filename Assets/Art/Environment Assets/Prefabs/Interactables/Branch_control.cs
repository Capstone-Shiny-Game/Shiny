using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch_control : MonoBehaviour
{
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInChildren<Rigidbody>().useGravity = true;
         
        }

    }
}
