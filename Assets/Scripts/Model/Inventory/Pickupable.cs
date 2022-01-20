using UnityEngine;
using UnityEngine.InputSystem;

// TODO (Elaine) : We need to decide exactly how the pickup-but-not-add-to-inventory behavior should work.
// TODO (Ella) : Replace this code with something more appropriate for long term use per above. #110

public class Pickupable : MonoBehaviour
{
    public GameObject Crow;

    private new Rigidbody rigidbody;
    private PlayerController playerController;
    private FlightController flightController;
    private InputAction grabAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/E"); // TODO : This seems very wrong. Angelique would know what to do better.
    private Vector3 smallScale;
    private Vector3 largeScale;
    private bool attached;
    private bool inRange;

    private void OnEnable()
    {
        grabAction.performed += ctx => TryGrabOrRelease();
        grabAction.Enable();
    }

    private void OnDisable()
    {
        grabAction.Disable();
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerController = Crow.GetComponent<PlayerController>();
        flightController = Crow.GetComponent<FlightController>();
        smallScale = transform.localScale;
        largeScale = smallScale * 2;
    }

    private void Update()
    {
        rigidbody.useGravity = !attached;

        if (attached)
            transform.position = Crow.transform.position - Crow.transform.forward * 5f;

        // TODO : we should probably do something more clever than just embiggening to indicate that the player needs to interact with this
        transform.localScale = inRange && !attached ? largeScale : smallScale;
    }

    private void TryGrabOrRelease()
    {
        if (attached)
        {
            // TODO : get speed when walking or splashing
            float speed = playerController.state == PlayerController.CrowState.Flying ? flightController.speed : 0;
            rigidbody.velocity = Crow.transform.forward * speed;
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
