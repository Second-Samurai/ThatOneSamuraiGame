using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

public class EnemyGuardController : PausableMonoBehaviour, IGuardMeter
{

    #region - - - - - - Fields - - - - - -

    private EnemyGuardView m_View;
    private Camera m_MainCamera;
    
    private Transform m_TargetEnemy;
    private Transform m_ReferencePlayer;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_View = this.GetComponent<EnemyGuardView>();
        
        ILockOnObserver _LockOnObserver = SceneManager.Instance.LockOnObserver 
            ?? throw new ArgumentNullException(nameof(SceneManager.Instance.LockOnObserver));
        _LockOnObserver.OnNewLockOnTarget.AddListener(target => this.m_TargetEnemy = target);

        this.m_ReferencePlayer = SceneManager.Instance.SceneState.ActivePlayer.transform;
    }
    
    private void Update()
    {
        if (this.IsPaused || !this.m_View.IsVisible) return;

        if (this.m_TargetEnemy != null)
        {
            this.UpdateEnemyGuardPosition();
            this.UpdateEnemyGuardRelativeScale();
        }
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
            this.m_TargetEnemy.position);
        
        float _MinScaleDist = 2 * 2; // full size
        float _MaxScaleDist = 6 * 6f; // scaled size
        float _Distance = (this.m_TargetEnemy.position - this.m_ReferencePlayer.position).sqrMagnitude;
        float _ClampedDistance = Mathf.Clamp(_Distance, _MinScaleDist, _MaxScaleDist);

        float _NormalizedDist = (_ClampedDistance - _MinScaleDist) / (_MaxScaleDist - _MinScaleDist);
        float _Scale = Mathf.Lerp(1.0f, 0.6f, _NormalizedDist);
        
        this.m_View.UpdateEnemyGuardScreenPosition(_ScreenPosition, _Scale);
    }

    private void UpdateEnemyGuardRelativeScale()
    {
        float _MinScaleDist = 2 * 2; // full size
        float _MaxScaleDist = 6 * 6f; // scaled size
        float _Distance = (this.m_TargetEnemy.position - this.m_ReferencePlayer.position).sqrMagnitude;
        float _ClampedDistance = Mathf.Clamp(_Distance, _MinScaleDist, _MaxScaleDist);

        float _NormalizedDist = (_ClampedDistance - _MinScaleDist) / (_MaxScaleDist - _MinScaleDist);
        float _Scale = Mathf.Lerp(1.0f, 0.6f, _NormalizedDist);
        this.m_View.UpdateEnemyGuardScale(_Scale);
    }

    #endregion Methods

}
