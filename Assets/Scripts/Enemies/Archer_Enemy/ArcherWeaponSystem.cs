using MBT;
using ThatOneSamuraiGame.Scripts.Base;
using UnityEngine;

public interface IEnemyWeaponSystem
{

    #region - - - - - - Methods - - - - - -

    void HasSensedPlayer();

    void PerformAttack();

    #endregion Methods

}

public class ArcherWeaponSystem : PausableMonoBehaviour, IEnemyWeaponSystem
{

    #region - - - - - - Fields - - - - - -
    
    [Header("Required Dependencies")]
    [SerializeField, RequiredField] private Transform m_AimTarget;
    [SerializeField, RequiredField] private Transform m_FirePoint;
    [SerializeField, RequiredField] private Blackboard m_Blackboard;
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
        GameValidator.NotNull(this.m_FirePoint, nameof(m_FirePoint));
        GameValidator.NotNull(this.m_AnimationReceiver, nameof(m_AnimationReceiver));
        GameValidator.NotNull(this.m_Blackboard, nameof(m_Blackboard));
        GameValidator.NotNull(this.m_ProjectilePrefab, nameof(m_ProjectilePrefab));
        
        this.BindActionsToBehaviourTree();
        
        this.m_AnimationReceiver.OnBowRelease.AddListener(((IEnemyWeaponSystem)this).PerformAttack);
    }

    private void Update()
    {
        if (this.IsPaused) return;
        ((IEnemyWeaponSystem)this).HasSensedPlayer();
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    void IEnemyWeaponSystem.HasSensedPlayer()
    {
        // The player's position is always known. All relevant systems 'react' to the player's position
        float _Distance = (this.m_AimTarget.position - this.transform.position).sqrMagnitude;
        if (_Distance < this.m_DetectionDistance * this.m_DetectionDistance)
        {
            this.m_HasDetectedPlayer.Value = true;
            return;
        }

        this.m_HasDetectedPlayer.Value = false;
    }

    void IEnemyWeaponSystem.PerformAttack() 
        => _ = Instantiate(
            this.m_ProjectilePrefab, 
            this.m_FirePoint.position,
            Quaternion.LookRotation(this.m_AimTarget.position - this.m_FirePoint.position));

    private void BindActionsToBehaviourTree() 
        => this.m_HasDetectedPlayer = this.m_Blackboard.GetVariable<BoolVariable>(
            ArcherBehaviourTreeConstants.HasDetectedPlayer);

    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, this.m_DetectionDistance);
    }

    #endregion Gizmos
  
}
