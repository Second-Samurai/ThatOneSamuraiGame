using System;
using ThatOneSamuraiGame.Scripts.Camera;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.SpecialAction
{
    
    public class PlayerSpecialAction : MonoBehaviour, IPlayerSpecialAction
    {

        #region - - - - - - Fields - - - - - -

        private ICameraController m_CameraController;
        private ICombatController m_CombatController;
        private PlayerAttackState m_PlayerAttackState;
        private IPlayerAttackHandler m_PlayerAttackHandler;
        private IDamageable m_PlayerDamageHandler;
        private IPlayerMovement m_PlayerMovement;
        
        private Animator m_Animator;
        private bool m_CanDodge = true;
        private float m_DodgeForce = 10f;
        private bool m_DodgeCache = false; // This field name makes no sense. Needs to be renamed
        private bool m_IsDodging = false;
        private PlayerFunctions m_PlayerFunctions;

        #endregion Fields

        #region - - - - - - Properties - - - - - -

        bool IPlayerSpecialAction.CanDodge
        {
            get => this.m_CanDodge;
            set => this.m_CanDodge = value;
        }

        bool IPlayerSpecialAction.IsDodging
        {
            get => this.m_IsDodging;
            set => this.m_IsDodging = value;
        }

        #endregion Properties
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_Animator = this.GetComponent<Animator>();
            this.m_CameraController = this.GetComponent<ICameraController>();
            this.m_CombatController = this.GetComponent<ICombatController>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;
            this.m_PlayerAttackHandler = this.GetComponent<IPlayerAttackHandler>();
            this.m_PlayerDamageHandler = this.GetComponent<IDamageable>();
            this.m_PlayerFunctions = this.GetComponent<PlayerFunctions>();
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -

        void IPlayerSpecialAction.Dodge()
        {
            if (this.m_PlayerMovement.MoveDirection != Vector3.zero && !this.m_IsDodging && this.m_CanDodge)
            {
                this.m_Animator.SetTrigger("Dodge");
                this.m_Animator.ResetTrigger("AttackLight");
                
                this.m_PlayerMovement.EnableMovement();
                this.m_PlayerMovement.EnableRotation();
                
                if (this.m_PlayerAttackState.HasBeenParried)
                    this.m_PlayerAttackHandler.EndParryAction();

                if (this.m_CameraController.IsLockedOn)
                {
                    StartCoroutine("DodgeImpulse");
                    StartCoroutine(
                        this.m_PlayerFunctions.DodgeImpulse(
                            new Vector3(
                                this.m_PlayerMovement.MoveDirection.x,
                                0,
                                this.m_PlayerMovement.MoveDirection.y),
                                this.m_DodgeForce
                        )
                    );
                }
                
                this.m_PlayerAttackHandler.ResetAttack();
            }
            else if (this.m_PlayerMovement.MoveDirection != Vector3.zero && !this.m_IsDodging && !this.m_CanDodge)
            {
                this.m_DodgeCache = true;
            }
            else if (this.m_PlayerMovement.MoveDirection == Vector3.zero && !this.m_IsDodging && this.m_CanDodge)
            {
                this.m_Animator.SetTrigger("Dodge");
                this.m_Animator.ResetTrigger("AttackLight");
                
                this.m_PlayerMovement.EnableMovement();
                this.m_PlayerMovement.EnableRotation();
                
                if (this.m_PlayerAttackState.HasBeenParried)
                    this.m_PlayerAttackHandler.EndParryAction();

                if (this.m_PlayerMovement.MoveDirection == Vector3.zero && !this.m_IsDodging && this.m_CanDodge)
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
            this.m_CanDodge = true;
            this.m_IsDodging = false;
        }

        private void StartDodge() // Accessible outside
        {
            this.m_PlayerAttackState.IsWeaponSheathed = false;
            // this.m_PlayerAttackState.PlayGleam = true; // This should be an event
            // this.m_PlayerAttackState.IsHeavyAttackChargin = false;
            this.m_PlayerAttackState.CanAttack = false;
            this.m_IsDodging = true;
            
            this.m_PlayerDamageHandler.DisableDamage();
            this.m_PlayerFunctions.DisableBlock();
            this.m_PlayerAttackHandler.ResetAttack();
        }

        private void EndDodge() // Accessible outside
        {
            this.m_PlayerAttackState.CanAttack = true;
            this.m_IsDodging = false;
            
            this.m_PlayerDamageHandler.EnableDamage();
            this.m_PlayerFunctions.EnableBlock();
            
            this.m_PlayerAttackHandler.ResetAttack();
        }

        #endregion Methods
        
    }
    
}
