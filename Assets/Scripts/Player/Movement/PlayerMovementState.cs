using System;
using System.Configuration;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    [Serializable]
    public class PlayerMovementState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private bool m_CanOverrideMovement = false;
        private Vector3 m_MoveDirection = Vector3.zero;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public bool CanOverrideMovement
        {
            get => this.m_CanOverrideMovement;
            set => this.m_CanOverrideMovement = value;
        }

        public Vector3 MoveDirection
        {
            get => this.m_MoveDirection;
            set => this.m_MoveDirection = value;
        }
        
        #endregion Properties

    }
    
}