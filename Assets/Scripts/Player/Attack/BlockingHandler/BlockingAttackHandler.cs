using Player.Animation;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using UnityEngine;

namespace ThatOneSamuraiGame
{

    public class BlockingAttackHandler : PausableMonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [Header("Block Variables")]
        [SerializeField] private float m_BlockCooldownLength;
        [SerializeField] public ParryEffects m_BlockingEffects;
        
        // Components
        private PlayerSFX m_PlayerSFX;
        private IKPuppet m_IKPuppet;
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private ParryAttackHandler m_ParryAttackHandler;
        private PlayerWeaponSystem m_PlayerWeaponSystem;
        private PlayerAttackState m_PlayerAttackState;
        
        // Block flags
        private bool m_IsBlockingEnabled = true;
        private bool m_IsBlocking;
        private float m_CurrentBlockCooldownTime;

        #endregion Fields
  
        #region - - - - - - Unity Methods - - - - - -

        private void Start()
        {
            this.m_ParryAttackHandler = this.GetComponent<ParryAttackHandler>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerState>().PlayerAttackState;
            this.m_PlayerSFX = this.GetComponent<PlayerSFX>();
            this.m_PlayerWeaponSystem = this.GetComponent<PlayerWeaponSystem>();
            this.m_IKPuppet = this.GetComponent<IKPuppet>();
        }
        
        private void Update()
        {
            if (this.IsPaused) return;
            this.RunBlockCooldown();
        }

        #endregion Unity Methods

        #region - - - - - - Block Action Methods - - - - - -

        public void StartBlock()
        {
            if (!this.CanBlock())
                return;

            if (this.m_IsBlocking && this.m_CurrentBlockCooldownTime >= 0f && !this.m_IsBlockingEnabled)
            {
                Debug.LogError("bIsblocking: " + this.m_IsBlocking + " blockTimer: " + this.m_CurrentBlockCooldownTime + " bcanBlock: " + this.m_IsBlockingEnabled);
                return;
            }
            
            if (this.m_PlayerAttackState.ParryStunned)
                ((IPlayerAttackSystem)this).EndParryAction();
            
            this.m_PlayerSFX.Armour();
            this.m_BlockingEffects.PlayGleam();
            this.m_IKPuppet.EnableIK();
            this.m_ParryAttackHandler.StopParry();
            
            this.m_IsBlocking = true;
        }

        public void HandleBlockHit(out bool _IsHitBlocked)
        {
            if (!this.m_IsBlocking)
            {
                _IsHitBlocked = false;
                return;
            }
            
            //rotate to face attacker
            this.m_BlockingEffects.PlayBlock();
            this.m_IsBlocking = false;
            this.m_PlayerAnimationComponent.TriggerGuardBreak();
            this.m_IKPuppet.DisableIK();

            _IsHitBlocked = true;
        }
        
        public void EndBlock()
        {
            if (!this.m_IsBlocking) return;
            
            this.m_IsBlocking = false;
            this.m_ParryAttackHandler.StopParry();
            this.m_IKPuppet.DisableIK();
            this.ResetBlockCooldown();
        }

        #endregion Block Action Methods

        #region - - - - - - Methods - - - - - -

        private void RunBlockCooldown()
        {
            if (this.m_CurrentBlockCooldownTime <= 0f)
            {
                this.m_CurrentBlockCooldownTime = 0f;
                return;
            }
            
            this.m_CurrentBlockCooldownTime -= Time.deltaTime;
        }
        
        private void ResetBlockCooldown() 
            => this.m_CurrentBlockCooldownTime = m_BlockCooldownLength;

        public void DisableBlock()
        {
            this.m_IsBlockingEnabled = false;
            this.m_IKPuppet.DisableIK();
        }

        public void EnableBlock() 
            => this.m_IsBlockingEnabled = true;
        
        public bool CanBlock() => this.m_PlayerWeaponSystem.IsWeaponDrawn && this.m_IsBlockingEnabled;

        #endregion Methods
  
    }

}