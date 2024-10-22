using ThatOneSamuraiGame.Scripts.Enumeration;
using ThatOneSamuraiGame.Scripts.Scene.DataContainers;
using ThatOneSamuraiGame.Scripts.Scene.Loaders;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Scene.SceneManager
{

    public interface ISceneManager
    {

        #region - - - - - - Properties - - - - - -
        
        // -------------------------------
        // Data State
        // -------------------------------

        SceneState SceneState { get; }
        
        // -------------------------------
        // Controllers and Managers
        // -------------------------------
        
        CheckpointManager CheckpointManager { get; }
        
        RewindManager RewindManager { get; }
        
        EnemyTracker EnemyTracker { get; }
        
        EnemySpawnManager EnemySpawnManager { get; }
        
        SceneLoader SceneLoader { get; }
        
        // -------------------------------
        // Camera
        // -------------------------------
        
        CameraControl CameraControl { get; }
        
        LockOnTracker LockOnTracker { get; }
        
        UnityEngine.Camera MainCamera { get; }
        
        GameObject ThirdPersonViewCamera { get;}
        
        // -------------------------------
        // Player
        // -------------------------------
        
        PlayerController PlayerController { get;  }

        #endregion Properties

        #region - - - - - - Methods - - - - - -

        void SetupScene();

        void SetupCurrentScene(GameScenes currentScene);

        #endregion Methods

    }

}