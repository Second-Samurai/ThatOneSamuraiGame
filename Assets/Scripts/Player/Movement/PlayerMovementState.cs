using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    public class PlayerMovementState
    {

        #region - - - - - - Fields - - - - - -

        private Vector3 m_MoveDirection;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public Vector3 MoveDirection
            => this.m_MoveDirection;

        #endregion Properties

    }
    
}