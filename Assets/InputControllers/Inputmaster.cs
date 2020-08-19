// GENERATED AUTOMATICALLY FROM 'Assets/InputControllers/Inputmaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Inputmaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputmaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputmaster"",
    ""maps"": [
        {
            ""name"": ""Camera"",
            ""id"": ""85273ce4-38c8-4f15-b531-81a4f5b70113"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""5399ca9e-aed4-4893-87c3-a5d7959306ba"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Height"",
                    ""type"": ""Value"",
                    ""id"": ""7508adbf-2129-48c7-aa72-e8338c4764c4"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""c16c2986-9061-4d16-946a-49a23a9b1db7"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f87835ae-4376-465f-9ddb-a1bf7abeaaca"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0ef75e35-f42d-4b71-89fb-0c3cc07ec1bf"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d9d421bd-8a4c-4896-ab5a-de9bb8857b43"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1aa5439a-f616-4f19-8b27-eb68efe0286e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1ddabe88-44f1-408c-8590-8931bc3028f2"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Height"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menus"",
            ""id"": ""840538c1-4d3e-4055-9f1b-376436ec3d9c"",
            ""actions"": [
                {
                    ""name"": ""Trader"",
                    ""type"": ""Button"",
                    ""id"": ""ef784ed7-7cf8-4df6-b111-4b326f3f74b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scoreboard"",
                    ""type"": ""Button"",
                    ""id"": ""ec33589a-b97b-433d-90d3-367878a885b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""e144e117-603b-42ca-8931-60f8b755f8b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0994f57e-9a1a-416b-9187-5bfc8cb6233a"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Trader"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c15c8a58-a411-4559-81c3-64fb16f4c713"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Scoreboard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac49f08b-3f2a-49b8-a8cb-b1d590628046"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Selection"",
            ""id"": ""57cdfeee-e9e6-4875-91d9-26cc3bcf52d9"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""75fb04d4-c3f1-4b78-82bf-7d7c473381ca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ac6ed957-7689-4ea5-809e-53f9c0a564de"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse and Keyboard"",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse and Keyboard"",
            ""bindingGroup"": ""Mouse and Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_Move = m_Camera.FindAction("Move", throwIfNotFound: true);
        m_Camera_Height = m_Camera.FindAction("Height", throwIfNotFound: true);
        // Menus
        m_Menus = asset.FindActionMap("Menus", throwIfNotFound: true);
        m_Menus_Trader = m_Menus.FindAction("Trader", throwIfNotFound: true);
        m_Menus_Scoreboard = m_Menus.FindAction("Scoreboard", throwIfNotFound: true);
        m_Menus_Menu = m_Menus.FindAction("Menu", throwIfNotFound: true);
        // Selection
        m_Selection = asset.FindActionMap("Selection", throwIfNotFound: true);
        m_Selection_Click = m_Selection.FindAction("Click", throwIfNotFound: true);
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

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_Move;
    private readonly InputAction m_Camera_Height;
    public struct CameraActions
    {
        private @Inputmaster m_Wrapper;
        public CameraActions(@Inputmaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Camera_Move;
        public InputAction @Height => m_Wrapper.m_Camera_Height;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Height.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnHeight;
                @Height.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnHeight;
                @Height.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnHeight;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Height.started += instance.OnHeight;
                @Height.performed += instance.OnHeight;
                @Height.canceled += instance.OnHeight;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);

    // Menus
    private readonly InputActionMap m_Menus;
    private IMenusActions m_MenusActionsCallbackInterface;
    private readonly InputAction m_Menus_Trader;
    private readonly InputAction m_Menus_Scoreboard;
    private readonly InputAction m_Menus_Menu;
    public struct MenusActions
    {
        private @Inputmaster m_Wrapper;
        public MenusActions(@Inputmaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Trader => m_Wrapper.m_Menus_Trader;
        public InputAction @Scoreboard => m_Wrapper.m_Menus_Scoreboard;
        public InputAction @Menu => m_Wrapper.m_Menus_Menu;
        public InputActionMap Get() { return m_Wrapper.m_Menus; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenusActions set) { return set.Get(); }
        public void SetCallbacks(IMenusActions instance)
        {
            if (m_Wrapper.m_MenusActionsCallbackInterface != null)
            {
                @Trader.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnTrader;
                @Trader.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnTrader;
                @Trader.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnTrader;
                @Scoreboard.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnScoreboard;
                @Scoreboard.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnScoreboard;
                @Scoreboard.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnScoreboard;
                @Menu.started -= m_Wrapper.m_MenusActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_MenusActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_MenusActionsCallbackInterface.OnMenu;
            }
            m_Wrapper.m_MenusActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Trader.started += instance.OnTrader;
                @Trader.performed += instance.OnTrader;
                @Trader.canceled += instance.OnTrader;
                @Scoreboard.started += instance.OnScoreboard;
                @Scoreboard.performed += instance.OnScoreboard;
                @Scoreboard.canceled += instance.OnScoreboard;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
            }
        }
    }
    public MenusActions @Menus => new MenusActions(this);

    // Selection
    private readonly InputActionMap m_Selection;
    private ISelectionActions m_SelectionActionsCallbackInterface;
    private readonly InputAction m_Selection_Click;
    public struct SelectionActions
    {
        private @Inputmaster m_Wrapper;
        public SelectionActions(@Inputmaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_Selection_Click;
        public InputActionMap Get() { return m_Wrapper.m_Selection; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SelectionActions set) { return set.Get(); }
        public void SetCallbacks(ISelectionActions instance)
        {
            if (m_Wrapper.m_SelectionActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_SelectionActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_SelectionActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_SelectionActionsCallbackInterface.OnClick;
            }
            m_Wrapper.m_SelectionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }
        }
    }
    public SelectionActions @Selection => new SelectionActions(this);
    private int m_MouseandKeyboardSchemeIndex = -1;
    public InputControlScheme MouseandKeyboardScheme
    {
        get
        {
            if (m_MouseandKeyboardSchemeIndex == -1) m_MouseandKeyboardSchemeIndex = asset.FindControlSchemeIndex("Mouse and Keyboard");
            return asset.controlSchemes[m_MouseandKeyboardSchemeIndex];
        }
    }
    public interface ICameraActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnHeight(InputAction.CallbackContext context);
    }
    public interface IMenusActions
    {
        void OnTrader(InputAction.CallbackContext context);
        void OnScoreboard(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
    }
    public interface ISelectionActions
    {
        void OnClick(InputAction.CallbackContext context);
    }
}
