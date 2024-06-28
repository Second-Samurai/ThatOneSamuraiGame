using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public interface IPlayerMovement
    {

        #region - - - - - - Properties - - - - - -

        Vector2 MoveDirection { get; }

        #endregion Properties
        
        #region - - - - - - Methods - - - - - -

        void DisableMovement();

        void DisableRotation();

        void EnableMovement();

        void EnableRotation();

        void PreparePlayerMovement(Vector2 moveDirection);

        void Sprint(bool isSprinting);

        #endregion Methods

    }
    
}
