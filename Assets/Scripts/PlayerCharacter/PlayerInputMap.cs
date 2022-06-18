// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerCharacter/PlayerInputMap.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputMap : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputMap"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""e3850eac-9bd1-4db1-9fbf-4b39a59d95df"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""1d2cd803-6094-48c2-b611-a24ab17f6ecc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CursorMove"",
                    ""type"": ""Value"",
                    ""id"": ""b8689c86-84d3-4835-bde6-d6a25af42131"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone"",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""cbcf9da5-ca49-4068-ac72-5b5b95fcbac6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""162a0794-3318-45c6-84f6-d427cafe6684"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4e128de2-b357-4805-8e7d-c96fac023e7f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""124538f6-cb26-43bb-bcd7-fb82b304b569"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2cc83d6a-a708-4faf-9c1f-3b47ab4facaa"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""a0b127bf-9ff6-4822-b560-bc485b5b5603"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a4d37ea6-3331-4436-a43f-363276d74dbb"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""23d053fa-44ac-4e0f-9094-73af8c5f8ac0"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""07eb300e-1422-4c48-8956-3d1cda68332f"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7850347f-26e5-48f8-9337-463fe850eba3"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DPad"",
                    ""id"": ""4671988c-34af-4233-918f-8aaf4faf9ef6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e9a486bc-8c81-4251-bf09-b2e518530cd9"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7e57263f-f5f3-4cc7-9316-33fd19b41400"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""7f528d82-4e62-419d-8b95-700fdb71b648"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c32f5959-025f-427d-95bf-96896439eb79"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""99c979c2-36f6-444f-a9bd-ba30b4700962"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Actions"",
            ""id"": ""d334e291-8373-458d-a42b-e586b1fa431b"",
            ""actions"": [
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""0b62a5a4-aa5f-4564-9037-d28dd2d7a7a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shockwave"",
                    ""type"": ""Button"",
                    ""id"": ""6ff9566f-e9da-4a14-a167-19fd2bed5150"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shield"",
                    ""type"": ""Button"",
                    ""id"": ""189153cf-a92c-42a5-9c0b-8e3444db2a67"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""0fca2a35-73fd-4d1e-9dd9-021d87c43cbb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThrowR"",
                    ""type"": ""Button"",
                    ""id"": ""b91c4f73-72f4-4b61-b5a5-a7f22e17fa00"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThrowL"",
                    ""type"": ""Button"",
                    ""id"": ""4f12486a-1561-4b19-956f-8ade4f9a3c3c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bc031dd0-1301-4c44-abcf-7ac534c9ae67"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c5a118b-2382-4131-b701-8ec1168a68e5"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1f1fe03-522f-495e-8bef-4f9bc54fdf8d"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Shockwave"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1f8bc084-2989-43dc-aaef-7bad94eb5d60"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Shield"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82f1a4c9-0ecb-4c3c-8a93-4da138d31f30"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64c3c1f1-bf67-4daf-a3d8-5d623823638a"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c91295b-3e06-4f6c-a07a-b5b8e8a3bd2e"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ThrowR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d75cd486-9ebf-4fe7-9639-2cd8898d795b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c02dcc44-44c6-4c6f-a266-19daf30a1ec8"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowR"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a281e303-a51d-4a54-8a5f-820754afb721"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ThrowL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea686eef-32af-4c83-b361-cf87f6dcd86c"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14b0840a-c837-4d4c-bb68-7013304070cf"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": []
        }
    ]
}");
        // Movement
        m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
        m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
        m_Movement_CursorMove = m_Movement.FindAction("CursorMove", throwIfNotFound: true);
        // Actions
        m_Actions = asset.FindActionMap("Actions", throwIfNotFound: true);
        m_Actions_LightAttack = m_Actions.FindAction("LightAttack", throwIfNotFound: true);
        m_Actions_Shockwave = m_Actions.FindAction("Shockwave", throwIfNotFound: true);
        m_Actions_Shield = m_Actions.FindAction("Shield", throwIfNotFound: true);
        m_Actions_Dodge = m_Actions.FindAction("Dodge", throwIfNotFound: true);
        m_Actions_ThrowR = m_Actions.FindAction("ThrowR", throwIfNotFound: true);
        m_Actions_ThrowL = m_Actions.FindAction("ThrowL", throwIfNotFound: true);
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

    // Movement
    private readonly InputActionMap m_Movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_Movement_Move;
    private readonly InputAction m_Movement_CursorMove;
    public struct MovementActions
    {
        private @PlayerInputMap m_Wrapper;
        public MovementActions(@PlayerInputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Movement_Move;
        public InputAction @CursorMove => m_Wrapper.m_Movement_CursorMove;
        public InputActionMap Get() { return m_Wrapper.m_Movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @CursorMove.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnCursorMove;
                @CursorMove.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnCursorMove;
                @CursorMove.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnCursorMove;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @CursorMove.started += instance.OnCursorMove;
                @CursorMove.performed += instance.OnCursorMove;
                @CursorMove.canceled += instance.OnCursorMove;
            }
        }
    }
    public MovementActions @Movement => new MovementActions(this);

    // Actions
    private readonly InputActionMap m_Actions;
    private IActionsActions m_ActionsActionsCallbackInterface;
    private readonly InputAction m_Actions_LightAttack;
    private readonly InputAction m_Actions_Shockwave;
    private readonly InputAction m_Actions_Shield;
    private readonly InputAction m_Actions_Dodge;
    private readonly InputAction m_Actions_ThrowR;
    private readonly InputAction m_Actions_ThrowL;
    public struct ActionsActions
    {
        private @PlayerInputMap m_Wrapper;
        public ActionsActions(@PlayerInputMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @LightAttack => m_Wrapper.m_Actions_LightAttack;
        public InputAction @Shockwave => m_Wrapper.m_Actions_Shockwave;
        public InputAction @Shield => m_Wrapper.m_Actions_Shield;
        public InputAction @Dodge => m_Wrapper.m_Actions_Dodge;
        public InputAction @ThrowR => m_Wrapper.m_Actions_ThrowR;
        public InputAction @ThrowL => m_Wrapper.m_Actions_ThrowL;
        public InputActionMap Get() { return m_Wrapper.m_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
        public void SetCallbacks(IActionsActions instance)
        {
            if (m_Wrapper.m_ActionsActionsCallbackInterface != null)
            {
                @LightAttack.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnLightAttack;
                @LightAttack.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnLightAttack;
                @LightAttack.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnLightAttack;
                @Shockwave.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShockwave;
                @Shockwave.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShockwave;
                @Shockwave.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShockwave;
                @Shield.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShield;
                @Shield.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShield;
                @Shield.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShield;
                @Dodge.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDodge;
                @Dodge.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDodge;
                @Dodge.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDodge;
                @ThrowR.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnThrowR;
                @ThrowR.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnThrowR;
                @ThrowR.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnThrowR;
                @ThrowL.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnThrowL;
                @ThrowL.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnThrowL;
                @ThrowL.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnThrowL;
            }
            m_Wrapper.m_ActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
                @Shockwave.started += instance.OnShockwave;
                @Shockwave.performed += instance.OnShockwave;
                @Shockwave.canceled += instance.OnShockwave;
                @Shield.started += instance.OnShield;
                @Shield.performed += instance.OnShield;
                @Shield.canceled += instance.OnShield;
                @Dodge.started += instance.OnDodge;
                @Dodge.performed += instance.OnDodge;
                @Dodge.canceled += instance.OnDodge;
                @ThrowR.started += instance.OnThrowR;
                @ThrowR.performed += instance.OnThrowR;
                @ThrowR.canceled += instance.OnThrowR;
                @ThrowL.started += instance.OnThrowL;
                @ThrowL.performed += instance.OnThrowL;
                @ThrowL.canceled += instance.OnThrowL;
            }
        }
    }
    public ActionsActions @Actions => new ActionsActions(this);
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnCursorMove(InputAction.CallbackContext context);
    }
    public interface IActionsActions
    {
        void OnLightAttack(InputAction.CallbackContext context);
        void OnShockwave(InputAction.CallbackContext context);
        void OnShield(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnThrowR(InputAction.CallbackContext context);
        void OnThrowL(InputAction.CallbackContext context);
    }
}
