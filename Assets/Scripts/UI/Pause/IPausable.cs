namespace ThatOneSamuraiGame.Scripts.UI.Pause
{
    
    /// <summary>
    /// Associated with objects intended to be paused.
    /// </summary>
    public interface IPausable
    {
        
        #region - - - - - - Methods - - - - - -

        void OnPause();

        void OnUnPause();

        #endregion Methods

    }
    
}
