using System.Collections;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupHandlers
{

    /// <summary>
    /// Responsible for handling initial setup logic for a scene.
    /// </summary>
    public class SceneSetupHandler : MonoBehaviour
    {


        #region - - - - - - Methods - - - - - -

        public IEnumerator RunSetup()
        {
            yield return null;
        }

        private IEnumerator SetupScene()
        {
            yield return null;
        }

        private IEnumerator SetupUserInterface()
        {
            IUserInterfaceManager _UserInterfaceManager =
                Object.FindFirstObjectByType<UserInterfaceManager>(FindObjectsInactive.Exclude);
            _UserInterfaceManager.SetupUserInterface();
            
            yield return null;
        }
        
        private IEnumerator SetupPlayer()
        {
            yield return null;
        }

        #endregion Methods
  
    }

}