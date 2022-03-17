using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Pick_controller : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    private void Start()
    {
       // gameObject.GetComponentInChildren<Rigidbody>().useGravity = false;
      //  gameObject.GetComponentInChildren<SphereCollider>().enabled = false;
      //  gameObject.GetComponentInChildren<BoxCollider>().enabled = false;

    }
    public void OnPick()
    {
        
       // gameObject.GetComponentInChildren<SphereCollider>().enabled = true;
      //  gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
      //  gameObject.GetComponentInChildren<Rigidbody>().AddForce(0,15,0);
      //  gameObject.GetComponentInChildren<Rigidbody>().useGravity = true;
        
        
        Destroy(gameObject.GetComponentInChildren<Canvas>().gameObject);
        Destroy(gameObject.GetComponentInChildren<MeshRenderer>().gameObject);
        
        Instantiate(objectToSpawn, transform.position, transform.rotation);
        //gameObject.transform.DetachChildren();
        Destroy(gameObject);
    }



}
