using UnityEngine;
using System;

public class WalkingController : MonoBehaviour
{
    public float ForwardSpeed = 8;
    public float BackwardsSpeed = 4;
    public float TurningSpeed = 60;

    public bool isIdle = false;
    public event Action WalkedOffEdge;
    public event Action<PlayerController.CrowState> SubstateChanged;

    private PlayerControllerInput PlayerInput;

    private float moveX = 0;
    private float moveY = 0;
    private PlayerController pcontroller;
    void Start()
    {
        Vector3 v = transform.eulerAngles;
        v.x = 0;
        v.z = 0;
        transform.eulerAngles = v;
        pcontroller = GetComponent<PlayerController>();
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
        //disables all flying animations(takeoff, glide, fly)
        pcontroller.AnimationWalkingSuite();
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
        displacement *= (displacement > 0 ? ForwardSpeed : BackwardsSpeed);
        Vector3 newPosition = transform.position + (transform.forward * displacement);
        transform.TestCollision(newPosition, out bool collided, out bool raycastNeeded);
        if (!collided)
        {
            transform.position = newPosition;

            Vector3 ground = transform.position;
            if (raycastNeeded)
                transform.CastGround(out ground, transform.localScale.y / 2);
            else
                ground = transform.FindGround(transform.localScale.y / 2);
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
            SubstateChanged?.Invoke(PlayerController.CrowState.Walking);
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