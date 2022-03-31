using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerchPosition : MonoBehaviour
{
    public Transform crowPerchPos;
    public Transform lookAt;
    public GameObject vCam;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FlightController controller = other.GetComponent<FlightController>();
            controller.InvokeLandPerch(crowPerchPos, lookAt);
            if (vCam)
                vCam.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && vCam)
            vCam.SetActive(false);
    }
}
