using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private FlightController flightController;

    private void Start()
    {
        flightController = GetComponent<FlightController>();
    }

    public void SetFixedPosition(Vector3 position)
    {
        flightController.enabled = false;
        this.transform.position = position;
    }

    private void OnEnable()
    {
        // register the function for the collision event
        NPCInteraction.OnPlayerCollided += SetFixedPosition;
    }

    private void OnDisable()
    {
        // degregister the function
        NPCInteraction.OnPlayerCollided -= SetFixedPosition;
    }
}
