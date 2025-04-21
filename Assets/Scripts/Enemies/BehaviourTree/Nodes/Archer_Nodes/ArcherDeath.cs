using System.Collections;
using MBT;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Tasks/ArcherDeath")]
public class ArcherDeath : Leaf
{
  
    #region - - - - - - Fields - - - - - -

    [Header("Required Dependencies")]
    [SerializeField, RequiredField] private Animator m_Animator;
    [SerializeField, RequiredField] private EnemyStateObserver m_EventObserver;
    [SerializeField, RequiredField] private AnimationRigControl m_RigControl;
    [SerializeField, RequiredField] private AnimationRagdollController m_RagdollController;
    [SerializeField, RequiredField] private Transform m_ArcherTransform;
    [SerializeField, RequiredField] private DynamicWeaponBody m_WeaponBody;
    private ILockOnObserver m_LockOnObserver;
    
    [Space]
    [SerializeField] private TransformReference m_PlayerTransform = new();
    [SerializeField] private float m_TimeTillDestroy = 1f;
    
    private bool m_HasPlayedAnimation;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_LockOnObserver = SceneManager.Instance.LockOnObserver;
        
        GameValidator.NotNull(this.m_Animator, nameof(m_Animator));
        GameValidator.NotNull(this.m_ArcherTransform, nameof(m_ArcherTransform));
        GameValidator.NotNull(this.m_EventObserver, nameof(m_EventObserver));
        GameValidator.NotNull(this.m_LockOnObserver, nameof(m_LockOnObserver));
        GameValidator.NotNull(this.m_RagdollController, nameof(m_RagdollController));
        GameValidator.NotNull(this.m_PlayerTransform, nameof(m_PlayerTransform));
        GameValidator.NotNull(this.m_WeaponBody, nameof(m_WeaponBody));
    }

    #endregion Unity Methods
  
    #region - - - - - - MBT Methods - - - - - -

    public override NodeResult Execute()
    {
        if (this.m_HasPlayedAnimation) return NodeResult.success;

        // Turn the character model into ragdoll
        Vector3 _Direction = this.m_ArcherTransform.position - this.m_PlayerTransform.Value.position;
        this.m_RigControl.DeActivateAllLayers();
        this.m_RagdollController.SwitchToRagdoll();
        this.m_RagdollController.ApplyForce(_Direction);
        
        // Make the weapon model subject to physics
        this.m_WeaponBody.ChangeToDynamic();
        this.m_WeaponBody.ApplyForce(_Direction, 8); // Arbitrary Force

        this.m_EventObserver.OnEnemyDeath.Invoke();
        this.StartCoroutine(this.KillArcher());
        this.m_HasPlayedAnimation = true;
        
        return NodeResult.success;
    }

    #endregion MBT Methods
  
    #region - - - - - - Methods - - - - - -

    private IEnumerator KillArcher()
    {
        yield return new WaitForSeconds(this.m_TimeTillDestroy);
        
        GameObject _Archer = this.transform.root.gameObject;
        this.m_LockOnObserver.OnRemoveLockOnTarget.Invoke(_Archer.transform);
        // Destroy(_Archer);
    }

    #endregion Methods
  
}
