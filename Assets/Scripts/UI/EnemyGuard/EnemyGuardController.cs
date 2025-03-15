using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

public class EnemyGuardController : PausableMonoBehaviour, IGuardMeter
{

    #region - - - - - - Fields - - - - - -

    // Required Component Fields
    private EnemyGuardView m_View;
    private Camera m_MainCamera;
    
    // Reference Target Fields
    private Transform m_TargetEnemy;
    private Transform m_ReferencePlayer;

    // Runtime Fields
    private float m_CalculatedScale;

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
        
        this.m_View.UpdateEnemyGuardScreenPosition(_ScreenPosition, this.m_CalculatedScale);
    }

    private void UpdateEnemyGuardRelativeScale()
    {
        const float _MinScaleDist = 2 * 2; // full size
        const float _MaxScaleDist = 6 * 6f; // scaled size
        float _ClampedDistance = Mathf.Clamp(
            (this.m_TargetEnemy.position - this.m_ReferencePlayer.position).sqrMagnitude, 
            _MinScaleDist, 
            _MaxScaleDist);

        float _NormalizedDist = (_ClampedDistance - _MinScaleDist) / (_MaxScaleDist - _MinScaleDist);
        this.m_CalculatedScale = Mathf.Lerp(1.0f, 0.6f, _NormalizedDist);
        this.m_View.UpdateEnemyGuardScale(this.m_CalculatedScale);
    }

    #endregion Methods

}
