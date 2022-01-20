using UnityEngine;

// TODO (Elaine) : We need to decide exactly how the pickup-but-not-add-to-inventory behavior should work.
// TODO (Ella) : Replace this code with something more appropriate for long term use per above. #110

public class Pickupable : MonoBehaviour
{
    public GameObject Crow;

    private new Rigidbody rigidbody;
    private PlayerController playerController;
    private FlightController flightController;

    private bool attached;
    private bool inRange;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerController = Crow.GetComponent<PlayerController>();
        flightController = Crow.GetComponent<FlightController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
