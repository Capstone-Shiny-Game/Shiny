using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

//Events
[Serializable]
public class FlightMoveEvent : UnityEvent<float, float> { }
[Serializable]
public class FlightLookEvent : UnityEvent<float, float> { }
[Serializable]
public class FlightTogglePOVEvent : UnityEvent { }

[Serializable]
public class FlightBrakeEvent : UnityEvent<bool> { }

[Serializable]
public class FlightBoostEvent : UnityEvent<bool> { }
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

    public FlightTogglePOVEvent flightTogglePOVHandler;

    private void Awake()
    {
        //CamController = cam.GetComponent<CameraController>();
        //FlightController = player.GetComponent<FlightController>();
        PlayerInput = new PlayerControllerInput();

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

        PlayerInput.FlightMap.ToggleFirstPerson.performed += OnToggleFirstPerson;

    }

    public void OnDisable()
    {
        PlayerInput.FlightMap.Disable();
    }

    private void OnToggleFirstPerson(InputAction.CallbackContext context)
    {
        flightTogglePOVHandler.Invoke();
    }
    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        flightLookHandler.Invoke(moveInput.x, moveInput.y);
    }
    private void OnLookExit(InputAction.CallbackContext context)
    {
        flightLookHandler.Invoke(0.0f, 0.0f);
    }
    private void OnFlight(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        flightMoveHandler.Invoke(moveInput.x, moveInput.y);
    }
    private void OnFlightEnd(InputAction.CallbackContext context)
    {
        flightMoveHandler.Invoke(0.0f, 0.0f);
    }
    private void OnBrake(InputAction.CallbackContext context)
    {
        flightBrakeHandler.Invoke(true);
    }
    private void OnBrakeExit(InputAction.CallbackContext context)
    {
       flightBrakeHandler.Invoke(false);
    }
    private void OnBoost(InputAction.CallbackContext context)
    {
        flightBoostHandler.Invoke(true);

    }
    private void OnBoostExit(InputAction.CallbackContext context)
    {
        flightBoostHandler.Invoke(false);
    }




}
