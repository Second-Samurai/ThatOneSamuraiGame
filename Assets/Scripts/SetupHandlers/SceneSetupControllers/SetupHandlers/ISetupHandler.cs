namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers
{

    public interface ISetupHandler
    {

        #region - - - - - - Methods - - - - - -

        void SetNext(ISetupHandler setupHandler);

        void Handle(SceneSetupContext setupContext);

        #endregion Methods

    }

}