using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;

namespace ThatOneSamuraiGame.Scripts.Player.Animation
{
    
    /// <summary>
    /// Responsible for handling animation events for the player.
    /// </summary>
    public class PlayerAnimationEventHandler : TOSGMonoBehaviourBase
    {

        #region - - - - - - Fields - - - - - -

        private IPlayerMovement m_PlayerMovement;
        private IPlayerSpecialAction m_PlayerSpecialAction;

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
            this.m_PlayerSpecialAction = this.GetComponent<IPlayerSpecialAction>();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - PlayerMovement Animation Events  - - - - - -
        
        // Tech Debt: # Rename these methods to represent their action.
        public void OverrideMovement() 
            => this.m_PlayerMovement.DisableMovement();

        // Tech Debt: # Rename these methods to represent their action.
        public void RemoveOverride() 
            => this.m_PlayerMovement.EnableMovement();

        #endregion PlayerMovement Animation Events

        #region - - - - - - PlayerSpecialAction Animation Events - - - - - -

        public void BlockDodge() 
            => this.m_PlayerSpecialAction.CanDodge = false;

        public void ResetDodge()
        {
            this.m_PlayerSpecialAction.CanDodge = true;
            this.m_PlayerSpecialAction.IsDodging = true;
        }

        public void StartDodging()
        {
            
        }

        #endregion PlayerSpecialAction Animation Events
        
    }
    
}