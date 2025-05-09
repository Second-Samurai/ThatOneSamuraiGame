using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;

namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseMediator
{

    public interface IPauseMediator
    {

        #region - - - - - - Methods - - - - - -

        void Notify(string component, PauseActionType pauseActionType);
        
        void SetPauseMenuController(IPauseMenuController pauseMenuController);

        #endregion Methods

    }

}