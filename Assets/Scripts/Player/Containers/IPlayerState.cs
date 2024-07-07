using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Containers
{
    
    public interface IPlayerState
    {

        #region - - - - - - Properties - - - - - -

        GameObject AttackTarket { get; }
        
        PlayerAttackState PlayerAttackState { get; }
        
        PlayerMovementState PlayerMovementState { get; }
        
        PlayerSpecialActionState PlayerSpecialActionState { get; }

        #endregion Properties
        
    }
    
}