//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/Scripts/PlayerControllerInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControllerInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControllerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControllerInput"",
    ""maps"": [
        {
            ""name"": ""FlightMap"",
            ""id"": ""ea112126-2d30-49b5-ad97-0543cd57b29d"",
            ""actions"": [
                {
                    ""name"": ""Flight"",
                    ""type"": ""Value"",
                    ""id"": ""da0c6682-e16b-44fd-b26a-9292cb47ae44"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""25c75b4b-4825-4c58-ac84-ccc63dd9d17e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ResetLook"",
                    ""type"": ""PassThrough"",
                    ""id"": ""834ae251-2bbc-4d76-b69e-470d984b5fcc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""StartLook"",
                    ""type"": ""Button"",
                    ""id"": ""e2a93101-112c-43aa-8778-2399540d6fe9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""6bc89b0c-aa8c-4094-9be1-f208d2cdb9b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Button"",
                    ""id"": ""2f3d5767-b19f-4b17-86f0-c3fec64b982b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LockCursor"",
                    ""type"": ""Button"",
                    ""id"": ""44d4a4da-759a-4383-89f0-d27fcdc2900d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""walkAction"",
                    ""type"": ""Button"",
                    ""id"": ""19586962-3660-409e-9606-261e0310c17b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""12ae123d-8c6d-43bf-8ea8-95532d84fff3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c99466a3-163d-4a1c-9dc0-905e4c648fa0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""afdc5b6e-7dcc-43cb-afea-5f93056d6257"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""77e056d8-2ec7-48df-a885-d2b1a7aa0d53"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""407e330b-2cb0-43d0-b039-713a57756bfb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5348f0f3-acc1-469c-940c-a8e7758389db"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""eb8a063e-af05-4b6d-aec0-1f5d2626d1ea"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""56ce34ff-21b7-423e-9701-d78880551348"",
                    ""path"": ""<Accelerometer>/acceleration"",
                    ""interactions"": """",
                    ""processors"": ""ScaleVector3(x=8,z=12)"",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6235f709-32c4-40e0-b0bb-1dcb10636a6f"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f1fe46f-e246-46ec-ae36-26eb35840f8e"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfa9be6e-e6b2-4e50-972e-009cb9b35b09"",
                    ""path"": ""<Touchscreen>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cf950a29-0cb3-42d7-84e9-64b798516eca"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResetLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d9d728c-17db-499f-ba46-8257e67848d4"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ResetLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5fb94888-b5b2-4fa0-8afc-3ccc52ac564d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""007930f4-0d87-4384-a0fd-09f1015fefce"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e32f2bb-5c36-4ea3-a990-629cf1d42768"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d521862d-6812-4e4a-8061-1a4f02c5ca71"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d32f931f-0a50-4afe-9b39-960ce01c47c5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a8710e4-5c2b-47f9-833c-b8cbc86e7e6e"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b98e4c5-fc02-4af5-b1ac-f22f8d879295"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7918514a-aeec-413f-b821-04e518a265f5"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""StartLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00aa8695-15cb-476c-a14a-3eabc05ff756"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""walkAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""7ae3b0a8-6ee2-4dc3-8925-6702a72c93fc"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e3def78e-b841-4030-8ba9-cac4e290bc0d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""73634f53-03cf-4e9d-bc32-e47d54a041b7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""61ccb89f-55f3-479a-8834-3979a6ca8d14"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""0f07518a-ab15-46af-bad9-7810a4bffd4a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""54d6da84-7de3-4bd8-bd9e-23be124f0275"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GUIMap"",
            ""id"": ""0813bbc5-9e26-4810-9dfa-abdb60148828"",
            ""actions"": [
                {
                    ""name"": ""PauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""088ffe75-0272-42d9-b31e-2a1b7117d700"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DropItem"",
                    ""type"": ""Button"",
                    ""id"": ""04d33bb1-4b2d-481b-b5eb-0c71faa42ac9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateSelection"",
                    ""type"": ""Button"",
                    ""id"": ""7113ddf8-b1df-495e-a333-de949a7be5cc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PickupItem"",
                    ""type"": ""Button"",
                    ""id"": ""88a677ad-d817-4b1e-8df5-76b5fddc7bb9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PrimaryTouch"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e8edaf48-d4e3-415f-a78c-c7d4c352236d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PrimaryPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ea63a437-bb2a-4abc-9948-b12773f7e975"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e944ccd6-bc1b-4102-aaa3-7b3914a5b873"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90e035dc-8f42-498e-a73a-bcf1246da425"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26a64350-37fd-449a-82ae-706c284662d1"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e76b2935-e368-4514-93aa-f3d8faa8bcf7"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickupItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""979887c6-cbdc-4e69-9595-93dfdfed170c"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryTouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94a35549-376e-4d2e-889e-74b4a8a6f347"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PrimaryPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // FlightMap
        m_FlightMap = asset.FindActionMap("FlightMap", throwIfNotFound: true);
        m_FlightMap_Flight = m_FlightMap.FindAction("Flight", throwIfNotFound: true);
        m_FlightMap_Look = m_FlightMap.FindAction("Look", throwIfNotFound: true);
        m_FlightMap_ResetLook = m_FlightMap.FindAction("ResetLook", throwIfNotFound: true);
        m_FlightMap_StartLook = m_FlightMap.FindAction("StartLook", throwIfNotFound: true);
        m_FlightMap_Boost = m_FlightMap.FindAction("Boost", throwIfNotFound: true);
        m_FlightMap_Brake = m_FlightMap.FindAction("Brake", throwIfNotFound: true);
        m_FlightMap_LockCursor = m_FlightMap.FindAction("LockCursor", throwIfNotFound: true);
        m_FlightMap_walkAction = m_FlightMap.FindAction("walkAction", throwIfNotFound: true);
        m_FlightMap_Walk = m_FlightMap.FindAction("Walk", throwIfNotFound: true);
        // GUIMap
        m_GUIMap = asset.FindActionMap("GUIMap", throwIfNotFound: true);
        m_GUIMap_PauseMenu = m_GUIMap.FindAction("PauseMenu", throwIfNotFound: true);
        m_GUIMap_DropItem = m_GUIMap.FindAction("DropItem", throwIfNotFound: true);
        m_GUIMap_RotateSelection = m_GUIMap.FindAction("RotateSelection", throwIfNotFound: true);
        m_GUIMap_PickupItem = m_GUIMap.FindAction("PickupItem", throwIfNotFound: true);
        m_GUIMap_PrimaryTouch = m_GUIMap.FindAction("PrimaryTouch", throwIfNotFound: true);
        m_GUIMap_PrimaryPosition = m_GUIMap.FindAction("PrimaryPosition", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // FlightMap
    private readonly InputActionMap m_FlightMap;
    private IFlightMapActions m_FlightMapActionsCallbackInterface;
    private readonly InputAction m_FlightMap_Flight;
    private readonly InputAction m_FlightMap_Look;
    private readonly InputAction m_FlightMap_ResetLook;
    private readonly InputAction m_FlightMap_StartLook;
    private readonly InputAction m_FlightMap_Boost;
    private readonly InputAction m_FlightMap_Brake;
    private readonly InputAction m_FlightMap_LockCursor;
    private readonly InputAction m_FlightMap_walkAction;
    private readonly InputAction m_FlightMap_Walk;
    public struct FlightMapActions
    {
        private @PlayerControllerInput m_Wrapper;
        public FlightMapActions(@PlayerControllerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Flight => m_Wrapper.m_FlightMap_Flight;
        public InputAction @Look => m_Wrapper.m_FlightMap_Look;
        public InputAction @ResetLook => m_Wrapper.m_FlightMap_ResetLook;
        public InputAction @StartLook => m_Wrapper.m_FlightMap_StartLook;
        public InputAction @Boost => m_Wrapper.m_FlightMap_Boost;
        public InputAction @Brake => m_Wrapper.m_FlightMap_Brake;
        public InputAction @LockCursor => m_Wrapper.m_FlightMap_LockCursor;
        public InputAction @walkAction => m_Wrapper.m_FlightMap_walkAction;
        public InputAction @Walk => m_Wrapper.m_FlightMap_Walk;
        public InputActionMap Get() { return m_Wrapper.m_FlightMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FlightMapActions set) { return set.Get(); }
        public void SetCallbacks(IFlightMapActions instance)
        {
            if (m_Wrapper.m_FlightMapActionsCallbackInterface != null)
            {
                @Flight.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnFlight;
                @Flight.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnFlight;
                @Flight.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnFlight;
                @Look.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLook;
                @ResetLook.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnResetLook;
                @ResetLook.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnResetLook;
                @ResetLook.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnResetLook;
                @StartLook.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnStartLook;
                @StartLook.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnStartLook;
                @StartLook.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnStartLook;
                @Boost.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBoost;
                @Brake.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBrake;
                @Brake.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBrake;
                @Brake.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBrake;
                @LockCursor.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLockCursor;
                @LockCursor.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLockCursor;
                @LockCursor.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLockCursor;
                @walkAction.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnWalkAction;
                @walkAction.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnWalkAction;
                @walkAction.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnWalkAction;
                @Walk.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnWalk;
            }
            m_Wrapper.m_FlightMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Flight.started += instance.OnFlight;
                @Flight.performed += instance.OnFlight;
                @Flight.canceled += instance.OnFlight;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @ResetLook.started += instance.OnResetLook;
                @ResetLook.performed += instance.OnResetLook;
                @ResetLook.canceled += instance.OnResetLook;
                @StartLook.started += instance.OnStartLook;
                @StartLook.performed += instance.OnStartLook;
                @StartLook.canceled += instance.OnStartLook;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
                @Brake.started += instance.OnBrake;
                @Brake.performed += instance.OnBrake;
                @Brake.canceled += instance.OnBrake;
                @LockCursor.started += instance.OnLockCursor;
                @LockCursor.performed += instance.OnLockCursor;
                @LockCursor.canceled += instance.OnLockCursor;
                @walkAction.started += instance.OnWalkAction;
                @walkAction.performed += instance.OnWalkAction;
                @walkAction.canceled += instance.OnWalkAction;
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
            }
        }
    }
    public FlightMapActions @FlightMap => new FlightMapActions(this);

    // GUIMap
    private readonly InputActionMap m_GUIMap;
    private IGUIMapActions m_GUIMapActionsCallbackInterface;
    private readonly InputAction m_GUIMap_PauseMenu;
    private readonly InputAction m_GUIMap_DropItem;
    private readonly InputAction m_GUIMap_RotateSelection;
    private readonly InputAction m_GUIMap_PickupItem;
    private readonly InputAction m_GUIMap_PrimaryTouch;
    private readonly InputAction m_GUIMap_PrimaryPosition;
    public struct GUIMapActions
    {
        private @PlayerControllerInput m_Wrapper;
        public GUIMapActions(@PlayerControllerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PauseMenu => m_Wrapper.m_GUIMap_PauseMenu;
        public InputAction @DropItem => m_Wrapper.m_GUIMap_DropItem;
        public InputAction @RotateSelection => m_Wrapper.m_GUIMap_RotateSelection;
        public InputAction @PickupItem => m_Wrapper.m_GUIMap_PickupItem;
        public InputAction @PrimaryTouch => m_Wrapper.m_GUIMap_PrimaryTouch;
        public InputAction @PrimaryPosition => m_Wrapper.m_GUIMap_PrimaryPosition;
        public InputActionMap Get() { return m_Wrapper.m_GUIMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GUIMapActions set) { return set.Get(); }
        public void SetCallbacks(IGUIMapActions instance)
        {
            if (m_Wrapper.m_GUIMapActionsCallbackInterface != null)
            {
                @PauseMenu.started -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPauseMenu;
                @PauseMenu.performed -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPauseMenu;
                @PauseMenu.canceled -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPauseMenu;
                @DropItem.started -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnDropItem;
                @DropItem.performed -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnDropItem;
                @DropItem.canceled -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnDropItem;
                @RotateSelection.started -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnRotateSelection;
                @RotateSelection.performed -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnRotateSelection;
                @RotateSelection.canceled -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnRotateSelection;
                @PickupItem.started -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPickupItem;
                @PickupItem.performed -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPickupItem;
                @PickupItem.canceled -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPickupItem;
                @PrimaryTouch.started -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPrimaryTouch;
                @PrimaryTouch.performed -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPrimaryTouch;
                @PrimaryTouch.canceled -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPrimaryTouch;
                @PrimaryPosition.started -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPrimaryPosition;
                @PrimaryPosition.performed -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPrimaryPosition;
                @PrimaryPosition.canceled -= m_Wrapper.m_GUIMapActionsCallbackInterface.OnPrimaryPosition;
            }
            m_Wrapper.m_GUIMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PauseMenu.started += instance.OnPauseMenu;
                @PauseMenu.performed += instance.OnPauseMenu;
                @PauseMenu.canceled += instance.OnPauseMenu;
                @DropItem.started += instance.OnDropItem;
                @DropItem.performed += instance.OnDropItem;
                @DropItem.canceled += instance.OnDropItem;
                @RotateSelection.started += instance.OnRotateSelection;
                @RotateSelection.performed += instance.OnRotateSelection;
                @RotateSelection.canceled += instance.OnRotateSelection;
                @PickupItem.started += instance.OnPickupItem;
                @PickupItem.performed += instance.OnPickupItem;
                @PickupItem.canceled += instance.OnPickupItem;
                @PrimaryTouch.started += instance.OnPrimaryTouch;
                @PrimaryTouch.performed += instance.OnPrimaryTouch;
                @PrimaryTouch.canceled += instance.OnPrimaryTouch;
                @PrimaryPosition.started += instance.OnPrimaryPosition;
                @PrimaryPosition.performed += instance.OnPrimaryPosition;
                @PrimaryPosition.canceled += instance.OnPrimaryPosition;
            }
        }
    }
    public GUIMapActions @GUIMap => new GUIMapActions(this);
    public interface IFlightMapActions
    {
        void OnFlight(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnResetLook(InputAction.CallbackContext context);
        void OnStartLook(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
        void OnLockCursor(InputAction.CallbackContext context);
        void OnWalkAction(InputAction.CallbackContext context);
        void OnWalk(InputAction.CallbackContext context);
    }
    public interface IGUIMapActions
    {
        void OnPauseMenu(InputAction.CallbackContext context);
        void OnDropItem(InputAction.CallbackContext context);
        void OnRotateSelection(InputAction.CallbackContext context);
        void OnPickupItem(InputAction.CallbackContext context);
        void OnPrimaryTouch(InputAction.CallbackContext context);
        void OnPrimaryPosition(InputAction.CallbackContext context);
    }
}
