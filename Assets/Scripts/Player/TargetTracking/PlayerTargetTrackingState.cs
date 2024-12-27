using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.TargetTracking
{
    
    public class PlayerTargetTrackingState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private GameObject m_AttackTarget;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public GameObject AttackTarget
        {
            get => this.m_AttackTarget;
            set => this.m_AttackTarget = value;
        }

        #endregion Properties
        
    }
    
}