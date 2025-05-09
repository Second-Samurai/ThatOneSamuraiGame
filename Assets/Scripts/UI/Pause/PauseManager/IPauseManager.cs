using ThatOneSamuraiGame.Scripts.UI.Pause.PauseMediator;

namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseManager
{

    public interface IPauseManager
    {

        #region - - - - - - Properties - - - - - -

        IPauseMediator PauseMediator { get; }

        #endregion Properties

    }

}