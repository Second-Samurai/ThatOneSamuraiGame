namespace ThatOneSamuraiGame.Scripts.UI.Pause.PauseMediator
{

    public interface IPauseMediator
    {

        #region - - - - - - Methods - - - - - -

        void Notify(string component, PauseActionType pauseActionType);

        #endregion Methods

    }

}