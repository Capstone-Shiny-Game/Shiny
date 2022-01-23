using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControllerInput;
using System;
using System.Linq;

public class WalkingController : MonoBehaviour, IFlightMapActions
{
    public float ForwardSpeed = 8;
    public float BackwardsSpeed = 4;
    public float SplashingSpeed = 4; // TODO (Ella) : Only allow crow to slowly walk in shallow water
    public float TurningSpeed = 60;

    public bool Splashing = false;

    public event Action WalkedOffEdge;

    private GroundDetector groundDetector;
    private PlayerControllerInput PlayerInput;

    private float moveX = 0;
    private float moveY = 0;

    void Start()
    {
        Vector3 v = transform.eulerAngles;
        v.x = 0;
        v.z = 0;
        transform.eulerAngles = v;

        groundDetector = GetComponent<GroundDetector>();
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        if (PlayerInput == null)
        {
            PlayerInput = new PlayerControllerInput();
            PlayerInput.FlightMap.SetCallbacks(this);
        }

        PlayerInput.FlightMap.Enable();
    }

    void OnDisable()
    {
        PlayerInput.FlightMap.Disable();
    }

    void Update()
    {
        transform.Rotate(0, moveX * Time.deltaTime * TurningSpeed, 0, Space.World);
        float displacement = moveY * Time.deltaTime;
        if (Splashing)
            displacement *= SplashingSpeed;
        else if (displacement >= 0)
            displacement *= ForwardSpeed;
        else
            displacement *= BackwardsSpeed;
        Vector3 newPosition = transform.position + (transform.forward * displacement);
        Collider[] colliders = Physics.OverlapSphere(newPosition, transform.localScale.magnitude);
        bool collided = colliders.Any(collider =>
        {
            if (collider.isTrigger)
                return false;

            string tag = collider.transform.tag;
            if (tag == "Player" || tag == "Terrain" || tag == "Water")
                return false;

            return true;
        });
        if (!collided)
        {
            transform.position = newPosition;
            if (groundDetector.FindGround(out Vector3 groundPos, out Splashing))
            {
                // TODO : Update the crow's `state` to Walking or Splashing as appropriate walking from ground to water or vice versa
                float dY = transform.position.y - groundPos.y;
                if (dY > transform.localScale.y)
                    WalkedOffEdge?.Invoke();
                else
                    transform.position = groundPos;
            }
        }
    }

    public void OnFlight(InputAction.CallbackContext context)
    {
        moveX = context.ReadValue<Vector2>().x;
        moveY = context.ReadValue<Vector2>().y;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // UNUSED
    }

    public void OnToggleFirstPerson(InputAction.CallbackContext context)
    {
        // UNUSED
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        // UNUSED
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        // UNUSED
    }

    public void OnLockCursor(InputAction.CallbackContext context)
    {
        // UNUSED
    }
}