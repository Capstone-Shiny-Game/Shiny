// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControllerInput.inputactions'

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
            ""id"": ""a053fb5f-2425-4647-8988-9a16a8170227"",
            ""actions"": [
                {
                    ""name"": ""Flight"",
                    ""type"": ""Value"",
                    ""id"": ""41696a77-9cba-432b-b51f-5182550e4e03"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""be7428bb-b38c-41d1-95c9-41550f6659d0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleFirstPerson"",
                    ""type"": ""Button"",
                    ""id"": ""bfc54581-d016-4562-ad9b-e158d29ffeea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""c4d5a96c-aad8-4132-b475-6805ec72ee37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Value"",
                    ""id"": ""3a4a991a-bf42-4265-91a5-1fa49268bea2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LockCursor"",
                    ""type"": ""Button"",
                    ""id"": ""f0f36d86-4d0c-4094-819f-fd1a08cb2512"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9c76ea18-ef1d-4a25-9d2d-88c0598227cd"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8543264a-b976-418b-ab13-a259bea5451f"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleFirstPerson"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a471abc-2690-462a-bc75-b2dbe19d09ac"",
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
                    ""id"": ""0765267f-2f2c-48b7-81d6-c96b45e6a99d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8ca559e-977b-4f13-8f48-f313fb2db9e7"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9399807f-bd63-4ada-bac3-8efc1770b258"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""178a8056-ee0d-4a6f-beeb-4004c31f314e"",
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
                    ""id"": ""641b13f6-9e98-4c79-90b2-42dbe36e94d1"",
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
                    ""id"": ""8fd7d325-3f5c-4676-970a-93fe92a448dc"",
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
                    ""id"": ""d9aa308a-8884-48ea-a937-fa60919a4f8c"",
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
                    ""id"": ""dca10acd-569a-4380-a3da-a85bd5b18013"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
