using UnityEngine;

public class TestEnemyGuard : MonoBehaviour, IGuarding
{

    #region - - - - - - Fields - - - - - -

    public GameObject UIEnemyGuardMeter;

    #endregion Fields
  
    #region - - - - - - Properties - - - - - -

    public bool CanRunCooldownTimer { get; set; }
    
    public bool CanRunRecoveryTimer { get; set; }

    public IFloatingEnemyGuardMeter UIGuardMeter
        => this.UIEnemyGuardMeter.GetComponent<IFloatingEnemyGuardMeter>();

    #endregion Properties

}
