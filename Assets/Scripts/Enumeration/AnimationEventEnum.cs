using System;
using ThatOneSamuraiGame.Scripts.Enumeration;
using UnityEngine;

public class AnimationEventsEnum : SmartEnum
{

    #region - - - - - - Constructors - - - - - -

    public AnimationEventsEnum(
        string name,
        int value,
        string clipName,
        Action<Animator, bool, float, int, Vector2> action)
        : base(name, value)
    {
        this.Action = action;
        this.ClipName = clipName;
    }

    #endregion Constructors
  
    #region - - - - - - Properties - - - - - -

    private Action<Animator, bool, float, int, Vector2> Action { get; set; }

    public string ClipName { get; private set; }

    #endregion Properties
    
    #region - - - - - - Methods - - - - - -

    public void Run(Animator animator, bool boolValue = false, float floatValue = 0f, int intValue = 0, Vector2 vector2Value = new())
        => this.Action.Invoke(animator, boolValue, floatValue, intValue, vector2Value);

    #endregion Methods
    
}
