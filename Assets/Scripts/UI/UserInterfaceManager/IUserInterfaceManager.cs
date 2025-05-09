using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMenu;

namespace ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager
{

    public interface IUserInterfaceManager
    {

        #region - - - - - - Properties - - - - - -

        IPauseMenuController PauseMenu { get; }
        
        ButtonController ButtonController { get; set;  }

        #endregion Properties


        #region - - - - - - Methods - - - - - -

        void SetupUserInterface();

        #endregion Methods

    }

}