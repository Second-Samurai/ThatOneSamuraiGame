using UnityEngine;

public interface IPlayerAnimationDispatcher
{

    #region - - - - - - Methods - - - - - -

    bool Check(PlayerAnimationCheckState eventState);

    void Dispatch(PlayerAnimationEventStates eventState, bool boolValue = false, float floatValue = 0f, int intValue = 0);
    
    #endregion Methods

}

/// <summary>
/// Responsible for sending updates to the Player's animator.
/// </summary>
[RequireComponent(typeof(Animator))]
public class PlayerAnimationDispatcher : MonoBehaviour, IPlayerAnimationDispatcher
{

    #region - - - - - - Fields - - - - - -

    [RequiredField]
    [SerializeField]
    private Animator m_PlayerAnimator;

    #endregion Fields
    
    // ************************************************************
    // IMPORTANT: 
    // Optional Action<> arguments have not been implemented into the Animation event.
    // The state implementation defines the usage but in here we default any boolean to false and floats to 0.0f.
    // ************************************************************

    #region - - - - - - Methods - - - - - -

    public bool Check(PlayerAnimationCheckState eventState)
        => eventState.CheckAction.Invoke(this.m_PlayerAnimator);

    public void Dispatch(
        PlayerAnimationEventStates eventState, 
        bool boolValue = false, 
        float floatValue = 0f, 
        int intValue = 0) 
        => eventState.Run(this.m_PlayerAnimator, boolValue, floatValue, intValue);

    #endregion Methods

}
