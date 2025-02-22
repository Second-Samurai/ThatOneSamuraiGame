using System.Linq;
using ThatOneSamuraiGame.GameLogging;
using UnityEngine;

public interface IPlayerAnimationDispatcher
{

    #region - - - - - - Methods - - - - - -

    bool Check(PlayerAnimationCheckState eventState);

    void Dispatch(PlayerAnimationEventStates eventState, bool boolValue = false, float floatValue = 0f, int intValue = 0);
    
    #endregion Methods

}

public interface IAnimationClipDurationProvider
{

    #region - - - - - - Methods - - - - - -

    float GetAnimationClipLength(string clipName);

    #endregion Methods

}

/// <summary>
/// Responsible for sending updates to the Player's animator.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour, IPlayerAnimationDispatcher, IAnimationClipDurationProvider
{

    #region - - - - - - Fields - - - - - -

    [RequiredField]
    [SerializeField]
    private Animator m_PlayerAnimator;

    #endregion Fields
    
    #region - - - - - - Methods - - - - - -

    public bool Check(PlayerAnimationCheckState eventState)
        => eventState.CheckAction.Invoke(this.m_PlayerAnimator);

    public void Dispatch(
        PlayerAnimationEventStates eventState, 
        bool boolValue = false, 
        float floatValue = 0f, 
        int intValue = 0) 
        => eventState.Run(this.m_PlayerAnimator, boolValue, floatValue, intValue);

    public float GetAnimationClipLength(string clipName)
    {
        if (this.m_PlayerAnimator.runtimeAnimatorController.animationClips.Any(ac => ac.name == clipName))
            return this.m_PlayerAnimator.runtimeAnimatorController.animationClips
                .First(ac => ac.name == clipName).length;
        
        GameLogger.LogError($"There is no animation clip under the name: '{clipName}'");
        return 0.2f; // Default fallback time
    }

    #endregion Methods

}
