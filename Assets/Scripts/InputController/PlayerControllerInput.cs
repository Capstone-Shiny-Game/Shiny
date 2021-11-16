// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/InputController/PlayerControllerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControllerInput : IInputActionCollection, IDisposable
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
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""25c75b4b-4825-4c58-ac84-ccc63dd9d17e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleFirstPerson"",
                    ""type"": ""Button"",
                    ""id"": ""834ae251-2bbc-4d76-b69e-470d984b5fcc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""6bc89b0c-aa8c-4094-9be1-f208d2cdb9b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Value"",
                    ""id"": ""2f3d5767-b19f-4b17-86f0-c3fec64b982b"",
                    ""expectedControlType"": ""Integer"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LockCursor"",
                    ""type"": ""Button"",
                    ""id"": ""44d4a4da-759a-4383-89f0-d27fcdc2900d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
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
                    ""id"": ""cf950a29-0cb3-42d7-84e9-64b798516eca"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleFirstPerson"",
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
                    ""action"": ""ToggleFirstPerson"",
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
        m_FlightMap_ToggleFirstPerson = m_FlightMap.FindAction("ToggleFirstPerson", throwIfNotFound: true);
        m_FlightMap_Boost = m_FlightMap.FindAction("Boost", throwIfNotFound: true);
        m_FlightMap_Brake = m_FlightMap.FindAction("Brake", throwIfNotFound: true);
        m_FlightMap_LockCursor = m_FlightMap.FindAction("LockCursor", throwIfNotFound: true);
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

    // FlightMap
    private readonly InputActionMap m_FlightMap;
    private IFlightMapActions m_FlightMapActionsCallbackInterface;
    private readonly InputAction m_FlightMap_Flight;
    private readonly InputAction m_FlightMap_Look;
    private readonly InputAction m_FlightMap_ToggleFirstPerson;
    private readonly InputAction m_FlightMap_Boost;
    private readonly InputAction m_FlightMap_Brake;
    private readonly InputAction m_FlightMap_LockCursor;
    public struct FlightMapActions
    {
        private @PlayerControllerInput m_Wrapper;
        public FlightMapActions(@PlayerControllerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Flight => m_Wrapper.m_FlightMap_Flight;
        public InputAction @Look => m_Wrapper.m_FlightMap_Look;
        public InputAction @ToggleFirstPerson => m_Wrapper.m_FlightMap_ToggleFirstPerson;
        public InputAction @Boost => m_Wrapper.m_FlightMap_Boost;
        public InputAction @Brake => m_Wrapper.m_FlightMap_Brake;
        public InputAction @LockCursor => m_Wrapper.m_FlightMap_LockCursor;
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
                @ToggleFirstPerson.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnToggleFirstPerson;
                @ToggleFirstPerson.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnToggleFirstPerson;
                @ToggleFirstPerson.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnToggleFirstPerson;
                @Boost.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBoost;
                @Brake.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBrake;
                @Brake.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBrake;
                @Brake.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnBrake;
                @LockCursor.started -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLockCursor;
                @LockCursor.performed -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLockCursor;
                @LockCursor.canceled -= m_Wrapper.m_FlightMapActionsCallbackInterface.OnLockCursor;
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
                @ToggleFirstPerson.started += instance.OnToggleFirstPerson;
                @ToggleFirstPerson.performed += instance.OnToggleFirstPerson;
                @ToggleFirstPerson.canceled += instance.OnToggleFirstPerson;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
                @Brake.started += instance.OnBrake;
                @Brake.performed += instance.OnBrake;
                @Brake.canceled += instance.OnBrake;
                @LockCursor.started += instance.OnLockCursor;
                @LockCursor.performed += instance.OnLockCursor;
                @LockCursor.canceled += instance.OnLockCursor;
            }
        }
    }
    public FlightMapActions @FlightMap => new FlightMapActions(this);
    public interface IFlightMapActions
    {
        void OnFlight(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnToggleFirstPerson(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
        void OnLockCursor(InputAction.CallbackContext context);
    }
}
