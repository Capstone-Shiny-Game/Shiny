using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_spawn_control : MonoBehaviour
{



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            GetComponentInChildren<Rigidbody>().useGravity = true;
            GetComponentInChildren<ParticleSystem>().Play();
            GetComponent<SphereCollider>().enabled = false;
            gameObject.transform.DetachChildren();
            //
            Destroy(gameObject);
        }

    }
}
