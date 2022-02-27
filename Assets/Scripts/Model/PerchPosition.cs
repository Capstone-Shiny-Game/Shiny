using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerchPosition : MonoBehaviour
{
    public Transform crowPerchPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FlightController controller = other.GetComponent<FlightController>();
            controller.InvokeLandPerch(crowPerchPos);
            
        }
    }
}
