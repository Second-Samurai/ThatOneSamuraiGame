using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {

        #region - - - - - - Methods - - - - - -
        void IPlayerMovement.MovePlayer(Vector2 moveDirection)
        {
            throw new System.NotImplementedException();
        }

        void IPlayerMovement.SprintPlayer(bool isSprinting)
        {
            throw new System.NotImplementedException();
        }

        #endregion Methods

    }
    
}
