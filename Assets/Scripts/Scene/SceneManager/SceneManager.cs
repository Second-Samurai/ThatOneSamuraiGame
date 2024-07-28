using System.Runtime;
using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Scene.SceneManager
{

    /// <summary>
    /// Responsible for managing both state and behavior of the scene.
    /// </summary>
    public class SceneManager : MonoBehaviour, ISceneManager
    {

        #region - - - - - - Fields - - - - - -
        
        [Header("Data State")]
        [SerializeField] private SceneState m_SceneState;

        [Header("Controllers and Managers")]
        [SerializeField] private CheckpointManager m_CheckPointManager;
        [SerializeField] private RewindManager m_RewindManager;
        [Space]
        [SerializeField] private EnemyTracker m_EnemyTracker;
        [SerializeField] private EnemySpawnManager m_EnemySpawnManager;

        [Header("Camera")]
        [SerializeField] private CameraControl m_CameraControl;
        [SerializeField] private LockOnTracker m_LockOnTracker;
        [SerializeField] private GameObject m_ThirdPersonViewCamera;
        private UnityEngine.Camera m_MainCamera;

        [Header("Player")]
        [SerializeField] private PlayerController m_PlayerController;
        
        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        // -------------------------------
        // Data State
        // -------------------------------
        
        SceneState ISceneManager.SceneState
            => this.m_SceneState;
        
        // -------------------------------
        // Controllers and Managers
        // -------------------------------
        
        CheckpointManager ISceneManager.CheckpointManager
            => this.m_CheckPointManager;

        RewindManager ISceneManager.RewindManager
            => this.m_RewindManager;

        EnemyTracker ISceneManager.EnemyTracker
            => this.m_EnemyTracker;

        EnemySpawnManager ISceneManager.EnemySpawnManager
            => this.m_EnemySpawnManager;
        
        // -------------------------------
        // Camera
        // -------------------------------
        
        CameraControl ISceneManager.CameraControl
            => this.m_CameraControl;

        LockOnTracker ISceneManager.LockOnTracker
            => this.m_LockOnTracker;

        UnityEngine.Camera ISceneManager.MainCamera
            => this.m_MainCamera;

        GameObject ISceneManager.ThirdPersonViewCamera
            => this.m_ThirdPersonViewCamera;
        
        // -------------------------------
        // Player
        // -------------------------------
        
        PlayerController ISceneManager.PlayerController
            => this.m_PlayerController;

        #endregion Properties

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            if (!this.DoesSceneStateExist())
                this.m_SceneState = this.GetComponent<SceneState>();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Validation Methods - - - - - -

        private bool DoesSceneStateExist()
            => this.m_SceneState != null;

        #endregion Validation Methods

    }

}