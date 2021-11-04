using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickupable : MonoBehaviour
{
    public GameObject Crow;

    private Rigidbody rigidbody;
    private FlightController flightController;
    private bool attached;
    private bool inRange;
    private InputAction pickupAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/E");


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        flightController = Crow.GetComponent<FlightController>();
    }

    private void Update()
    {

        rigidbody.useGravity = !attached;

        if (attached)
            transform.position = Crow.transform.position - Crow.transform.forward * 5f;

        transform.localScale = Vector3.one * (inRange && !attached ? 5 : 2);
    }

    private void OnEnable()
    {
        // register the function for the collision event
        //NPCInteraction.OnPlayerCollided += SetFixedPosition;
        pickupAction.performed += ctx => pickup();
        pickupAction.Enable();
    }

    private void OnDisable()
    {
        // degregister the function
        //NPCInteraction.OnPlayerCollided -= SetFixedPosition;
        pickupAction.Disable();
    }

    private void pickup()
    {
        if (attached)
        {
            rigidbody.velocity = Crow.transform.forward * flightController.speed;
            attached = false;
        }
        else if (inRange)
            attached = true;
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
