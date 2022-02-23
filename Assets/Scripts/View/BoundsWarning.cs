using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsWarning : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        //canvas = GameObject.Find("CanvasWarning");
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(true);
        }
        Debug.Log("Trigger");
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
    }
}
