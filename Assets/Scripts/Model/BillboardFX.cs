using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFX : MonoBehaviour
{
    public float VanishDistance = 100;
    public GameObject dialUI;

    void Update()
    {
        if(Time.timeScale == 0 || (dialUI != null && dialUI.activeInHierarchy))
        {
            gameObject.GetComponent<Canvas>().enabled = false;
            return;
        }

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