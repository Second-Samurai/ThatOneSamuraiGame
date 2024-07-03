namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    public interface IPlayerAttackState
    {
        
        #region - - - - - - Properties - - - - - -

        bool CanAttack { get; set; }
        
        bool HasBeenParried { get; set; }
        
        bool IsWeaponSheathed { get; set; }

        #endregion Properties
        
    }
    
}