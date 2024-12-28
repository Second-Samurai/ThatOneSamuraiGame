using UnityEngine;

public interface IPlayerMovementState
{
    
    #region - - - - - - Methods - - - - - -

    void ApplyMovement();

    void CalculateMovement();

    PlayerMovementStates GetState();

    void PerformDodge();

    void SetInputDirection(Vector2 inputDirection);

    #endregion Methods

}
