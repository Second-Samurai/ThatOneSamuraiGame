using UnityEngine;

public interface IPlayerMovementState
{
    
    #region - - - - - - Methods - - - - - -

    void CalculateMovement();

    void ApplyMovement();

    void PerformDodge();

    void SetInputDirection(Vector2 inputDirection);

    #endregion Methods
    
}
