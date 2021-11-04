using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using static PlayerControllerInput;

public class WalkingController : MonoBehaviour, IFlightMapActions
{
    public float ForwardSpeed = 8;
    public float BackwardsSpeed = 4;
    public float TurningSpeed = 60;
    public float HeightOffset = 0.5f;

    private GroundDetector ground;
    private bool useTerrain;
    private PlayerControllerInput PlayerInput;

    private float moveX = 0;
    private float moveY = 0;

    void Start()
    {
        Vector3 v = transform.eulerAngles;
        v.x = 0;
        v.z = 0;
        transform.eulerAngles = v;

        // TODO (Ella) *hisses at these sins*
        //useTerrain = SceneManager.GetActiveScene().name == "WalkingTest";
        //if (!useTerrain)
        useTerrain = false;
        ground = GetComponent<GroundDetector>() ?? gameObject.AddComponent<GroundDetector>();
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
        displacement *= displacement >= 0 ? ForwardSpeed : BackwardsSpeed;
        transform.position += transform.forward * displacement;
        Vector3 pos = transform.position;
        // TODO (Ella): will we ever have more than one terrain
        if (useTerrain)
            pos.y = Terrain.activeTerrains.Select(t => t.SampleHeight(pos)).Max() + HeightOffset;
        else if (ground.FindGround() is Vector3 groundPos)
            pos.y = groundPos.y + HeightOffset;
        transform.position = pos;
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