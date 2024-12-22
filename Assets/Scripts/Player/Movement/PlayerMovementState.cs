using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{
    
    // TODO: Rename to PlayerMovementDataContainer
    [Serializable]
    public class PlayerMovementState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] 
        private bool m_CanOverrideMovement;
        [SerializeField] 
        private bool m_IsMovementEnabled;
        [SerializeField]
        private bool m_IsMovementLocked;
        
        private Vector3 m_MoveDirection = Vector3.zero;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public bool CanOverrideMovement
        {
            get => this.m_CanOverrideMovement;
            set => this.m_CanOverrideMovement = value;
        }

        public bool IsMovementEnabled
        {
            get => this.m_IsMovementEnabled;
            set => this.m_IsMovementEnabled = value;
        }

        public bool IsMovementLocked
        {
            get => this.m_IsMovementLocked;
            set => this.m_IsMovementLocked = value;
        }

        public Vector3 MoveDirection
        {
            get => this.m_MoveDirection;
            set => this.m_MoveDirection = value;
        }
        
        #endregion Properties

    }
    
}