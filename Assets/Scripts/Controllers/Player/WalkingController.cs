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
    private Crow crow;
    private PlayerController pcontroller;
    private void Awake()
    {
        crow = GetComponent<Crow>();

    }
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
        crow.Model.transform.localPosition = new Vector3(0.0f, -0.52f, 0.0f);
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
        Vector3 oldPosition = transform.position;
        transform.position += transform.forward * displacement;
        Vector3 ground;
        if (transform.RaycastNeeded())
        {
            if (!transform.CastGround(out ground, transform.localScale.y / 2))
            {
                transform.position = oldPosition;
                return;
            }
        }
        else
            ground = transform.FindGround(transform.localScale.y / 2);
        float dY = transform.position.y - ground.y;
        if (dY > transform.localScale.y)
            WalkedOffEdge?.Invoke();
        else
            transform.position = ground;

        //check if no input from player.
        CheckIdle();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!this.enabled || collision.collider is TerrainCollider || collision.gameObject.CompareTag("Terrain") || collision.collider.isTrigger)
            return;
        else
        {
            float displacement = 1.8f;
            if (moveY <= 0)
                transform.position += transform.forward * displacement;
            else
                transform.position -= transform.forward * displacement;
        }
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