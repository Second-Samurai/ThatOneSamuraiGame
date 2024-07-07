namespace ThatOneSamuraiGame.Scripts.Player.SpecialAction
{
    
    public interface IPlayerSpecialAction
    {

        #region - - - - - - Properties - - - - - -

        bool CanDodge { get; set; }
        
        bool IsDodging { get; set; }

        #endregion Properties
        
        #region - - - - - - Methods - - - - - -

        void Dodge();

        void ResetDodge();

        #endregion Methods

    }
    
}
