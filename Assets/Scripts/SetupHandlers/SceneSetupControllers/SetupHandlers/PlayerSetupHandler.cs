using ThatOneSamuraiGame.Scripts.Input;
using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public class PlayerSetupHandler : MonoBehaviour, ISetupHandler
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private Transform m_PlayerSpawnPoint;
        
        private ISetupHandler m_NextHandler;
        private PlayerController m_PlayerController;

        #endregion Fields
  
        #region - - - - - - Methods - - - - - -

        void ISetupHandler.SetNext(ISetupHandler setupHandler)
            => this.m_NextHandler = setupHandler;

        void ISetupHandler.Handle()
        {
            GameSettings _GameSettings = GameManager.instance.gameSettings;
            SceneState _SceneState = SceneManager.Instance.SceneState;
            
            Vector3 _TargetHolderPos = _GameSettings.targetHolderPrefab.transform.position;
            GameObject _TargetHolder = Instantiate(_GameSettings.targetHolderPrefab, _TargetHolderPos, Quaternion.identity);

            PlayerController _PlayerControl = Object.FindFirstObjectByType<PlayerController>();
            if(_PlayerControl != null)
            {
                SceneManager.Instance.CameraControl = _PlayerControl.GetComponent<CameraControl>(); // TODO: This is camera specific, move to camera setup handler
                SceneManager.Instance.SceneState.ActivePlayer = _PlayerControl.gameObject;
                this.InitialisePlayer(_TargetHolder);
                return;
            }

            GameObject _PlayerObject = 
                Instantiate(_GameSettings.playerPrefab, this.m_PlayerSpawnPoint.position, Quaternion.identity);
            this.m_PlayerController = _PlayerObject.GetComponentInChildren<PlayerController>();
            _SceneState.ActivePlayer = _PlayerObject;
            this.InitialisePlayer(_TargetHolder);
            
            this.m_NextHandler?.Handle();
        }
        
        private void InitialisePlayer(GameObject targetHolder) // Handled in pipeline
        {
            // TODO: Change this to be the PlayerInitializerCommand
            this.m_PlayerController.Init(targetHolder);

            IInputManager _InputManager = GameManager.instance.InputManager;

            if (_InputManager.DoesGameplayInputControlExist()) 
                _InputManager.PossesPlayerObject(this.m_PlayerController.gameObject);
        }

        #endregion Methods
  
    }

}