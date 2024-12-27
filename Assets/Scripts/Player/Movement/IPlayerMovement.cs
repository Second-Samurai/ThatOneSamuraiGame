using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public interface IPlayerMovement
    {
        
        #region - - - - - - Methods - - - - - -

        void DisableMovement();

        void DisableRotation();

        void EnableMovement();

        void EnableRotation();

        void PreparePlayerMovement(Vector2 moveDirection);

        void PrepareSprint(bool isSprinting);

        #endregion Methods

    }
    
}
