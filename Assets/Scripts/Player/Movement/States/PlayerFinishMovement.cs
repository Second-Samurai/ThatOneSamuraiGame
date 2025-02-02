using Player.Animation;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public class PlayerFinishMovement : BasePlayerMovementState
{
    
    #region - - - - - - Fields - - - - - -

    private readonly ILockOnSystem m_LockOnSystem;
    private GameObject m_TargetEnemy;

    #endregion Fields

    #region - - - - - - Constructors - - - - - -

    public PlayerFinishMovement(
        PlayerMovementDataContainer movementDataContainer,
        PlayerAnimationComponent playerAnimationComponent,
        Transform playerTransform)
        :base(playerAnimationComponent, playerTransform, movementDataContainer) { }

    #endregion Constructors
  
    #region - - - - - - Methods - - - - - -

    public override void CalculateMovement() { }

    public override void ApplyMovement() { }
    
    public override PlayerMovementStates GetState()
        => PlayerMovementStates.Finisher;

    public override void PerformDodge() { }

    public override void SetInputDirection(Vector2 inputDirection) { }

    #endregion Methods
    
}
