using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break_log : MonoBehaviour
{
    [SerializeField] GameObject break1;
    [SerializeField] GameObject break2;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Vector3 spawnLocation = Vector3.zero;

    public void OnButtonInteraction()
    {
        Destroy(GetComponentInChildren<Canvas>().gameObject);
        GetComponentInChildren<ParticleSystem>().Play();
        break1.GetComponent<Rigidbody>().AddForce(Vector3.up);
        break2.GetComponent<Rigidbody>().AddForce(Vector3.up);
        break1.GetComponent<Rigidbody>().isKinematic = false;
        break2.GetComponent<Rigidbody>().isKinematic = false;

        Instantiate(objectToSpawn, transform.position + spawnLocation, transform.rotation);

        Destroy(this.gameObject, 5);


    }

   

}
