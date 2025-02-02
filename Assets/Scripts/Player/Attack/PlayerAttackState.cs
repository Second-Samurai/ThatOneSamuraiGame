using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    [Serializable]
    public class PlayerAttackState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private bool m_CanAttack = true;
        [FormerlySerializedAs("m_HasBeenParried")] [SerializeField] 
        private bool m_IsParryStunned;
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

        public bool ParryStunned
        {
            get => this.m_IsParryStunned;
            set => this.m_IsParryStunned = value;
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