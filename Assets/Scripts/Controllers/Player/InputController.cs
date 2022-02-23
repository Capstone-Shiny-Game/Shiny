using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//Events
[Serializable]
public class FlightMoveEvent : UnityEvent<float, float> { }
[Serializable]
public class FlightLookEvent : UnityEvent<float, float> { }
[Serializable]
public class StartTouchEvent : UnityEvent<Vector2, float> { }
[Serializable]
public class EndTouchEvent : UnityEvent<Vector2, float> { }
[Serializable]
public class FlightLookResetEvent : UnityEvent { }

[Serializable]
public class FlightBrakeEvent : UnityEvent<bool> { }

[Serializable]
public class FlightBoostEvent : UnityEvent<bool> { }
//[Serializable]
//public class PauseEvent : UnityEvent { }
[Serializable]
public class DropEvent : UnityEvent { }
[Serializable]
public class PickupItemEvent : UnityEvent { }
[Serializable]
public class RotateSelectionEvent : UnityEvent { }
public class InputController : MonoBehaviour
{
    //public GameObject cam;
    //public GameObject player;

    private PlayerControllerInput PlayerInput;
    //private CameraController CamController;
    //private FlightController FlightController;

    //Event Handlers
    public FlightMoveEvent flightMoveHandler;
    public FlightLookEvent flightLookHandler;

    public FlightBrakeEvent flightBrakeHandler;
    public FlightBoostEvent flightBoostHandler;

    //public PauseEvent PauseHandler;
    public DropEvent DropHandler;
    public PickupItemEvent ItemHandler;
    public RotateSelectionEvent RotateSelectionHandler;
    public StartTouchEvent OnStartTouch;
    public EndTouchEvent OnEndTouch;

    public FlightLookResetEvent flightResetLookHandler;
    public TMP_Text test;
    public bool useGyro = true;
    float multiplier = 10.0f;

    private void Awake()
    {
        //CamController = cam.GetComponent<CameraController>();
        //FlightController = player.GetComponent<FlightController>();
        PlayerInput = new PlayerControllerInput();
        test.text = "Please use input";

    }

    public void OnEnable()
    {
        PlayerInput.FlightMap.Enable();
        //subscribe to the events when the following is triggered:
        PlayerInput.FlightMap.Flight.performed += OnFlight;
        PlayerInput.FlightMap.Flight.canceled += OnFlightEnd;

        PlayerInput.FlightMap.Boost.performed += OnBoost;
        PlayerInput.FlightMap.Boost.canceled += OnBoostExit;

        PlayerInput.FlightMap.Brake.performed += OnBrake;
        PlayerInput.FlightMap.Brake.canceled += OnBrakeExit;

        PlayerInput.FlightMap.Look.performed += OnLook;
        PlayerInput.FlightMap.Look.canceled += OnLookExit;

        PlayerInput.FlightMap.ResetLook.performed += OnToggleResetLook;

        PlayerInput.GUIMap.Enable();
        PlayerInput.GUIMap.PauseMenu.performed += OnPause;
        PlayerInput.GUIMap.DropItem.performed += OnDrop;
        PlayerInput.GUIMap.RotateSelection.performed += OnRotateSelection;
        PlayerInput.GUIMap.PrimaryTouch.started += StartTouchPrimary;
        PlayerInput.GUIMap.PrimaryTouch.canceled += EndTouchPrimary;

        PlayerInput.GUIMap.PickupItem.performed += OnPickup;
        if (UnityEngine.InputSystem.Gyroscope.current != null)
            InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
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
        Vector2 moveInput = context.ReadValue<Vector2>();
        flightLookHandler?.Invoke(moveInput.x, moveInput.y);
    }
    private void OnLookExit(InputAction.CallbackContext context)
    {
        flightLookHandler?.Invoke(0.0f, 0.0f);
    }
    private void OnFlight(InputAction.CallbackContext context)
    {
        if (UnityEngine.InputSystem.Gyroscope.current != null && UnityEngine.InputSystem.Gyroscope.current.enabled)
        {
            //Debug.Log("Gyroscope is enabled");
            //Debug.Log(Accelerometer.current.acceleration.ReadValue());
            Vector3 input = context.ReadValue<Vector3>();
            //test.text = "Gyroscope: " + "X: " + input.x + "Y: " + input.y + "Z: " + input.z;
            flightMoveHandler?.Invoke(Mathf.Clamp(-input.z * multiplier, -1.0f, 1.0f), Mathf.Clamp(input.x * multiplier, -1.0f, 1.0f));

        }
        else if (!useGyro || UnityEngine.InputSystem.Gyroscope.current == null)
        {
            test.text = "N/A";

            Vector2 moveInput = context.ReadValue<Vector2>();
            flightMoveHandler?.Invoke(moveInput.x, moveInput.y);
        }

    }
    private void OnFlightEnd(InputAction.CallbackContext context)
    {
        if (UnityEngine.InputSystem.Gyroscope.current == null)
            flightMoveHandler?.Invoke(0.0f, 0.0f);
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




}
