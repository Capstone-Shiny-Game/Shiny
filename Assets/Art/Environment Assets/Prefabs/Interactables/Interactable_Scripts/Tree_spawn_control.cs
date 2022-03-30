using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree_spawn_control : MonoBehaviour
{


    [SerializeField] private GameObject objectToSpawn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            /*GetComponentInChildren<Rigidbody>().useGravity = true;*/

            /* GetComponent<SphereCollider>().enabled = false;
             //*/
            //
            Destroy(gameObject.GetComponentInChildren<MeshRenderer>().gameObject);

            Instantiate(objectToSpawn, transform.position, transform.rotation);

            GetComponentInChildren<ParticleSystem>().Play();
            gameObject.transform.DetachChildren();
            Destroy(gameObject);
        }

    }
}
