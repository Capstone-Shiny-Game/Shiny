using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private FlightController flightController;
    private WalkingController walkingController;

    private void Start()
    {
        flightController = GetComponent<FlightController>();
        walkingController = GetComponent<WalkingController>();
        StartFlight();
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

    private void StartFlight() {
        flightController.enabled = true;
        walkingController.enabled = false;
    }

    private void StopFlight() {
        flightController.enabled = false;
        walkingController.enabled = true;
    }

    private void SetFixedPosition(Vector3 position) => this.transform.position = position;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BOUNCE");
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Vector3 norm = collision.GetContact(0).normal;
            StartCoroutine(flightController.BounceOnCollision(norm));
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
            StartCoroutine(flightController.Boost());

        }
        else if (other.CompareTag("Terrain"))
        {
            // TODO (Ella) : This is evil. 
            if (SceneManager.GetActiveScene().name == "WalkingTest")
            {
                StopFlight();
            }
            else
            {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y + 5f,
                    transform.position.z);

                StartCoroutine(flightController.Slow());
            }
        }
        else if (other.CompareTag("NPC"))
        {
            Vector3 npcFront = other.gameObject.transform.position + other.transform.forward * 3.0f;

            StopFlight();
            SetFixedPosition(npcFront);
        }
    }
}
