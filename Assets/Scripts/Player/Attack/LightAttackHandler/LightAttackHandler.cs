using System.Collections;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

public class LightAttackHandler : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    private IPlayerMovement m_PlayerMovement;
    private IPlayerAnimationDispatcher m_AnimationDispatcher;

    private int m_AttackAnimationIndex;
    private bool m_IsAttackRunning;
    private bool m_IsAttackQueued;

    private float m_SprintAttackClipTimeLength;
    private float m_AttackVariantOneTimeLength;
    private float m_AttackVariationTwoTimeLength;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_AnimationDispatcher = this.GetComponent<IPlayerAnimationDispatcher>();
        this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
        
        IAnimationClipDurationProvider _ClipDurationProvider = this.GetComponent<IAnimationClipDurationProvider>();
        this.m_SprintAttackClipTimeLength = _ClipDurationProvider
            .GetAnimationClipLength(PlayerAnimationEventStates.SprintAttack.ClipName);
        this.m_AttackVariantOneTimeLength = _ClipDurationProvider
            .GetAnimationClipLength($"1st{PlayerAnimationEventStates.AttackLight.ClipName}"); // Enum state clip-name is generic
        this.m_AttackVariationTwoTimeLength = _ClipDurationProvider
            .GetAnimationClipLength($"2nd{PlayerAnimationEventStates.AttackLight.ClipName}"); // Enum state clip-name is generic
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void QueueLightAttack()
    {
        if (this.m_IsAttackRunning)
        {
            this.m_IsAttackQueued = true;
            return;
        }

        this.StartCoroutine(this.PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        if (this.m_PlayerMovement.IsSprinting)
        {
            this.m_AnimationDispatcher.Dispatch(PlayerAnimationEventStates.SprintAttack);
            yield return new WaitForSeconds(this.m_SprintAttackClipTimeLength);
        }
        else
        {
            // Run alternating attack animation sequences
            this.m_AnimationDispatcher.Dispatch(
                PlayerAnimationEventStates.AttackLight, 
                intValue: this.m_AttackAnimationIndex % 2 == 0 ? 1 : 2);

            yield return new WaitForSeconds(this.m_AttackAnimationIndex % 2 == 0
                ? this.m_AttackVariantOneTimeLength
                : this.m_AttackVariationTwoTimeLength);
            
            this.m_AttackAnimationIndex++;
        }

        this.m_IsAttackRunning = false;

        if (this.m_IsAttackQueued) yield break;
        this.m_IsAttackQueued = false;
        this.StartCoroutine(this.PerformAttack());
    }

    #endregion Methods
  
}
