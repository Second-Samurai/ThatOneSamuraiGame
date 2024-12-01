using System;
using ThatOneSamuraiGame.Legacy;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    public class PlayerAttackHandler : PausableMonoBehaviour, IPlayerAttackHandler
    {

        #region - - - - - - Fields - - - - - -

        private Animator m_Animator;
        private ICameraController m_CameraController;
        private ICombatController m_CombatController;
        private HitstopController m_HitstopController;
        private PlayerFunctions m_PlayerFunctions;
        private PlayerAttackState m_PlayerAttackState;

        [SerializeField] 
        private GameEvent m_ShowHeavyTutorialEvent; // This event feels out of place for this component.
        [SerializeField] 
        private GameEvent m_ShowHeavyTelegraphEvent; // This event feels out of place for this component.
        [SerializeField] 
        private GameEvent m_EndHeavyTelegraphEvent; // This event feels out of place for this component. 

        private bool m_CanBlock = true;
        private float m_HeavyAttackTimer;
        private float m_HeavyAttackMaxTimer = 2f;
        private bool m_HasPerformedAttack;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_Animator = this.GetComponent<Animator>();
            this.m_CameraController = this.GetComponent<ICameraController>();
            this.m_CombatController = this.GetComponent<ICombatController>();
            // this.m_HitstopController = GameManager.instance.GetComponent<HitstopController>();
            this.m_HitstopController = Object.FindFirstObjectByType<HitstopController>();
            this.m_PlayerFunctions = this.GetComponent<PlayerFunctions>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;

            this.m_HeavyAttackTimer = this.m_HeavyAttackMaxTimer;
        }

        private void Update()
        {
            if (this.IsPaused)
                return;
            
            if (this.m_PlayerAttackState.IsHeavyAttackCharging) 
                this.TickHeavyTimer();
            else if (Mathf.Approximately(this.m_HeavyAttackTimer, this.m_HeavyAttackMaxTimer))
                this.m_HeavyAttackTimer = this.m_HeavyAttackMaxTimer;
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -

        void IPlayerAttackHandler.Attack()
        {
            // Perform main attack            
            if (this.m_PlayerAttackState.CanAttack && !this.m_HasPerformedAttack)
            {
                this.m_CombatController.RunLightAttack();
                this.m_HasPerformedAttack = true;
                
                if (this.m_HitstopController.bIsSlowing)
                    this.m_HitstopController.CancelEffects();
            }
            else if (this.m_HasPerformedAttack)
                this.m_HasPerformedAttack = false;
            
            // Perform Heavy attack
            if (!this.m_Animator.GetBool("HeavyAttackHeld")) return;
            this.PerformHeavyAttack();
            this.m_HasPerformedAttack = false;
        }

        void IPlayerAttackHandler.DrawSword() 
            => this.m_CombatController?.DrawSword();

        // Tech-Debt: #35 - PlayerFunctions will be refactored to mitigate large class bloat.
        void IPlayerAttackHandler.EndBlock()
            => this.m_PlayerFunctions.bInputtingBlock = false;

        void IPlayerAttackHandler.EndParryAction()
        {
            this.m_PlayerAttackState.HasBeenParried = false;
            this.m_HitstopController.CancelEffects();
        }
        
        void IPlayerAttackHandler.StartHeavy()
        {
            this.m_HeavyAttackTimer = this.m_HeavyAttackMaxTimer;
            this.StartHeavyAttack();
        }

        // Note: This behaviour is not implemented, but will be open for future use.
        void IPlayerAttackHandler.StartHeavyAlternative() 
            => throw new NotImplementedException();

        void IPlayerAttackHandler.StartBlock()
        {
            if (!this.m_CanBlock)
                return;

            if (this.m_PlayerAttackState.HasBeenParried)
                ((IPlayerAttackHandler)this).EndParryAction();
            
            this.m_PlayerFunctions.StartBlock();
        }
        
        void IPlayerAttackHandler.ResetAttack()
        {
            this.m_PlayerAttackState.CanAttack = true;
            this.m_HasPerformedAttack = false;
            
            this.m_CombatController.EndAttacking();
            this.m_CombatController.ResetAttackCombo();
        }

        private void PerformHeavyAttack()
        {
            this.m_ShowHeavyTutorialEvent.Raise();
            this.m_EndHeavyTelegraphEvent.Raise();

            this.m_PlayerAttackState.IsHeavyAttackCharging = false;
            this.m_PlayerAttackState.IsWeaponSheathed = false;

            // Create gleam effect
            this.m_PlayerFunctions.parryEffects.PlayGleam();
            
            this.m_Animator.SetBool("HeavyAttackHeld", false);
            this.m_CameraController.ResetCameraRoll();
        }

        private void StartHeavyAttack()
        {
            if (!this.m_PlayerAttackState.CanAttack && this.m_Animator.GetBool("HeavyAttackHeld"))
                return;
            
            this.m_ShowHeavyTelegraphEvent.Raise();

            this.m_PlayerAttackState.IsWeaponSheathed = true;
            this.m_PlayerAttackState.IsHeavyAttackCharging = true;
            this.m_Animator.SetBool("HeavyAttackHeld", true);

            this.m_CameraController.RollCamera();
        }

        // Tech-Debt: #36 - Create a simple universal timer to keep timer behaviour consistent.
        private void TickHeavyTimer()
        {
            this.m_HeavyAttackTimer -= Time.deltaTime;
            
            if (!(this.m_HeavyAttackTimer <= 0)) return;
            this.m_HasPerformedAttack = true;
            this.m_HeavyAttackTimer = this.m_HeavyAttackMaxTimer;
            this.PerformHeavyAttack();
        }

        #endregion Methods
        
    }
    
}
