using System;
using UnityEngine;

namespace Player.Animation
{
    /// <summary>
    /// Responsible for handling animation parameters for the player.
    /// </summary>
    public class PlayerAnimationComponent : MonoBehaviour
    {
        #region - - - - - - Animation Parameter Strings - - - - - -
        
        // Movement Parameters
        private static readonly string IsSprinting = "IsSprinting";
        private static readonly string XInput = "XInput";
        private static readonly string YInput = "YInput";
        private static readonly string InputSpeed = "InputSpeed";
        
        // Attack Parameters
        private static readonly string SprintAttack = "SprintAttack";
        private static readonly string AttackLight = "AttackLight";
        private static readonly string FirstAttack = "FirstAttack";
        private static readonly string SecondAttack = "SecondAttack";
        private static readonly string LoopAttack = "LoopAttack";
        private static readonly string StartHeavy = "StartHeavy";
        private static readonly string AttackHeavy = "AttackHeavy";
        private static readonly string ComboCount = "ComboCount";
        
        // Miscellaneous Triggers
        private static readonly string DrawSword = "DrawSword";
        private static readonly string FinisherSetup = "FinisherSetup";
        private static readonly string Parrying = "Parrying";
        private static readonly string Dodge = "Dodge";
        private static readonly string GuardBreak = "GuardBreak";
        private static readonly string Knockdown = "Knockdown";
        
        // Miscellaneous Bools
        private static readonly string IsParried = "IsParried";
        private static readonly string IsDead = "IsDead";
        private static readonly string VGuard = "VGuard";
        private static readonly string LockedOn = "LockedOn";
        
        
        #endregion Animation Parameter Strings 

        #region - - - - - - Fields - - - - - -
        
        private Animator m_Animator;
        private float m_MinClampToZero = 0.0000001f;
        
        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Unity Animator Variable Changes - - - - - -
        
        public void SetFireEvents(bool doFireEvents) => m_Animator.fireEvents = doFireEvents;
        
        public void SetRootMotion(bool doRootMotion) => m_Animator.applyRootMotion = doRootMotion;

        #endregion Unity Animator Variable Changes
        
        #region - - - - - - Parameter Change Functions - - - - - -
        
        public void SetSprinting(bool isSprinting)
        {
            m_Animator.SetBool(IsSprinting, isSprinting);
        }

        public void TriggerDrawSword()
        {
            //m_Animator.SetBool("IsDrawn", true);
            m_Animator.SetTrigger(DrawSword);
            ResetLightAttack();
        }

        public void TriggerSprintAttack()
        {
            m_Animator.SetTrigger(SprintAttack);
        }

        public void TriggerLightAttack(int attackVariant)
        {
            m_Animator.SetTrigger(AttackLight);

            switch (attackVariant)
            {
                case 1:
                    m_Animator.SetBool(FirstAttack, true);
                    m_Animator.SetBool(SecondAttack, false);
                    break;
                case 2:
                    m_Animator.SetBool(FirstAttack, false);
                    m_Animator.SetBool(SecondAttack, true);
                    break;
                default:
                    break;
            }
        }
        
        public void ResetLightAttack()
        {
            m_Animator.ResetTrigger(AttackLight);
        }

        public void ChargeHeavyAttack(bool isHeavyAttacking)
        {
            if (isHeavyAttacking)
                m_Animator.SetTrigger(StartHeavy);
        }

        public void TriggerHeavyAttack()
        {
            m_Animator.SetTrigger(AttackHeavy);
        }
        
        public void ResetAttackParameters()
        {
            m_Animator.SetBool(FirstAttack, false);
            m_Animator.SetBool(SecondAttack, false);
            //m_Animator.SetBool(LoopAttack, false);
        }

        public void TriggerIsParried()
        {
            m_Animator.SetTrigger(IsParried);
        }
        
        public void SetDead(bool isDead)
        {
            m_Animator.SetBool(IsDead, isDead);
        }

        public void TriggerParry()
        {
            m_Animator.SetTrigger(Parrying);
        }

        public void TriggerDodge()
        {
            m_Animator.SetTrigger(Dodge);
        }

        public void TriggerGuardBreak()
        {
            m_Animator.SetTrigger(GuardBreak);
        }

        public void TriggerKnockdown()
        {
            m_Animator.SetTrigger(Knockdown);
        }

        public void TriggerFinisher()
        {
            m_Animator.SetTrigger(FinisherSetup);
        }
        
        public void SetInputDirection(Vector2 inputDirection, float movementSmoothingDampingTime = 0)
        {
            m_Animator.SetFloat(
                XInput, 
                inputDirection.x, 
                movementSmoothingDampingTime, 
                Time.deltaTime);
            
            m_Animator.SetFloat(
                YInput,
                inputDirection.y,
                movementSmoothingDampingTime,
                Time.deltaTime);
            
            // Clamp small values to zero to prevent near-zero issues
            if (Mathf.Abs(inputDirection.x) < m_MinClampToZero)
                m_Animator.SetFloat(XInput, 0f);
            
            if (Mathf.Abs(inputDirection.y) < m_MinClampToZero)
                m_Animator.SetFloat(YInput, 0f);
        }

        /// <summary>
        /// Determine what walking/running animation should be played based on inputSpeed
        /// </summary>
        /// <param name="inputSpeed">The speed parameter the movement blend tree uses to determine what animation
        /// is to be player. Sprint multiplier is stored in PlayerMovement </param>
        /// <param name="sprintMultiplier"> Multiplied to inputSpeed to determine the speed</param>
        /// <param name="movementSmoothingDampingTime"></param>
        public void SetInputSpeed(float inputSpeed, float sprintMultiplier, float movementSmoothingDampingTime = 0)
        {
            // INPUT SPEED THRESHOLDS
            // x < 0.25 Walking
            // 0.25 < x < 0.5 Jogging (Moving with no sprint)
            // x == 1 Sprinting
            
            m_Animator.SetFloat(
                InputSpeed,
                inputSpeed * sprintMultiplier,
                movementSmoothingDampingTime,
                Time.deltaTime);
        }
        
        #endregion Parameter Changes
        
        #region - - - - - - Rewind Functions - - - - - -

        public AnimationTimeData GetAnimationTimeData()
        {
            return new AnimationTimeData(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 
                m_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 
                m_Animator.GetFloat(InputSpeed), 
                m_Animator.GetFloat(XInput), 
                m_Animator.GetFloat(YInput), 
                m_Animator.GetBool(LockedOn), 
                m_Animator.GetBool(VGuard), 
                //m_Animator.GetInteger(ComboCount), 
                m_Animator.GetBool(FirstAttack), 
                m_Animator.GetBool(SecondAttack), 
                //m_Animator.GetBool(LoopAttack), 
                m_Animator.GetBool(IsDead)/*, 
                m_Animator.GetBool(HeavyAttackHeld), 
                m_Animator.GetBool(FinisherSetup)*/);
        }
        
        public void SetAnimationTimeData(AnimationTimeData animationTimeData)
        {
            // Other variables from AnimationTimeData can be set, but these were the ones listed in RewindEntity
            m_Animator.SetBool(LockedOn, animationTimeData.lockedOn);
            m_Animator.SetBool(VGuard, animationTimeData.vGuard);
            m_Animator.SetInteger(ComboCount, animationTimeData.comboCount);
            m_Animator.SetBool(FirstAttack, animationTimeData.firstAttack);
            m_Animator.SetBool(SecondAttack, animationTimeData.secondAttack);
            m_Animator.SetBool(LoopAttack, animationTimeData.loopAttack);
            //m_Animator.SetBool(HeavyAttackHeld, animationTimeData.HeavyAttackHeld);
            m_Animator.SetBool(FinisherSetup, animationTimeData.FinisherSetup);
        }
        
        public void SetAnimationOverride(int currentClip, float currentFrame)
        {
            m_Animator.Play(currentClip, 0, currentFrame);
        }
        
        #endregion Rewind Functions
    }
}
