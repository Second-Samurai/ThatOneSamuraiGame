using System;
using ThatOneSamuraiGame.Scripts.UI.UserInterfaceManager;
using UnityEngine;

public class TestEnemyAttackSystem : MonoBehaviour, IEnemyAttackSystem, IDebuggable<DebuggingAttackDeflectionInfo>
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
        
        if (this.m_CurrentGuard == 0)
            this.m_GuardMeter.HideEnemyGuardMeter();
        else if (!Mathf.Approximately(this.m_CurrentGuard, this.m_MaxGuard))
            this.m_GuardMeter.ShowEnemyGuardMeter();
        
        this.m_GuardMeter.UpdateEnemyGuardMeter(this.m_CurrentGuard, this.m_MaxGuard);
        
        Debug.Log("Enemy Guard Meter is updated");
    }

    #endregion Methods

    #region - - - - - - Debugging Methods - - - - - -

    void IDebuggable<DebuggingAttackDeflectionInfo>.DebugInvoke()
    {
    }

    DebuggingAttackDeflectionInfo IDebuggable<DebuggingAttackDeflectionInfo>.GetDebugInfo()
        => new()
        {
            CurrentGuard = this.m_CurrentGuard,
            GuardDamage = this.m_GuardDamage,
            MaxGuard = this.m_MaxGuard
        };

    #endregion Debugging Methods

}

public class DebuggingAttackDeflectionInfo
{

    #region - - - - - - Properties - - - - - -

    public float CurrentGuard { get; set; }

    public float GuardDamage { get; set; }

    public float MaxGuard { get; set; }

    #endregion Properties
  
}