using MBT;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

public interface IEnemyWeaponSystem
{

    #region - - - - - - Methods - - - - - -

    void PerformAttack();

    #endregion Methods

}

public class ArcherWeaponSystem : PausableMonoBehaviour, IEnemyWeaponSystem
{

    #region - - - - - - Fields - - - - - -
    
    [Header("Required Dependencies")]
    [SerializeField, RequiredField] private Transform m_AimTarget;
    [SerializeField, RequiredField] private Transform m_FirePoint;
    private ArcherAnimationReciever m_AnimationReceiver;
    
    [Header("Projectile Info")]
    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private float m_DetectionDistance;
    private BoolVariable m_HasDetectedPlayer;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_AnimationReceiver = this.GetComponent<ArcherAnimationReciever>();
        
        GameValidator.NotNull(this.m_AimTarget, nameof(m_AimTarget));
        GameValidator.NotNull(this.m_AnimationReceiver, nameof(m_AnimationReceiver));
        GameValidator.NotNull(this.m_FirePoint, nameof(m_FirePoint));
        GameValidator.NotNull(this.m_ProjectilePrefab, nameof(m_ProjectilePrefab));
        
        this.m_AnimationReceiver.OnBowRelease.AddListener(((IEnemyWeaponSystem)this).PerformAttack);
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    void IEnemyWeaponSystem.PerformAttack() 
        => _ = Instantiate(
            this.m_ProjectilePrefab, 
            this.m_FirePoint.position,
            Quaternion.LookRotation(this.m_AimTarget.position - this.m_FirePoint.position));

    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, this.m_DetectionDistance);
    }

    #endregion Gizmos
  
}
