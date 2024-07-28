namespace ThatOneSamuraiGame.Scripts.Player.SpecialAction
{
    
    public interface IPlayerSpecialAction
    {

        #region - - - - - - Methods - - - - - -

        // -----------------------------------------------------
        // Dodge related Events
        // -----------------------------------------------------
        
        void Dodge();

        void ResetDodge();
        
        // -----------------------------------------------------
        // Rewind related Events
        // -----------------------------------------------------

        void ActivateRewind();

        #endregion Methods

    }
    
}
