using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Containers
{
    
    public interface IPlayerState
    {

        #region - - - - - - Properties - - - - - -

        GameObject AttackTarket { get; }
        
        bool CanOverrideMovement { get; set; }
        
        PlayerAttackState PlayerAttackState { get; }
        
        PlayerMovementState PlayerMovementState { get; }

        #endregion Properties
        
    }
    
}