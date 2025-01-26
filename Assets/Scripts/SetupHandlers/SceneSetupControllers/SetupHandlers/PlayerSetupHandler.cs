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
            
            // Initialise if the player exists.
            if (this.m_PlayerObject != null)
                this.SetActivePlayer(setupContext.CameraController, setupContext.LockOnObserver, this.m_PlayerObject);
            else
            {
                // Create a player object if the player does not exist in scene.
                GameObject _SpawnedPlayerObject = Instantiate(
                    GameManager.instance.gameSettings.playerPrefab, 
                    this.m_PlayerSpawnPoint.position, 
                    Quaternion.identity);
                this.SetActivePlayer(setupContext.CameraController, setupContext.LockOnObserver, _SpawnedPlayerObject);
            }
            
            this.SetupPlayerLockOnControl(
                setupContext.CameraController, 
                setupContext.LockOnSystem, 
                setupContext.LockOnObserver);

            print("[LOG]: Completed Scene Player setup.");
            this.m_NextHandler?.Handle(setupContext);
        }

        private void SetupPlayerLockOnControl(
            ICameraController cameraController, 
            LockOnSystem lockOnSystem, 
            ILockOnObserver lockOnObserver)
        {
            // Initialize lock on system.
            lockOnSystem.Initialize(new() 
            {
                CameraController = cameraController, 
                AnimationDispatcher = this.m_PlayerObject.GetComponent<IPlayerAnimationDispatcher>()
            });
            
            // Assign values
            PlayerTargetTrackingState _PlayerTargetTrackingState = 
                this.m_PlayerObject.GetComponent<IPlayerState>().PlayerTargetTrackingState;
            lockOnObserver.OnNewLockOnTarget
                .AddListener(newTarget => _PlayerTargetTrackingState.AttackTarget = newTarget);
        }

        private void SetActivePlayer(
            ICameraController cameraController, 
            ILockOnObserver lockOnObserver, 
            GameObject playerObject)
        {
            this.m_PlayerObject = playerObject;
            
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
                cameraController,
                lockOnObserver);
            _InitializerCommand.Execute();

            IInputManager _InputManager = GameManager.instance.InputManager;
            if (_InputManager.DoesGameplayInputControlExist()) 
                _InputManager.PossesPlayerObject(_PlayerController.gameObject);
        }

        private void ValidateParameters(SceneSetupContext setupContext)
        {
            _ = GameValidator.NotNull(this.m_PlayerSpawnPoint, nameof(this.m_PlayerSpawnPoint));
            _ = GameValidator.NotNull(this.m_PlayerObject, nameof(this.m_PlayerObject));
            _ = GameValidator.NotNull(setupContext.LockOnObserver, nameof(setupContext.LockOnObserver));
            _ = GameValidator.NotNull(setupContext.LockOnSystem, nameof(setupContext.LockOnSystem));
        }

        #endregion Methods
  
    }

}