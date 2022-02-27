using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch_child_control : MonoBehaviour
{
    [SerializeField] Collider coll;

    private void Start()
    {
        GetComponent<Rigidbody>().useGravity = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Terrain"))
        {
            coll.enabled = true;
        }
        
    }
}
