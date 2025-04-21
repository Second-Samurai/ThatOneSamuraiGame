using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

// TODO: Change actions to be interface based
public class AnimationRagdollController : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private GameObject m_TargetCharacterModel;
    
    [Header("Rigidbody & Collider Fields")]
    [SerializeField] private RigidbodyInterpolation m_Interpolation;
    [SerializeField] private CollisionDetectionMode m_CollisionDetection;
    [SerializeField] private PhysicsMaterial m_CollisionMaterial;
    [SerializeField] private LayerMask m_ExcludedCollisionLayersOnKinematic;
    [SerializeField] private LayerMask m_ExcludedCollisionLayersOnRagdoll;

    [Header("Force Characteristics")]
    [SerializeField] private Rigidbody m_SpineRigidbody;
    [SerializeField] private float m_RagdollForce;
    
    // Required Dependencies
    private Animator m_Animator;
    private NavMeshAgent m_NavMeshAgent;
    private Rigidbody m_RootRigidbody;
    private Collider m_RootCollider;
    
    // Ragdoll components from all bones
    private List<Rigidbody> m_RagdollRigidbodies;
    private List<Collider> m_RagdollColliders;
    
    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_Animator = this.GetComponent<Animator>();
        this.m_NavMeshAgent = this.GetComponent<NavMeshAgent>();
        this.m_RootCollider = this.GetComponent<Collider>();
        this.m_RootRigidbody = this.GetComponent<Rigidbody>();

        GameValidator.NotNull(this.m_Animator, nameof(m_Animator));
        GameValidator.NotNull(this.m_NavMeshAgent, nameof(m_NavMeshAgent));
        GameValidator.NotNull(this.m_RootCollider, nameof(m_RootCollider));
        GameValidator.NotNull(this.m_RootRigidbody, nameof(m_RootRigidbody));
        
        this.m_RagdollRigidbodies = this.m_TargetCharacterModel.GetComponentsInChildren<Rigidbody>().ToList();
        this.m_RagdollColliders = this.m_TargetCharacterModel.GetComponentsInChildren<Collider>().ToList();

        this.SetupRagdoll();
        this.SwitchToKinematic();
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void ApplyForce(Vector3 direction)
    {
        if (!this.m_SpineRigidbody) return;
        
        this.m_SpineRigidbody.AddForce(direction.normalized * this.m_RagdollForce, ForceMode.Impulse);
    }
    
    public void SwitchToKinematic()
    {
        for (int i = 0; i < this.m_RagdollRigidbodies.Count; i++)
            this.m_RagdollRigidbodies[i].isKinematic = true;
        
        for (int i = 0; i < this.m_RagdollColliders.Count; i++)
        {
            Collider _Collider = this.m_RagdollColliders[i];
            _Collider.excludeLayers = this.m_ExcludedCollisionLayersOnKinematic;
            _Collider.isTrigger = true;
        }
    }

    public void SwitchToRagdoll()
    {
        this.m_NavMeshAgent.enabled = false;
        this.m_Animator.enabled = false;
        this.m_RootRigidbody.isKinematic = true;
        this.m_RootCollider.enabled = false;

        for (int i = 0; i < this.m_RagdollRigidbodies.Count; i++)
        {
            Rigidbody _Rigidbody = this.m_RagdollRigidbodies[i];
            _Rigidbody.isKinematic = false;
        }

        for (int i = 0; i < this.m_RagdollColliders.Count; i++)
        {
            Collider _Collider = this.m_RagdollColliders[i];
            _Collider.excludeLayers = this.m_ExcludedCollisionLayersOnRagdoll;
            _Collider.isTrigger = false;
        }
    }

    private void SetupRagdoll()
    {
        for (int i = 0; i < this.m_RagdollRigidbodies.Count; i++)
        {
            Rigidbody _Rigidbody = this.m_RagdollRigidbodies[i];
            _Rigidbody.interpolation = this.m_Interpolation;
            _Rigidbody.collisionDetectionMode = this.m_CollisionDetection;
            
            // Clear out all forces
            _Rigidbody.linearVelocity = Vector3.zero;
            _Rigidbody.angularVelocity = Vector3.zero;
        }

        for (int i = 0; i < this.m_RagdollColliders.Count; i++)
        {
            Collider _Collider = this.m_RagdollColliders[i];
            _Collider.material = this.m_CollisionMaterial;
        }
    }

    #endregion Methods
  
}
