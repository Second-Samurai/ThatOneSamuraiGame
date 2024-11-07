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
          
        EnemyTracker EnemyTracker { get; }
        
        EnemySpawnManager EnemySpawnManager { get; }
        
        SceneLoader SceneLoader { get; }
        
        // -------------------------------
        // Camera
        // -------------------------------
        
        CameraControl CameraControl { get; set; }

        LockOnTracker LockOnTracker { get; }
        
        UnityEngine.Camera MainCamera { get; set; }

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