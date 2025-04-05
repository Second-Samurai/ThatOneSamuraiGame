using UnityEngine;
using UnityEngine.Events;

public class ArcherAnimationReciever : MonoBehaviour
{

    #region - - - - - - Properties - - - - - -

    public UnityEvent OnIdleCompletion { get; } = new();

    #endregion Properties

    #region - - - - - - Methods - - - - - -

    public void CompleteIdleClip()
        => this.OnIdleCompletion.Invoke();

    #endregion Methods

}
