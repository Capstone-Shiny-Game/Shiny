using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Pick_controller : MonoBehaviour
{

    private void Start()
    {
        gameObject.GetComponentInChildren<Rigidbody>().useGravity = false;
        gameObject.GetComponentInChildren<SphereCollider>().enabled = false;
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

    }
    public void OnPick()
    {
        
        gameObject.GetComponentInChildren<SphereCollider>().enabled = true;
        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;
        gameObject.GetComponentInChildren<Rigidbody>().AddForce(0,15,0);
        gameObject.GetComponentInChildren<Rigidbody>().useGravity = true;
        
        
        Destroy(gameObject.GetComponentInChildren<Canvas>().gameObject);
        gameObject.transform.DetachChildren();
        Destroy(gameObject);
    }



}
