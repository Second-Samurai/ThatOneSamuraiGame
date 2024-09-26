using System.Collections;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.GameSetupHandlers
{

    public class GameSetupHandler : MonoBehaviour
    {
        
        #region - - - - - - Methods - - - - - -

        public IEnumerator RunSetup()
        {
            yield return StartCoroutine(this.SetupManagers());
            yield return StartCoroutine(this.LaunchGameScene());
        }

        private IEnumerator SetupManagers()
        {
            Debug.Log("[LOG]: There are no managers to currently setup. Placeholder log thrown instead.");
            yield return null;
        }

        private IEnumerator LaunchGameScene()
        {
            
            yield return null;
        }

        #endregion Methods
  
    }

}