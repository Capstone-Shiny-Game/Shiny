using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch_control : MonoBehaviour
{
    [SerializeField] GameObject prefabObj;
    SphereCollider coll;

    private void Start()
    {
        
    }

    private void Awake()
    {
        prefabObj.GetComponent<ItemWorld>().enabled = false;
        coll = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInChildren<Rigidbody>().useGravity = true;
            coll.radius = .2f;
        }

        if (other.CompareTag("Terrain"))
        {
            Debug.Log("trigger");
            prefabObj.GetComponent<ItemWorld>().enabled = true;
        }
    }
}
