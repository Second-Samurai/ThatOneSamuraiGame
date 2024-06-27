using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public interface IPlayerMovement
    {

        #region - - - - - - Methods - - - - - -

        void MovePlayer(Vector2 moveDirection);

        void SprintPlayer(bool isSprinting);

        #endregion Methods

    }
    
}
