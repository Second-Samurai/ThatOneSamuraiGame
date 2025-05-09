using System;
using ThatOneSamuraiGame.Scripts.Input.Gameplay;
using ThatOneSamuraiGame.Scripts.Input.Menu;
using ThatOneSamuraiGame.Scripts.Input.Rewind;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThatOneSamuraiGame.Scripts.Input
{
    
    /// <summary>
    /// Manages the input possession between scenes.
    /// </summary>
    public class InputManager : MonoBehaviour, IInputManager
    {
        
        #region - - - - - - - Fields - - - - - - -

        public static InputManager Instance;

        private IInputControl m_GameplayInputControl;
        private IInputControl m_MenuInputControl;
        private IInputControl m_RewindInputControl;

        [SerializeField]
        private PlayerInput m_PlayerInput;

        private ControlDevices m_SelectedDevice;
        private InputActionMapType m_ActiveInputActionMap;
        
        #endregion Fields

        #region - - - - - - - Lifecycle Methods - - - - - - -
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            this.m_PlayerInput ??= this.GetComponent<PlayerInput>();
            
            // Temp: Set the default control device to be keyboard and mouse
            this.m_SelectedDevice = ControlDevices.KeyboardAndMouse;
        }

        #endregion Lifecycle Methods

        #region - - - - - - Methods - - - - - -

        public bool IsMembersValid()
        {
            return GameValidator.NotNull(this.m_GameplayInputControl, nameof(this.m_GameplayInputControl))
                   && GameValidator.NotNull(this.m_MenuInputControl, nameof(this.m_MenuInputControl))
                   && GameValidator.NotNull(this.m_RewindInputControl, nameof(this.m_RewindInputControl));
        }
        
        void IInputManager.ConfigureMenuInputControl()
        {
            IMenuInputControl _MenuInputControl;
            switch (this.m_SelectedDevice)
            {
                case ControlDevices.KeyboardAndMouse:
                    _MenuInputControl = GameManager.instance.GameState.SessionUser
                                            .AddComponent<MenuMouseAndKeyboardInputControl>();
                    break;
                case ControlDevices.Gamepad:
                    throw new NotImplementedException();
                default:
                    _MenuInputControl = GameManager.instance.GameState.SessionUser
                                            .AddComponent<MenuMouseAndKeyboardInputControl>();
                    break;
            };
            
            _MenuInputControl.ConfigureInputEvents(this.m_PlayerInput);
            this.m_MenuInputControl = _MenuInputControl;
        }
                
        bool IInputManager.DoesGameplayInputControlExist()
            => this.m_GameplayInputControl == null;

        void IInputManager.DisableActiveInputControl()
        {
            switch (this.m_ActiveInputActionMap)
            {
                case InputActionMapType.Gamplay:
                    this.m_GameplayInputControl.DisableInput();
                    break;
                case InputActionMapType.Menu:
                    this.m_MenuInputControl.DisableInput();
                    break;
                case InputActionMapType.Rewind:
                    this.m_RewindInputControl.DisableInput();
                    break;
                default:
                    Debug.LogWarning("Either input control is not set or input mapping type not specified.");
                    break;
            }
        }

        void IInputManager.EnableActiveInputControl()
        {
            switch (this.m_ActiveInputActionMap)
            {
                case InputActionMapType.Gamplay:
                    this.m_GameplayInputControl.EnableInput();
                    break;
                case InputActionMapType.Menu:
                    this.m_MenuInputControl.EnableInput();
                    break;
                case InputActionMapType.Rewind:
                    this.m_RewindInputControl.EnableInput();
                    break;
                default:
                    Debug.LogWarning("Either input control is not set or input mapping type not specified.");
                    break;
            }
        }
        
        // -------------------------------------
        // Player object possession methods
        // -------------------------------------

        void IInputManager.PossesPlayerObject(GameObject playerObject)
        {
            // Possess gameplay and rewind input control
            IGameplayInputControl _GameplayInputControl;
            IRewindInputControl _RewindInputControl;
            switch (this.m_SelectedDevice)
            {
                case ControlDevices.KeyboardAndMouse:
                    _GameplayInputControl = playerObject.AddComponent<GameplayMouseAndKeyboardInputControl>();
                    _RewindInputControl = playerObject.AddComponent<RewindMouseAndKeyboardInputControl>();
                    break;
                case ControlDevices.Gamepad:
                    throw new NotImplementedException();
                default:
                    _GameplayInputControl = playerObject.AddComponent<GameplayMouseAndKeyboardInputControl>();
                    _RewindInputControl = playerObject.AddComponent<RewindMouseAndKeyboardInputControl>();
                    break;
            };

            GameplayInputControlData _InputControlData = new GameplayInputControlData();
            _GameplayInputControl.ConfigureInputEvents(this.m_PlayerInput);
            _GameplayInputControl.SetInputControlData(_InputControlData);
            _GameplayInputControl.SetInitialiseGameplayInput(
                                    new GameplayInputInitializerCommand(
                                        _InputControlData, 
                                        ((ISceneManager)SceneManager.Instance).SceneState.ActivePlayer,
                                        GameManager.instance.GameState.SessionUser));
            this.m_GameplayInputControl = _GameplayInputControl;

            _RewindInputControl.ConfigureInputEvents(this.m_PlayerInput);
            this.m_RewindInputControl = _RewindInputControl;
            this.m_RewindInputControl.DisableInput();
        }

        void IInputManager.UnpossesPlayerObject(GameObject playerObject)
        {
            // Important: Only will be implemented if the player object becomes disposed and requires a new object.
            throw new NotImplementedException();
        }
        
        // -------------------------------------
        // InputControl state management
        // -------------------------------------

        void IInputManager.SwitchToGameplayControls()
        {
            this.m_PlayerInput.SwitchCurrentActionMap("Gameplay");
            
            this.m_GameplayInputControl.EnableInput();
            this.m_MenuInputControl.DisableInput();
            this.m_RewindInputControl.DisableInput();
            
            this.m_ActiveInputActionMap = InputActionMapType.Gamplay;
        }

        void IInputManager.SwitchToMenuControls()
        {
            this.m_PlayerInput.SwitchCurrentActionMap("Menu");
            
            this.m_GameplayInputControl.DisableInput();
            this.m_MenuInputControl.EnableInput();
            this.m_RewindInputControl.DisableInput();
            
            this.m_ActiveInputActionMap = InputActionMapType.Menu;
        }

        void IInputManager.SwitchToRewindControls()
        {
            this.m_PlayerInput.SwitchCurrentActionMap("Rewind");
            
            this.m_GameplayInputControl.DisableInput();
            this.m_MenuInputControl.DisableInput();
            this.m_RewindInputControl.EnableInput();
            
            this.m_ActiveInputActionMap = InputActionMapType.Rewind;
        }

        #endregion Methods

    }
    
    public enum ControlDevices 
    {
        KeyboardAndMouse,
        Gamepad
    }

    public enum InputActionMapType
    {
        Gamplay,
        Menu,
        Rewind
    }
    
}
