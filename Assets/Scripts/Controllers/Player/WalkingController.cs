using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControllerInput;
using System;
using System.Linq;

public class WalkingController : MonoBehaviour, IFlightMapActions
{
    public float ForwardSpeed = 8;
    public float BackwardsSpeed = 4;
    public float SplashingSpeed = 0.1f;
    public float TurningSpeed = 60;

    public bool Splashing = false;
    public bool isIdle = false;
    public event Action WalkedOffEdge;
    public event Action<PlayerController.CrowState> SubstateChanged;

    private PlayerControllerInput PlayerInput;

    private float moveX = 0;
    private float moveY = 0;

    void Start()
    {
        Vector3 v = transform.eulerAngles;
        v.x = 0;
        v.z = 0;
        transform.eulerAngles = v;
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        if (PlayerInput == null)
        {
            PlayerInput = new PlayerControllerInput();
            PlayerInput.FlightMap.SetCallbacks(this);
        }
        isIdle = true;
        SubstateChanged?.Invoke(PlayerController.CrowState.Idle);
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
        bool prevSplashing = Splashing;
        bool collided = false;
        bool needsRaycast = false;
        foreach (Collider collider in colliders)
        {
            if (collider.isTrigger || collider is TerrainCollider || collider.CompareTag("Player"))
                continue;
            else if (collider.CompareTag("Terrain") || collider.CompareTag("Water"))
                needsRaycast = true;
            else
            {
                collided = true;
                break;
            }
        }
        if (!collided)
        {
            transform.position = newPosition;
            Vector3 ground = transform.FindGround(transform.localScale.y / 2);
            if (needsRaycast
                && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, transform.localScale.magnitude * 2)
                && (hit.transform.CompareTag("Terrain") || hit.transform.CompareTag("Water")))
            {
                Vector3 candidateGround = hit.transform.position;
                candidateGround.y += transform.localScale.y / 2;
                if (candidateGround.y > ground.y)
                {
                    ground = candidateGround;
                    Splashing = hit.transform.CompareTag("Water");
                }
                else
                    Splashing = false;
            }
            else
                Splashing = false;
            if (Splashing != prevSplashing)
                SubstateChanged?.Invoke(Splashing ? PlayerController.CrowState.Splashing : PlayerController.CrowState.Walking);
            float dY = transform.position.y - ground.y;
            if (dY > transform.localScale.y)
                WalkedOffEdge?.Invoke();
            else
                transform.position = ground;
        }
        //check if no input from player.
        CheckIdle();
    }

    public void CheckIdle()
    {
        if ((moveX != 0 || moveY != 0) && isIdle)
        {
            isIdle = false;
            SubstateChanged?.Invoke(Splashing ? PlayerController.CrowState.Splashing : PlayerController.CrowState.Walking);
        }
        else if (moveX == 0 && moveY == 0 && !isIdle)
        {
            isIdle = true;
            SubstateChanged?.Invoke(PlayerController.CrowState.Idle);
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