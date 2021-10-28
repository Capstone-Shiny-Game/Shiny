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

    private void OnEnable()
    {
        // register the function for the collision event
        //NPCInteraction.OnPlayerCollided += SetFixedPosition;
    }

    private void OnDisable()
    {
        // degregister the function
        //NPCInteraction.OnPlayerCollided -= SetFixedPosition;
    }

    private void StartFlight() => flightController.enabled = true;

    private void StopFlight() => flightController.enabled = false;

    private void SetFixedPosition(Vector3 position) => this.transform.position = position;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BOUNCE");
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Vector3 norm = collision.GetContact(0).normal;
            flightController.SetBounce(norm);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ring") && !flightController.isBoost)
        {
            Debug.Log("RING2");
            Transform targetRing = other.gameObject.transform;
            flightController.SetTargetRing(targetRing);
            //transform.LookAt(targetRing);
            flightController.StartBoost();
        }
        else if (other.CompareTag("Terrain"))
        {
            transform.position = new Vector3(
                transform.position.x, transform.position.y + 5f, transform.position.z);

            Debug.Log("AAAAA");
            flightController.StartSlow();
        }
        else if (other.CompareTag("NPC"))
        {
            Vector3 npcFront = other.gameObject.transform.position + other.transform.forward * 3.0f;

            StopFlight();
            SetFixedPosition(npcFront);
        }
    }
}
