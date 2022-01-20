using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public GameObject Crow;

    private Rigidbody rigidbody;
    private FlightController flightController;
    private MeshRenderer meshRenderer;
    private bool attached;
    private bool inRange;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        flightController = Crow.GetComponent<FlightController>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (attached)
            {
                rigidbody.velocity = Crow.transform.forward * flightController.speed;
                attached = false;
            }
            else if (inRange)
                attached = true;
        }

        rigidbody.useGravity = !attached;

        if (attached)
            transform.position = Crow.transform.position - Crow.transform.forward * 5f;

        transform.localScale = Vector3.one * (inRange && !attached ? 5 : 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Crow)
            inRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Crow)
            inRange = false;
    }
}
