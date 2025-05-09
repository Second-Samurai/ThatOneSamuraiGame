using UnityEngine;

public class WeaponPhysicsController : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private Rigidbody m_WeaponRigidbody;
    [SerializeField, RequiredField] private Collider m_WeaponCollider;
    
    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_WeaponCollider = this.GetComponent<Collider>();
        this.m_WeaponRigidbody = this.GetComponent<Rigidbody>();
        
        GameValidator.NotNull(this.m_WeaponCollider, nameof(m_WeaponCollider));
        GameValidator.NotNull(this.m_WeaponRigidbody, nameof(m_WeaponRigidbody));
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void ApplyForce(Vector3 direction, float force) 
        => this.m_WeaponRigidbody.AddForce(direction.normalized * force, ForceMode.Impulse);

    public void ChangeToKinematic()
    {
        this.m_WeaponRigidbody.isKinematic = true;
        this.m_WeaponRigidbody.useGravity = false;
        this.m_WeaponCollider.enabled = false;
    }

    public void ChangeToDynamic()
    {
        this.m_WeaponRigidbody.isKinematic = false;
        this.m_WeaponRigidbody.useGravity = true;
        this.m_WeaponCollider.enabled = true;
    }

    #endregion Methods
  
}
