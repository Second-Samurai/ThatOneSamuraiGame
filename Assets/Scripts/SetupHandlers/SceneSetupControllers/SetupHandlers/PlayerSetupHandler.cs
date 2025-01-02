using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Initializers;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    /// <summary>
    /// Handles the initial setup of the player object.
    /// </summary>
    /// <remarks>
    /// This can spawn a player in the case no player is provided. However, it is recommended that an instance exists
    /// during the start of the gameplay,
    /// </remarks>
    public class PlayerSetupHandler : MonoBehaviour, ISetupHandler
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private Transform m_PlayerSpawnPoint;
        [SerializeField] private GameObject m_PlayerObject;
        
        private ISetupHandler m_NextHandler;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle(SceneSetupContext setupContext)
        {
            this.ValidateParameters(setupContext);
            
            // Load dependencies
            ICameraController _CameraController = setupContext.CameraControlObject.GetComponent<ICameraController>();
            
            // Initialise if the player exists.
            if (this.m_PlayerObject != null)
                this.SetActivePlayer(_CameraController, this.m_PlayerObject);
            else
            {
                // Create a player object if the player does not exist in scene.
                GameObject _SpawnedPlayerObject = Instantiate(
                    GameManager.instance.gameSettings.playerPrefab, 
                    this.m_PlayerSpawnPoint.position, 
                    Quaternion.identity);
                this.SetActivePlayer(_CameraController, _SpawnedPlayerObject);
            }
            
            this.SetupPlayerLockOnControl(_CameraController, setupContext.LockOnControlObject);

            print("[LOG]: Completed Scene Player setup.");
            this.m_NextHandler?.Handle(setupContext);
        }

        private void SetupPlayerLockOnControl(ICameraController cameraController, GameObject lockOnObject)
        {
            if (!GameValidator.NotNull(lockOnObject, nameof(lockOnObject))
                || !GameValidator.NotNull(cameraController, nameof(cameraController))) return;

            IInitialize<LockOnSystemInitializationData> _LockOnInitializer =
                lockOnObject.GetComponent<IInitialize<LockOnSystemInitializationData>>();
            ILockOnObserver _LockOnObserver = lockOnObject.GetComponent<ILockOnObserver>();
            
            // Initialize lock on system.
            _LockOnInitializer.Initialize(new LockOnSystemInitializationData(cameraController));
            
            // Assign values
            PlayerTargetTrackingState _PlayerTargetTrackingState = 
                this.m_PlayerObject.GetComponent<IPlayerState>().PlayerTargetTrackingState;;
            _LockOnObserver.OnNewLockOnTarget
                .AddListener(newTarget => _PlayerTargetTrackingState.AttackTarget = newTarget);
        }

        private void SetActivePlayer(ICameraController cameraController, GameObject playerObject)
        {
            GameObject _TargetHolder = Instantiate(
                GameManager.instance.gameSettings.targetHolderPrefab, 
                GameManager.instance.gameSettings.targetHolderPrefab.transform.position, 
                Quaternion.identity);
            PlayerController _PlayerController = playerObject.GetComponent<PlayerController>();
            
            SceneManager _SceneManager = SceneManager.Instance;
            _SceneManager.SceneState.ActivePlayer = playerObject;
            _SceneManager.PlayerController = _PlayerController;
            _SceneManager.SceneState.ActivePlayer = playerObject;
            
            // Initialise the player
            PlayerInitializerCommand _InitializerCommand = new PlayerInitializerCommand(
                _PlayerController.gameObject,
                 _TargetHolder,
                cameraController);
            _InitializerCommand.Execute();

            IInputManager _InputManager = GameManager.instance.InputManager;
            if (_InputManager.DoesGameplayInputControlExist()) 
                _InputManager.PossesPlayerObject(_PlayerController.gameObject);
        }

        private void ValidateParameters(SceneSetupContext setupContext)
        {
            _ = GameValidator.NotNull(this.m_PlayerSpawnPoint, nameof(this.m_PlayerSpawnPoint));
            _ = GameValidator.NotNull(this.m_PlayerObject, nameof(this.m_PlayerObject));
            _ = GameValidator.NotNull(setupContext.CameraControlObject, nameof(setupContext.CameraControlObject));
            _ = GameValidator.NotNull(setupContext.LockOnControlObject, nameof(setupContext.LockOnControlObject));
        }

        #endregion Methods
  
    }

}