using UnityEngine;

// Note: This serves mostly as a mocked stub method.
public class TestEnemyUIGuard : MonoBehaviour, IFloatingEnemyGuardMeter
{

    #region - - - - - - Methods - - - - - -

    public void ShowFinisherKey() 
        => Debug.Log("Triggered to show the finisher key.");

    public void HideFinisherKey() 
        => Debug.Log("Triggered to hide the finisher key.");

    #endregion Methods
  
}
