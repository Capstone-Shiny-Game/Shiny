using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch_child_control : MonoBehaviour
{
    [SerializeField] Collider coll;
    private void OnCollisionEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            coll.enabled = true;
        }
        
    }
}
