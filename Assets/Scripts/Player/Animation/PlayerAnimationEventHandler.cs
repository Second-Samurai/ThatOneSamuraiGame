using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Movement;

namespace ThatOneSamuraiGame.Scripts.Player.Animation
{
    
    /// <summary>
    /// Responsible for handling animation events for the player.
    /// </summary>
    public class PlayerAnimationEventHandler : TOSGMonoBehaviourBase
    {

        #region - - - - - - Fields - - - - - -

        private IPlayerMovement m_PlayerMovement;

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - PlayerMovement Animation Events  - - - - - -
        
        // TODO: This animation event needs to be renamed to DisablePlayerMovement
        public void OverrideMovement()
        {
            this.m_PlayerMovement.DisableMovement();
        }

        // TODO: This animation event needs to be renamed to EnablePlayerMovement
        public void RemoveOverride()
        {
            this.m_PlayerMovement.EnableMovement();
        }

        #endregion PlayerMovement Animation Events
        
    }
    
}