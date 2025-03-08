using UnityEngine;

public class TestEnemyAttackSystem : MonoBehaviour, IEnemyAttackSystem
{

    #region - - - - - - Methods - - - - - -

    public void HandleAttackDeflection()
    {
        Debug.Log("Handle enemy animation state.");
        Debug.Log("Enemy Guard Meter is updated");
    }

    #endregion Methods
  
}