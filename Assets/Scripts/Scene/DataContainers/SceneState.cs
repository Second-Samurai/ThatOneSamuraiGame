using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Scene.DataContainers
{

    /// <summary>
    /// SceneState keeps track of non-persistent references and values during gameplay.
    /// </summary>
    /// <remarks>
    ///     The lifetime of the container is throughout an entire sequence of scenes of related gameplay.
    ///     This is not tied to the lifetime of a single unity scene.
    /// </remarks>
    public class SceneState : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        // Public Fields
        public bool IsGamePaused;
        
        // Scene Objects
        public GameObject ActivePlayer;
        
        #endregion Fields

    }

}