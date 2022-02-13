using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerControllerInput;
using System;
using System.Linq;

public class WalkingController : MonoBehaviour
{
    public float ForwardSpeed = 8;
    public float BackwardsSpeed = 4;
    public float SplashingSpeed = 4; // TODO (Ella) : Only allow crow to slowly walk in shallow water
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
        if (moveY == 0)
        {
            CheckIdle();
            return;
        }
        float displacement = moveY * Time.deltaTime;
        if (Splashing)
            displacement *= SplashingSpeed;
        else if (displacement >= 0)
            displacement *= ForwardSpeed;
        else
            displacement *= BackwardsSpeed;
        Vector3 newPosition = transform.position + (transform.forward * displacement);
        transform.TestCollision(newPosition, out bool collided, out bool raycastNeeded);
        if (!collided)
        {
            bool oldSplashing = Splashing;
            transform.position = newPosition;

            Vector3 ground = transform.position;
            Debug.Log(raycastNeeded);
            if (raycastNeeded)
                transform.CastGround(out ground, out Splashing, transform.localScale.y / 2);
            else
            {
                ground = transform.FindGround(transform.localScale.y / 2);
                Splashing = false;
            }

            if (Splashing != oldSplashing)
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
    public void SetWalkXY(float x, float y)
    {
        moveX = x;
        moveY = y;
    }

}