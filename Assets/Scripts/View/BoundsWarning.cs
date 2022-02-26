using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsWarning : MonoBehaviour
{
    public GameObject FlyTooHighImages;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FlyTooHighImages.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FlyTooHighImages.SetActive(false);
        }
    }

}
