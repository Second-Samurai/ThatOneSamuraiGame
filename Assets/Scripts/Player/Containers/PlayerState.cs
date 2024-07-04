using NUnit.Framework.Constraints;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Containers
{
    
    public class PlayerState : MonoBehaviour, IPlayerState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private GameObject m_AttackTarget;

        private bool m_CanOverrideMovement = false;

        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        GameObject IPlayerState.AttackTarket 
            => this.m_AttackTarget;

        bool IPlayerState.CanOverrideMovement
        {
            get => this.m_CanOverrideMovement;
            set => this.m_CanOverrideMovement = value;
        }

        #endregion Properties
        
    }
    
}