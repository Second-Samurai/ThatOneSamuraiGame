using System;
using UnityEngine;

namespace Player.Animation
{
    /// <summary>
    /// Responsible for handling animation parameters for the player.
    /// </summary>
    public class PlayerAnimationComponent : MonoBehaviour
    {
        #region - - - - - - Animation Parameter Strings to Hashes - - - - - -
        
        // Movement Parameters
        //private static readonly string IsSprinting = "IsSprinting";
        private static readonly string XInput = "XInput";
        private static readonly string YInput = "YInput";
        private static readonly string InputSpeed = "InputSpeed";
        
        // Attack Parameters
        private static readonly string AttackLight = "AttackLight";
        private static readonly string FirstAttack = "FirstAttack";
        private static readonly string SecondAttack = "SecondAttack";
        private static readonly string LoopAttack = "LoopAttack";
        private static readonly string HeavyAttackHeld = "HeavyAttackHeld";
        private static readonly string ComboCount = "ComboCount";
        
        // Miscellaneous Parameters
        private static readonly string FinisherSetup = "FinisherSetup";
        private static readonly string IsParried = "IsParried";
        private static readonly string IsDead = "IsDead";
        private static readonly string VGuard = "VGuard";
        private static readonly string LockedOn = "LockedOn";
        private static readonly string DrawSword = "DrawSword";
        
        #endregion Animation Parameter Strings to Hashes 

        #region - - - - - - Fields - - - - - -
        
        private Animator m_Animator;
        
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
        
        [System.Obsolete("This is an obsolete method, modify InputSpeed instead")]
        public void SetSprinting(bool isSprinting)
        {
            //m_Animator.SetBool(IsSprinting, isSprinting);
        }

        public void TriggerDrawSword()
        {
            //m_Animator.SetBool("IsDrawn", true);
            m_Animator.SetTrigger(DrawSword);
            ResetLightAttack();
        }

        public void TriggerLightAttack()
        {
            m_Animator.SetTrigger(AttackLight);
        }
        
        public void ResetLightAttack()
        {
            m_Animator.ResetTrigger(AttackLight);
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
        }

        /// <summary>
        /// Determine what walking/running animation should be played based on inputSpeed
        /// </summary>
        /// <param name="inputSpeed">The speed parameter the movement blend tree uses to determine what animation
        /// is to be player. Sprint multiplier is stored in PlayerMovement </param>
        /// <param name="movementSmoothingDampingTime"></param>
        // INPUT SPEED
        // < 1 walking
        // == 1 jogging
        // > 1 sprinting
        public void SetInputSpeed(float inputSpeed, float movementSmoothingDampingTime = 0)
        {
            m_Animator.SetFloat(
                InputSpeed,
                inputSpeed,
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
                m_Animator.GetBool(IsDead), 
                m_Animator.GetBool(HeavyAttackHeld)/*, 
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
            m_Animator.SetBool(HeavyAttackHeld, animationTimeData.HeavyAttackHeld);
            m_Animator.SetBool(FinisherSetup, animationTimeData.FinisherSetup);
        }
        
        public void SetAnimationOverride(int currentClip, float currentFrame)
        {
            m_Animator.Play(currentClip, 0, currentFrame);
        }
        
        #endregion Rewind Functions
    }
}
