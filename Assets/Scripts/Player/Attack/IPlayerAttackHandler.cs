namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    public interface IPlayerAttackHandler
    {
        
        #region - - - - - - Methods - - - - - -

        void Attack();

        void DrawSword();

        void StartHeavy();

        void StartHeavyAlternative();

        void StartBlock();

        void EndBlock();

        #endregion Methods

    }
    
}
