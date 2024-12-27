using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    [Serializable]
    public class PlayerAttackState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private bool m_CanAttack = true;
        [SerializeField] 
        private bool m_HasBeenParried;
        [SerializeField]
        private bool m_IsHeavyAttackCharging;
        [SerializeField]
        private bool m_IsWeaponSheathed;

        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        public bool CanAttack
        {
            get => this.m_CanAttack;
            set => this.m_CanAttack = value;
        }

        public bool HasBeenParried
        {
            get => this.m_HasBeenParried;
            set => this.m_HasBeenParried = value;
        }

        public bool IsHeavyAttackCharging
        {
            get => this.m_IsHeavyAttackCharging;
            set => this.m_IsHeavyAttackCharging = value;
        }

        public bool IsWeaponSheathed
        {
            get => this.m_IsWeaponSheathed;
            set => this.m_IsWeaponSheathed = value;
        }

        #endregion Properties

    }
    
}