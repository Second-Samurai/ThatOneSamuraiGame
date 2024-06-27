namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    public interface IPlayerAttackHandler
    {

        #region - - - - - - Methods - - - - - -

        void Attack();

        void DrawSword();

        void StartHeavy();

        void OnStartHeavyAlternative();

        void OnStartBlock();

        void OnEndBlock();

        #endregion Methods

    }
    
}
