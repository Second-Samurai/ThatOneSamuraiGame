using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;

namespace ThatOneSamuraiGame.Scripts.Player.Animation
{
    
    /// <summary>
    /// Responsible for handling animation events for the player.
    /// </summary>
    public class PlayerAnimationEventHandler : PausableMonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        private IDamageable m_PlayerDamage;
        private IPlayerAttackHandler m_PlayerAttackHandler;
        private PlayerFunctions m_PlayerFunctions;
        private IPlayerMovement m_PlayerMovement;
        private IPlayerSpecialAction m_PlayerSpecialAction;
        
        // Player states
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerSpecialActionState m_PlayerSpecialActionState;

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerAttackHandler = this.GetComponent<IPlayerAttackHandler>();
            this.m_PlayerDamage = this.GetComponent<IDamageable>();
            this.m_PlayerFunctions = this.GetComponent<PlayerFunctions>();
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
            this.m_PlayerSpecialAction = this.GetComponent<IPlayerSpecialAction>();

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this.m_PlayerMovementState = _PlayerState.PlayerMovementState;
            this.m_PlayerSpecialActionState = _PlayerState.PlayerSpecialActionState;
        }

        #endregion Lifecycle Methods

        #region - - - - - - PlayerAttack Animation Events - - - - - -

        public void DisableRotation()
            => this.m_PlayerMovement.DisableRotation();

        public void EnableRotation()
            => this.m_PlayerMovement.EnableRotation();

        // Tech Debt: #48 - Rename these methods to represent their action.
        public void EndGotParried()
            => this.m_PlayerAttackState.HasBeenParried = false;

        // Tech Debt: #48 - Rename these methods to represent their action.
        public void StartGotParried()
            => this.m_PlayerAttackState.HasBeenParried = true;

        #endregion PlayerAttack Animation Events
        
        #region - - - - - - PlayerMovement Animation Events  - - - - - -
        
        // Tech Debt: #48 - Rename these methods to represent their action.
        public void OverrideMovement() 
            => this.m_PlayerMovement.DisableMovement();

        // Tech Debt: #48 - Rename these methods to represent their action.
        public void RemoveOverride() 
            => this.m_PlayerMovement.EnableMovement();

        public void LockMoveInput() // This is not being used anywhere
        { 
            if (this.m_PlayerMovementState.IsMovementLocked)
                return;

            this.m_PlayerMovementState.IsMovementLocked = true;
            this.StartDodging(); // Note: I find it unusual that Dodging is invoked when not moving the character.
        }

        public void UnlockMoveInput()
        {
            if (!this.m_PlayerMovementState.IsMovementLocked)
                return;

            this.m_PlayerMovementState.IsMovementLocked = false;
            this.EndDodging();
        }

        #endregion PlayerMovement Animation Events

        #region - - - - - - PlayerSpecialAction Animation Events - - - - - -
        
        // public void BlockDodge() 
        //     => this.m_PlayerSpecialActionState.CanDodge = false;

        // // TODO: Remove resets from happening exclusivly from the AnimationEventHandler.
        // public void ResetDodge()
        //     => this.m_PlayerSpecialAction.ResetDodge();

        public void StartDodging()
        {
            this.m_PlayerAttackState.CanAttack = false;
            this.m_PlayerSpecialActionState.IsDodging = true;
            this.m_PlayerAttackState.IsHeavyAttackCharging = false;
            this.m_PlayerAttackState.IsWeaponSheathed = false;
            
            this.m_PlayerDamage.DisableDamage();
            this.m_PlayerFunctions.DisableBlock();
            this.m_PlayerAttackHandler.ResetAttack();
        }

        public void EndDodging()
        {
            this.m_PlayerAttackState.CanAttack = true;
            // this.m_PlayerSpecialActionState.IsDodging = false;
            
            this.m_PlayerDamage.EnableDamage();
            this.m_PlayerFunctions.EnableBlock();
            this.m_PlayerAttackHandler.ResetAttack();
        }

        #endregion PlayerSpecialAction Animation Events

    }
    
}