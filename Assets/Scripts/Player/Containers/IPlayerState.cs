using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;

namespace ThatOneSamuraiGame.Scripts.Player.Containers
{
    
    public interface IPlayerState
    {

        #region - - - - - - Properties - - - - - -

        PlayerAttackState PlayerAttackState { get; }
        
        PlayerMovementState PlayerMovementState { get; }
        
        PlayerSpecialActionState PlayerSpecialActionState { get; }
        
        PlayerTargetTrackingState PlayerTargetTrackingState { get; }

        #endregion Properties
        
    }
    
}