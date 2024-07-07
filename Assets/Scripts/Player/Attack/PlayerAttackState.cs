using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    [Serializable]
    public class PlayerAttackState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private bool m_CanAttack;
        [SerializeField]
        private bool m_HasBeenParried;
        [SerializeField]
        private bool m_IsWeaponSheathed;

        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        public bool CanAttack
            => this.m_CanAttack;

        public bool HasBeenParried
            => this.m_HasBeenParried;

        public bool IsWeaponSheathed
            => this.m_IsWeaponSheathed;

        #endregion Properties

    }
    
}