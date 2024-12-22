using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.TargetTracking
{
    
    public class PlayerTargetTrackingState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private Transform m_AttackTarget;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public Transform AttackTarget
        {
            get => this.m_AttackTarget;
            set => this.m_AttackTarget = value;
        }

        #endregion Properties
        
    }
    
}