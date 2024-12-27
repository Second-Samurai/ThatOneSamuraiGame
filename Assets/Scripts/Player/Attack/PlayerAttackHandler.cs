using System;
using Player.Animation;
using ICameraController = ThatOneSamuraiGame.Legacy.ICameraController;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThatOneSamuraiGame.Scripts.Player.Attack
{
    
    public class PlayerAttackHandler : PausableMonoBehaviour, IPlayerAttackHandler
    {

        #region - - - - - - Fields - - - - - -

        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private Legacy.ICameraController m_CameraController;
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
        private bool m_HeavyInputHeld = false;
        private float m_HeavyAttackTimer;
        private float m_HeavyAttackMaxTimer = 2f;

        private bool m_GleamTimerFinished = false;
        private float m_GleamTimer;
        private float m_GleamPrecedeTime = 0.4f;
        //private bool m_HasPerformedAttack;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerAnimationComponent = this.GetComponent<PlayerAnimationComponent>();
            this.m_CameraController = this.GetComponent<Legacy.ICameraController>();
            this.m_CombatController = this.GetComponent<ICombatController>();
            // this.m_HitstopController = GameManager.instance.GetComponent<HitstopController>();
            this.m_HitstopController = Object.FindFirstObjectByType<HitstopController>();
            this.m_PlayerFunctions = this.GetComponent<PlayerFunctions>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;

            this.m_HeavyAttackTimer = this.m_HeavyAttackMaxTimer;
            m_GleamTimer = m_HeavyAttackMaxTimer - m_GleamPrecedeTime;
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
        
        
        // NOTE: IPlayerAttackHandler.Attack() is a release input option (e.g. OnMouseUp)
        void IPlayerAttackHandler.Attack()
        {
            // If the player was holding down the button (i.e. heavy attacking), don't perform a light attack.
            // This is different from m_PlayerAttackState.IsHeavyAttackCharging, as a heavy attack could
            // execute and the mouse button could still be held down
            if (m_HeavyInputHeld)
            {
                m_HeavyInputHeld = false;
            }
            else if (this.m_PlayerAttackState.CanAttack)
            {
                this.m_CombatController.AttemptLightAttack();
                //this.m_HasPerformedAttack = true;
                
                if (this.m_HitstopController.bIsSlowing)
                    this.m_HitstopController.CancelEffects();
            }
            
            //this.m_HasPerformedAttack = false;
        }

        void IPlayerAttackHandler.DrawSword() 
            => this.m_CombatController?.DrawSword();

        // Tech-Debt: #35 - PlayerFunctions will be refactored to mitigate large class bloat.
        void IPlayerAttackHandler.EndBlock()
            => this.m_PlayerFunctions.bInputtingBlock = false;

        void IPlayerAttackHandler.EndParryAction()
        {
            this.m_PlayerAttackState.ParryStunned = false;
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
        
        private bool CanBlock() => m_CombatController.IsSwordDrawn() && this.m_CanBlock;
        
        void IPlayerAttackHandler.StartBlock()
        {
            if (!CanBlock())
                return;

            if (this.m_PlayerAttackState.ParryStunned)
                ((IPlayerAttackHandler)this).EndParryAction();
            
            this.m_PlayerFunctions.StartBlock();
        }
        
        void IPlayerAttackHandler.ResetAttack()
        {
            this.m_PlayerAttackState.CanAttack = true;
            //this.m_HasPerformedAttack = false;
            
            this.m_CombatController.EndAttacking();
            this.m_CombatController.ResetAttackCombo();
        }
        
        private void StartHeavyAttack()
        {
            if (!this.m_PlayerAttackState.CanAttack)
                return;
            
            this.m_ShowHeavyTelegraphEvent.Raise();

            this.m_HeavyInputHeld = true;
            this.m_PlayerAttackState.IsWeaponSheathed = true;
            this.m_PlayerAttackState.IsHeavyAttackCharging = true;
            
            this.m_PlayerAnimationComponent.ResetAttackParameters();
            this.m_PlayerAnimationComponent.ChargeHeavyAttack(true);

            this.m_CameraController.RollCamera();
        }
        
        private void PerformHeavyAttack()
        {
            this.m_ShowHeavyTutorialEvent.Raise();
            this.m_EndHeavyTelegraphEvent.Raise();

            this.m_PlayerAttackState.IsHeavyAttackCharging = false;
            this.m_PlayerAttackState.IsWeaponSheathed = false;
            this.m_GleamTimerFinished = false;
            
            this.m_CameraController.ResetCameraRoll();
            
            this.m_PlayerAnimationComponent.TriggerHeavyAttack();
        }
        
        private void TickHeavyTimer()
        {
            CountdownHeavyTimer();
            
            if(!m_GleamTimerFinished)
                CountdownGleamTimer();
        }

        // Tech-Debt: #36 - Create a simple universal timer to keep timer behaviour consistent.
        private void CountdownHeavyTimer()
        {
            this.m_HeavyAttackTimer -= Time.deltaTime;
            
            if (!(this.m_HeavyAttackTimer <= 0)) return;
            
            //this.m_HasPerformedAttack = true;
            this.m_HeavyAttackTimer = this.m_HeavyAttackMaxTimer;
            this.PerformHeavyAttack();
        }
        
        private void CountdownGleamTimer()
        {
            m_GleamTimer -= Time.deltaTime;
            
            if (!(m_GleamTimer <= 0)) return;
            
            m_GleamTimerFinished = true;
            m_GleamTimer = m_HeavyAttackMaxTimer - m_GleamPrecedeTime;
            m_PlayerFunctions.parryEffects.PlayGleam();
        }
        
        #endregion Methods
        
    }
    
}
