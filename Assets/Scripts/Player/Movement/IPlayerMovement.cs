using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public interface IPlayerMovement
    {

        #region - - - - - - Properties - - - - - -

        bool IsSprinting { get; }

        #endregion Properties
  
        #region - - - - - - Methods - - - - - -

        void CancelMove();

        void DisableMovement();

        void DisableRotation();

        void Dodge();

        void EnableMovement();

        void EnableRotation();

        void PreparePlayerMovement(Vector2 moveDirection);

        void PrepareSprint(bool isSprinting);

        void SetState(PlayerMovementStates movementState);

        #endregion Methods

    }
    
}
