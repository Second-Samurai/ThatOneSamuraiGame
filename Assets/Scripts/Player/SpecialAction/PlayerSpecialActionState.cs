using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.SpecialAction
{
    
    [Serializable]
    public class PlayerSpecialActionState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] 
        private bool m_CanDodge = true;
        [SerializeField] 
        private bool m_IsDodging;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public bool CanDodge
        {
            get => this.m_CanDodge;
            set => this.m_CanDodge = value;
        }

        public bool IsDodging
        {
            get => this.m_IsDodging;
            set => this.m_IsDodging = value;
        }

        #endregion Properties

    }
    
}