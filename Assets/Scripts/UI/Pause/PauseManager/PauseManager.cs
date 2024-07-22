using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMediator;
using ThatOneSamuraiGame.Scripts.UI.Pause.PauseObserver;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseManager
{

    // ------------------------------------------
    // There is no behavior implemented. This will be done as part of ticket: #46
    //  - For now this class will behave as a container.
    // ------------------------------------------
    
    /// <summary>
    /// Responsible for managing pause related game-logic components.
    /// </summary>
    public class PauseManager : MonoBehaviour, IPauseManager
    {

        #region - - - - - - Fields - - - - - -

        private IPauseMediator m_PauseMediator;
        private IPauseObserver m_PauseObserver;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PauseMediator = this.GetComponent<IPauseMediator>();
            this.m_PauseObserver = this.GetComponent<IPauseObserver>();
        }

        #endregion Lifecycle Methods

        #region - - - - - - Methods - - - - - -
        
        IPauseMediator IPauseManager.PauseMediator
            => this.m_PauseMediator;

        #endregion Methods

    }

}