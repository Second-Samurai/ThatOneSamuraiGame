using ThatOneSamuraiGame.Legacy;
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

        [SerializeField] private CameraController m_CameraController;
        [SerializeField] private Transform m_PlayerSpawnPoint;
        [SerializeField] private GameObject m_PlayerObject;
        [SerializeField] private LockOnObserver m_LockOnObserver;
        
        private ISetupHandler m_NextHandler;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle()
        {
            // Validate required values
            _ = GameValidator.NotNull(this.m_PlayerSpawnPoint, nameof(this.m_PlayerSpawnPoint));
            _ = GameValidator.NotNull(this.m_PlayerObject, nameof(this.m_PlayerObject));
            // _ = GameValidator.NotNull(this.m_PlayerCameraTargetController, nameof(this.m_PlayerCameraTargetController));
            // _ = GameValidator.NotNull(this.m_ThirdPersonCamController, nameof(this.m_ThirdPersonCamController));
            
            // Initialise if the player exists.
            if (this.m_PlayerObject != null)
                this.SetActivePlayer(this.m_PlayerObject);
            else
            {
                // Create a player object if the player does not exist in scene.
                GameObject _SpawnedPlayerObject = Instantiate(
                    GameManager.instance.gameSettings.playerPrefab, 
                    this.m_PlayerSpawnPoint.position, 
                    Quaternion.identity);
                this.SetActivePlayer(_SpawnedPlayerObject);
            }
            
            this.SetupPlayerLockOnControl();

            print("[LOG]: Completed Scene Player setup.");
            this.m_NextHandler?.Handle();
        }
        
        private void InitialisePlayer(GameObject targetHolder, PlayerController playerController)
        {
            // TODO: Remove old camera controllers
            PlayerInitializerCommand _InitializerCommand = new PlayerInitializerCommand(
                playerController.gameObject,
                // this.m_PlayerCameraTargetController,
                // this.m_ThirdPersonCamController,
                targetHolder,
                m_CameraController);
            _InitializerCommand.Execute();

            IInputManager _InputManager = GameManager.instance.InputManager;
            if (_InputManager.DoesGameplayInputControlExist()) 
                _InputManager.PossesPlayerObject(playerController.gameObject);
        }

        private void SetupPlayerLockOnControl()
        {
            if (!GameValidator.NotNull(this.m_LockOnObserver, nameof(this.m_LockOnObserver))) return;

            PlayerTargetTrackingState _PlayerTargetTrackingState = 
                this.m_PlayerObject.GetComponent<IPlayerState>().PlayerTargetTrackingState;;
            this.m_LockOnObserver.OnNewLockOnTarget
                .AddListener(newTarget => _PlayerTargetTrackingState.AttackTarget = newTarget);
        }

        private void SetActivePlayer(GameObject playerObject)
        {
            GameObject _TargetHolder = Instantiate(
                GameManager.instance.gameSettings.targetHolderPrefab, 
                GameManager.instance.gameSettings.targetHolderPrefab.transform.position, 
                Quaternion.identity);
            PlayerController _PlayerController = playerObject.GetComponent<PlayerController>();
            SceneManager _SceneManager = SceneManager.Instance;
            
            _SceneManager.CameraControl = playerObject.GetComponent<CameraControl>(); // TODO: This is camera specific, move to camera setup handler
            _SceneManager.SceneState.ActivePlayer = playerObject;
            _SceneManager.PlayerController = _PlayerController;
            _SceneManager.SceneState.ActivePlayer = playerObject;
            
            this.InitialisePlayer(_TargetHolder, _PlayerController);
        }
        
        #endregion Methods
  
    }

}