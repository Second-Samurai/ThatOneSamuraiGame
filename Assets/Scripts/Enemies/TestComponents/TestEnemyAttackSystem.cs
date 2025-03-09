using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

public class TestEnemyAttackSystem : MonoBehaviour, IEnemyAttackSystem
{

    #region - - - - - - Fields - - - - - -

    public float m_MaxGuard;
    public float m_GuardDamage;

    private float m_CurrentGuard;
    private IGuardMeter m_GuardMeter;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_GuardMeter = UserInterfaceManager.Instance.GuardMeter;
        this.m_CurrentGuard = this.m_MaxGuard;
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void HandleAttackDeflection()
    {
        Debug.Log("Handle enemy animation state.");
        
        this.m_CurrentGuard -= this.m_GuardDamage;
        this.m_CurrentGuard = Mathf.Clamp(this.m_CurrentGuard, 0, this.m_MaxGuard);
        
        if (!Mathf.Approximately(this.m_CurrentGuard, this.m_MaxGuard))
            this.m_GuardMeter.ShowEnemyGuardMeter();
        else if (this.m_CurrentGuard == 0)
            this.m_GuardMeter.HideEnemyGuardMeter();
        
        this.m_GuardMeter.UpdateEnemyGuardMeter(this.m_CurrentGuard, this.m_MaxGuard);
        
        Debug.Log("Enemy Guard Meter is updated");
    }

    #endregion Methods
  
}