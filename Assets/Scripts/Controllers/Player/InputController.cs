using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

//Events
[Serializable]
public class FlightMoveEvent : UnityEvent<float, float> { }
[Serializable]
public class FlightWalkEvent : UnityEvent<float, float> { }
[Serializable]
public class FlightLookEvent : UnityEvent<float, float> { }
[Serializable]
public class StartTouchEvent : UnityEvent<Vector2, float> { }
[Serializable]
public class EndTouchEvent : UnityEvent<Vector2, float> { }
[Serializable]
public class FlightLookResetEvent : UnityEvent { }
[Serializable]
public class FlightStartLookEvent : UnityEvent { }
[Serializable]
public class FlightBrakeEvent : UnityEvent<bool> { }

[Serializable]
public class FlightBoostEvent : UnityEvent<bool> { }
//[Serializable]
//public class PauseEvent : UnityEvent { }
[Serializable]
public class DropEvent : UnityEvent { }

[Serializable]
public class FlightSwapToWalkEvent : UnityEvent { }

[Serializable]
public class PickupItemEvent : UnityEvent { }

[Serializable]
public class RotateSelectionEvent : UnityEvent { }

[Serializable]
public class CawEvent : UnityEvent { }

public class InputController : MonoBehaviour
{
    //public GameObject cam;
    //public GameObject player;

    private PlayerControllerInput PlayerInput;
    //private CameraController CamController;
    //private FlightController FlightController;

    //Event Handlers
    public FlightMoveEvent flightMoveHandler;
    public FlightWalkEvent flightWalkHandler;

    public FlightLookEvent flightLookHandler;
    public FlightStartLookEvent flightStartLookHandler;
    private bool other = false;

    public FlightBrakeEvent flightBrakeHandler;
    public FlightBoostEvent flightBoostHandler;
    public FlightSwapToWalkEvent flightSwapHandler;

    //public PauseEvent PauseHandler;
    public DropEvent DropHandler;
    public PickupItemEvent ItemHandler;
    public RotateSelectionEvent RotateSelectionHandler;
    public StartTouchEvent OnStartTouch;
    public EndTouchEvent OnEndTouch;

    public FlightLookResetEvent flightResetLookHandler;

    public CawEvent CawEventHandler;

    public TMP_Text test;
    public static bool AccelerometerAvailable { get; private set; } = false;
    public bool UseAccelerometer { get; private set; }
    private bool canLook = false;
    public bool menuOpen = false;
    private bool isMoving = false;
    private const float deadZone = 0.5f;
    private float ZBias = float.NaN;

    private void Awake()
    {
        //CamController = cam.GetComponent<CameraController>();
        //FlightController = player.GetComponent<FlightController>();
        PlayerInput = new PlayerControllerInput();
        test.text = "Please use input";
    }

    public void OnEnable()
    {
        TouchSimulation.Enable();

        PlayerInput.FlightMap.Enable();
        // Subscribe to the events when the following is triggered:
        PlayerInput.FlightMap.walkAction.performed += OnFlightSwap;

        PlayerInput.FlightMap.Flight.performed += OnFlight;
        PlayerInput.FlightMap.Flight.canceled += OnFlightEnd;

        PlayerInput.FlightMap.Walk.performed += OnWalk;
        PlayerInput.FlightMap.Walk.canceled += OnWalkEnd;

        PlayerInput.FlightMap.Boost.performed += OnBoost;
        PlayerInput.FlightMap.Boost.canceled += OnBoostExit;

        PlayerInput.FlightMap.Brake.performed += OnBrake;
        PlayerInput.FlightMap.Brake.canceled += OnBrakeExit;

        PlayerInput.FlightMap.Look.performed += OnLook;
        PlayerInput.FlightMap.Look.canceled += OnLookExit;
        PlayerInput.FlightMap.StartLook.performed += StartLook;
        PlayerInput.FlightMap.StartLook.canceled += EndLook;

        PlayerInput.FlightMap.ResetLook.performed += OnToggleResetLook;

        PlayerInput.GUIMap.Enable();
        PlayerInput.GUIMap.PauseMenu.performed += OnPause;
        PlayerInput.GUIMap.DropItem.performed += OnDrop;
        PlayerInput.GUIMap.RotateSelection.performed += OnRotateSelection;
        PlayerInput.GUIMap.PrimaryTouch.started += StartTouchPrimary;
        PlayerInput.GUIMap.PrimaryTouch.canceled += EndTouchPrimary;
        PlayerInput.GUIMap.PickupItem.performed += OnPickup;
        PlayerInput.GUIMap.Caw.performed += OnCaw;

        if (Accelerometer.current != null)
        {
            AccelerometerAvailable = true;
            InputSystem.EnableDevice(Accelerometer.current);
        }

        Settings.OnSettingsChanged += SettingsChanged;
        SettingsChanged(null, null);
    }

    private void SettingsChanged(object sender, EventArgs e)
    {
        UseAccelerometer = !Settings.settingsData.disableAccelerometer;
    }

    private void OnWalk(InputAction.CallbackContext obj)
    {
        
        isMoving = true;
        Vector2 moveInput = obj.ReadValue<Vector2>();
        flightWalkHandler?.Invoke(moveInput.x, moveInput.y);
    }

    private void OnWalkEnd(InputAction.CallbackContext obj)
    {
        flightWalkHandler?.Invoke(0.0f, 0.0f);
        isMoving = false;
    }

    private void OnFlightSwap(InputAction.CallbackContext context)
    {
        if (menuOpen)
            return;

        flightSwapHandler?.Invoke();
        //disable all controls
        if (other == true)
        {
            StartCoroutine(DisableControlsForCutscene());
            other = false;
        }
        else
        {
            other = true;
        }
    }
    private IEnumerator DisableControlsForCutscene()
    {
        PlayerInput.FlightMap.Disable();
        PlayerInput.GUIMap.Disable();
        yield return new WaitForSeconds(1.1f);
        PlayerInput.FlightMap.Enable();
        PlayerInput.GUIMap.Enable();
    }
    private void EndLook(InputAction.CallbackContext context)
    {
        canLook = false;
    }

    private void StartLook(InputAction.CallbackContext context)
    {
        canLook = true;

    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        OnStartTouch?.Invoke(PrimaryPosition(), (float)context.startTime);
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        OnEndTouch?.Invoke(PrimaryPosition(), (float)context.time);
    }
    public Vector2 PrimaryPosition()
    {
        return PlayerInput.GUIMap.PrimaryPosition.ReadValue<Vector2>();
    }

    public void OnDisable()
    {
        PlayerInput.FlightMap.Disable();
        PlayerInput.GUIMap.Disable();
    }

    private void OnToggleResetLook(InputAction.CallbackContext context)
    {
        flightResetLookHandler?.Invoke();
    }
    private void OnLook(InputAction.CallbackContext context)
    {
        bool notMovingFromJoystick = !isMoving && (AccelerometerAvailable && UseAccelerometer);
        if (canLook && notMovingFromJoystick && !menuOpen)
        {
            // Touchscreen t = TouchSimulation.instance.simulatedTouchscreen;
            Vector2 moveInput = context.ReadValue<Vector2>();
            //Vector2 moveInputTouch = t.primaryTouch.delta.ReadValue();
            //Debug.Log(moveInput);
            // Debug.Log(moveInputTouch);
            // test.text += "\n" + moveInput.x + moveInput.y;
            flightLookHandler?.Invoke(moveInput.x, moveInput.y);
        }

    }
    private void OnLookExit(InputAction.CallbackContext context)
    {
        flightLookHandler?.Invoke(0.0f, 0.0f);
    }

    private void OnFlight(InputAction.CallbackContext context)
    {
        isMoving = true;

        if (UseAccelerometer && AccelerometerAvailable)
        {
            Vector3 input = context.ReadValue<Vector3>();
            if (float.IsNaN(ZBias))
                ZBias = input.z;

            float x = input.x;
            float z = input.z - ZBias;

            test.text = $"Accelerometer: X: {x},\nZ: {input.z} - {ZBias} = {z}";

            if (x < 0)
            {
                x += deadZone;
                x = Mathf.Clamp(x, -1, 0);
            }
            else
            {
                x -= deadZone;
                x = Mathf.Clamp(x, 0, 1);
            }

            if (z < 0)
            {
                z += deadZone;
                z = Mathf.Clamp(z, -1, 0);
            }
            else
            {
                z -= deadZone;
                z = Mathf.Clamp(z, 0, 1);
            }
            flightMoveHandler?.Invoke(x, z);
        }
        else
        {
            test.text = "N/A";
            Vector2 moveInput = context.ReadValue<Vector2>();

            flightMoveHandler?.Invoke(moveInput.x, moveInput.y);
        }
    }

    public void ResetZBias()
    {
        ZBias = float.NaN;
    }

    private void OnFlightEnd(InputAction.CallbackContext context)
    {
        //if (!AccelerometerAvailable || !UseAccelerometer)
            flightMoveHandler?.Invoke(0.0f, 0.0f);

        isMoving = false;
    }
    private void OnBrake(InputAction.CallbackContext context)
    {
        flightBrakeHandler?.Invoke(true);
    }
    private void OnBrakeExit(InputAction.CallbackContext context)
    {
        flightBrakeHandler?.Invoke(false);
    }
    private void OnBoost(InputAction.CallbackContext context)
    {
        flightBoostHandler?.Invoke(true);

    }
    private void OnBoostExit(InputAction.CallbackContext context)
    {
        flightBoostHandler?.Invoke(false);
    }


    private void OnPause(InputAction.CallbackContext context)
    {
        AkSoundEngine.PostEvent("menuExit", gameObject);
        AkSoundEngine.PostEvent("pause", gameObject);
        MenuManager.instance.SwitchMenu(MenuManager.instance.lastOpenedPauseMenu);
        //PauseHandler?.Invoke();
    }

    private void OnDrop(InputAction.CallbackContext context)
    {
        DropHandler?.Invoke();
    }

    private void OnPickup(InputAction.CallbackContext obj)
    {
        ItemHandler?.Invoke();
    }

    private void OnRotateSelection(InputAction.CallbackContext context)
    {
        RotateSelectionHandler?.Invoke();
    }

    private void OnCaw(InputAction.CallbackContext context)
    {
        CawEventHandler?.Invoke();
    }

}
