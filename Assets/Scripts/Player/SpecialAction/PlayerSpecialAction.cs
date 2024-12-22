using ThatOneSamuraiGame.Legacy;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.SpecialAction
{
    
    public class PlayerSpecialAction : MonoBehaviour, IPlayerSpecialAction
    {
        
        #region - - - - - - Fields - - - - - -

        private Legacy.ICameraController m_CameraController;
        private ICombatController m_CombatController;
        private IPlayerAttackHandler m_PlayerAttackHandler;
        private IDamageable m_PlayerDamageHandler;
        private IPlayerMovement m_PlayerMovement;
        
        // Player States
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementState m_PlayerMovementState;
        private PlayerSpecialActionState m_PlayerSpecialActionState;
        
        private Animator m_Animator;
        private float m_DodgeForce = 10f;
        private bool m_DodgeCache = false; // This field name makes no sense. Needs to be renamed
        private PlayerFunctions m_PlayerFunctions;
        private RewindInput m_RewindInput; // This is temporary until a separate player handler is created.

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_Animator = this.GetComponent<Animator>();
            this.m_CameraController = this.GetComponent<Legacy.ICameraController>();
            this.m_CombatController = this.GetComponent<ICombatController>();
            this.m_PlayerAttackHandler = this.GetComponent<IPlayerAttackHandler>();
            this.m_PlayerDamageHandler = this.GetComponent<IDamageable>();
            this.m_PlayerFunctions = this.GetComponent<PlayerFunctions>();
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
            this.m_RewindInput = this.GetComponent<RewindInput>();

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>(); 
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this.m_PlayerMovementState = _PlayerState.PlayerMovementState;
            this.m_PlayerSpecialActionState = _PlayerState.PlayerSpecialActionState;
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        // -----------------------------------------------------
        // Dodge related Events
        // -----------------------------------------------------

        /* Refactor Notes:
         *  - States are discrete between a normal set of dodges and dodges from the LockOn
         *  - Dodge should be a facade but behaviour should be their own states
         *  - SpecialAction and undescriptive is too generic and should be its own specific 'Dodge' class
         * 
         */
        void IPlayerSpecialAction.Dodge()
        {
            // Run dodge when moving
            if (this.m_PlayerMovementState.MoveDirection != Vector3.zero 
                && !this.m_PlayerSpecialActionState.IsDodging 
                && this.m_PlayerSpecialActionState.CanDodge)
            {
                this.m_Animator.SetTrigger("Dodge");
                this.m_Animator.ResetTrigger("AttackLight");
                
                this.m_PlayerMovement.EnableMovement();
                this.m_PlayerMovement.EnableRotation();
                
                // TODO: If condition should be a check for the method
                if (this.m_PlayerAttackState.HasBeenParried)
                    this.m_PlayerAttackHandler.EndParryAction();

                // TODO: Change to point the player's locked state rather than the Cameras
                if (this.m_CameraController.IsLockedOn)
                {
                    StartCoroutine("DodgeImpulse");
                    StartCoroutine(
                        this.m_PlayerFunctions.DodgeImpulse(
                            new Vector3(
                                this.m_PlayerMovementState.MoveDirection.x,
                                0,
                                this.m_PlayerMovementState.MoveDirection.y),
                                this.m_DodgeForce
                        )
                    );
                }
                
                this.m_PlayerAttackHandler.ResetAttack();
            }
            // The below may not be feasable to trigger. But below runs if not able to dodge, is moving and is not dodging
            else if (this.m_PlayerMovementState.MoveDirection != Vector3.zero 
                     && !this.m_PlayerSpecialActionState.IsDodging 
                     && !this.m_PlayerSpecialActionState.CanDodge)
            {
                this.m_DodgeCache = true;
            }
            // Run this is dodge is triggered but no movement is toggled
            else if (this.m_PlayerMovementState.MoveDirection == Vector3.zero 
                     && !this.m_PlayerSpecialActionState.IsDodging 
                     && this.m_PlayerSpecialActionState.CanDodge)
            {
                this.m_Animator.SetTrigger("Dodge");
                this.m_Animator.ResetTrigger("AttackLight");
                
                this.m_PlayerMovement.EnableMovement();
                this.m_PlayerMovement.EnableRotation();
                
                if (this.m_PlayerAttackState.HasBeenParried)
                    this.m_PlayerAttackHandler.EndParryAction();

                // The below if statement is unusually repeated
                if (this.m_PlayerMovementState.MoveDirection == Vector3.zero 
                    && !this.m_PlayerSpecialActionState.IsDodging 
                    && this.m_PlayerSpecialActionState.CanDodge)
                {
                    StopCoroutine("DodgeImpulse");
                    StartCoroutine(this.m_PlayerFunctions.DodgeImpulse(
                                    new Vector3(0, 0, 1), 
                                    this.m_DodgeForce));
                }
                
                this.m_PlayerAttackHandler.ResetAttack();
            }
        }

        void IPlayerSpecialAction.ResetDodge()
        {
            this.m_PlayerSpecialActionState.CanDodge = true;
            this.m_PlayerSpecialActionState.IsDodging = false;
        }

        // -----------------------------------------------------
        // Rewind related Events
        // -----------------------------------------------------

        void IPlayerSpecialAction.ActivateRewind()
        { 
            
        }

        //=>   //this.m_RewindInput.OnInitRewind(); // This is temporary until a separate player handler is created.
        
        #endregion Methods

    }
    
}
