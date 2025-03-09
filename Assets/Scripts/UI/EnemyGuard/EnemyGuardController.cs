using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

public class EnemyGuardController : PausableMonoBehaviour, IGuardMeter
{

    #region - - - - - - Fields - - - - - -

    private EnemyGuardView m_View;
    private Transform m_TargetEnemy;
    private Camera m_MainCamera;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        ILockOnObserver _LockOnObserver = SceneManager.Instance.LockOnObserver 
            ?? throw new ArgumentNullException(nameof(SceneManager.Instance.LockOnObserver));
        _LockOnObserver.OnNewLockOnTarget.AddListener(target => this.m_TargetEnemy = target);
    }
    
    private void Update()
    {
        if (this.IsPaused || !this.m_View.IsVisible) return;
        
        this.UpdateEnemyGuardPosition();
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void ShowEnemyGuardMeter()
        => this.m_View.ShowEnemyGuard();

    public void HideEnemyGuardMeter()
        => this.m_View.HideEnemyGuard();

    public void UpdateEnemyGuardMeter(float currentGuard, float maxGuard) 
        => this.m_View.UpdateEnemyGuardSlider(currentGuard / maxGuard);

    private void UpdateEnemyGuardPosition()
    {
        Vector2 _ScreenPosition = RectTransformUtility.WorldToScreenPoint(
            this.m_MainCamera, 
            this.m_TargetEnemy.position + new Vector3(0, 3.5f));
        
        this.m_View.UpdateEnemyGuardScreenPosition(_ScreenPosition);
    }

    #endregion Methods

}
