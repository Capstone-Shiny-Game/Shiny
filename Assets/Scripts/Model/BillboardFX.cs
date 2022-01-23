using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFX : MonoBehaviour
{
    public float VanishDistance = 100;
    void Update()
    {
        if(Vector3.Distance(transform.position, Camera.main.transform.position) < VanishDistance)
        {
            gameObject.GetComponent<Canvas>().enabled = true;
            transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.back, Camera.main.transform.rotation * -Vector3.down);
        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }

    }
}