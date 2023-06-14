//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.3
//     from Assets/Runtime/Scripts/Entities/Player/Input/PlayerInputActions.inputactions
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

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""f31db121-9416-41aa-b8b8-57e5836e3b9d"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""b1fceffa-aa7d-4211-9654-845e84931379"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PrimaryButton"",
                    ""type"": ""Button"",
                    ""id"": ""24741057-6e67-4e02-b2a5-69ffdb264336"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondaryButton"",
                    ""type"": ""Button"",
                    ""id"": ""3f27d20d-4ac8-4687-98fa-d1e18f173d80"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""1293f93e-2c24-445c-aced-7e8d2c1ccdfd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""RightStickRotation"",
                    ""type"": ""Value"",
                    ""id"": ""2e171dd3-7db4-4512-bd88-82d3167b38e9"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Swap1"",
                    ""type"": ""Button"",
                    ""id"": ""e719619b-4281-4de9-a519-f5f145c8806d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Swap2"",
                    ""type"": ""Button"",
                    ""id"": ""7ebba6e2-8f0f-433c-bac5-d8e6e28b0ca6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Swap3"",
                    ""type"": ""Button"",
                    ""id"": ""7430cdec-ea6d-4d77-87b0-7f561eb0661f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""f0317a60-48e5-4c2d-95cd-eb81dfa2e0cf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""89ff6d6d-6349-4f4a-8780-3c2dccd8175b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD Vector3"",
                    ""id"": ""f67277ed-6c60-4046-bf8c-6cde83f491a8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""2a058782-862e-44c5-b78e-ff4aa6713283"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""10adb0b8-bf2f-48cc-93f3-ebeb4fb88261"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""7c04c5c8-b891-4398-9659-025034c210a2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""8d918ad0-3a89-4a12-baee-b02b287b7602"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""73ee8327-1fb1-4818-8830-6de45a828a9d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b2f2c3cc-e6e0-4f11-ac7d-dc41bf76374c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""PrimaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18dcd812-5eb9-4626-8fc9-ebe3093c3d93"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PrimaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d26c2fd8-6f43-4a25-ab91-15502c4fc07a"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RightStickRotation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6851a06-f51e-4d90-9f83-6bb3073ca663"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""SecondaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""787c605b-a01e-4fac-b03c-35c76e2ae6d5"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SecondaryButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7af56c1e-9f4f-4796-bd66-e9e808fd5474"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe94cfd5-fc26-4bae-94bd-3bb666de97a4"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9fea4353-627c-4190-988a-bc313b23f13b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7142297f-173b-4002-b03d-dfb3f1a7d36c"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swap3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1d47acb-d521-46b7-946b-09af4d66bbae"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8479103-cedd-4302-b03d-a05e1fc1dfa4"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Movement = m_Gameplay.FindAction("Movement", throwIfNotFound: true);
        m_Gameplay_PrimaryButton = m_Gameplay.FindAction("PrimaryButton", throwIfNotFound: true);
        m_Gameplay_SecondaryButton = m_Gameplay.FindAction("SecondaryButton", throwIfNotFound: true);
        m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
        m_Gameplay_RightStickRotation = m_Gameplay.FindAction("RightStickRotation", throwIfNotFound: true);
        m_Gameplay_Swap1 = m_Gameplay.FindAction("Swap1", throwIfNotFound: true);
        m_Gameplay_Swap2 = m_Gameplay.FindAction("Swap2", throwIfNotFound: true);
        m_Gameplay_Swap3 = m_Gameplay.FindAction("Swap3", throwIfNotFound: true);
        m_Gameplay_Sprint = m_Gameplay.FindAction("Sprint", throwIfNotFound: true);
        m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Movement;
    private readonly InputAction m_Gameplay_PrimaryButton;
    private readonly InputAction m_Gameplay_SecondaryButton;
    private readonly InputAction m_Gameplay_Look;
    private readonly InputAction m_Gameplay_RightStickRotation;
    private readonly InputAction m_Gameplay_Swap1;
    private readonly InputAction m_Gameplay_Swap2;
    private readonly InputAction m_Gameplay_Swap3;
    private readonly InputAction m_Gameplay_Sprint;
    private readonly InputAction m_Gameplay_Interact;
    public struct GameplayActions
    {
        private @PlayerInputActions m_Wrapper;
        public GameplayActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Gameplay_Movement;
        public InputAction @PrimaryButton => m_Wrapper.m_Gameplay_PrimaryButton;
        public InputAction @SecondaryButton => m_Wrapper.m_Gameplay_SecondaryButton;
        public InputAction @Look => m_Wrapper.m_Gameplay_Look;
        public InputAction @RightStickRotation => m_Wrapper.m_Gameplay_RightStickRotation;
        public InputAction @Swap1 => m_Wrapper.m_Gameplay_Swap1;
        public InputAction @Swap2 => m_Wrapper.m_Gameplay_Swap2;
        public InputAction @Swap3 => m_Wrapper.m_Gameplay_Swap3;
        public InputAction @Sprint => m_Wrapper.m_Gameplay_Sprint;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                @PrimaryButton.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryButton;
                @PrimaryButton.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryButton;
                @PrimaryButton.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPrimaryButton;
                @SecondaryButton.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryButton;
                @SecondaryButton.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryButton;
                @SecondaryButton.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSecondaryButton;
                @Look.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLook;
                @RightStickRotation.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightStickRotation;
                @RightStickRotation.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightStickRotation;
                @RightStickRotation.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightStickRotation;
                @Swap1.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap1;
                @Swap1.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap1;
                @Swap1.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap1;
                @Swap2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap2;
                @Swap2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap2;
                @Swap2.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap2;
                @Swap3.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap3;
                @Swap3.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap3;
                @Swap3.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwap3;
                @Sprint.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSprint;
                @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @PrimaryButton.started += instance.OnPrimaryButton;
                @PrimaryButton.performed += instance.OnPrimaryButton;
                @PrimaryButton.canceled += instance.OnPrimaryButton;
                @SecondaryButton.started += instance.OnSecondaryButton;
                @SecondaryButton.performed += instance.OnSecondaryButton;
                @SecondaryButton.canceled += instance.OnSecondaryButton;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @RightStickRotation.started += instance.OnRightStickRotation;
                @RightStickRotation.performed += instance.OnRightStickRotation;
                @RightStickRotation.canceled += instance.OnRightStickRotation;
                @Swap1.started += instance.OnSwap1;
                @Swap1.performed += instance.OnSwap1;
                @Swap1.canceled += instance.OnSwap1;
                @Swap2.started += instance.OnSwap2;
                @Swap2.performed += instance.OnSwap2;
                @Swap2.canceled += instance.OnSwap2;
                @Swap3.started += instance.OnSwap3;
                @Swap3.performed += instance.OnSwap3;
                @Swap3.canceled += instance.OnSwap3;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnPrimaryButton(InputAction.CallbackContext context);
        void OnSecondaryButton(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnRightStickRotation(InputAction.CallbackContext context);
        void OnSwap1(InputAction.CallbackContext context);
        void OnSwap2(InputAction.CallbackContext context);
        void OnSwap3(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
