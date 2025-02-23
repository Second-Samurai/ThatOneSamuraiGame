namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    public interface IPlayerAttackSystem
    {
        
        #region - - - - - - Methods - - - - - -

        void Attack();

        // void DrawWeapon();

        void EndAttack();

        void ResetAttack();

        void StartHeavy();

        void StartHeavyAlternative();

        void EndParryAction();

        void StartBlock();

        void EndBlock();

        void EnableAttack();

        void DisableAttack();

        #endregion Methods

    }
    
}
