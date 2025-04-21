using MBT;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{

    #region - - - - - - Fields - - - - - -

    // Required Dependencies
    [SerializeField, RequiredField] private Blackboard m_BehaviourTree;
    
    // Behaviour Tree Fields
    private BoolVariable m_IsDead;
    
    // Runtime Fields
    [SerializeField] private int m_MaxHitCount;
    private int m_CurrentHitCount;
    private bool m_CanDamage = true;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        GameValidator.NotNull(this.m_BehaviourTree, nameof(m_BehaviourTree));
        
        this.m_IsDead = this.m_BehaviourTree.GetVariable<BoolVariable>(EnemyHealthConstants.IsDead);
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -
    
    public EntityType GetEntityType()
        => EntityType.Enemy;

    public bool CheckCanDamage()
        => this.m_CurrentHitCount < this.m_MaxHitCount;

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (!this.m_CanDamage) return;

        this.m_CurrentHitCount++;
        if (this.m_CurrentHitCount >= this.m_MaxHitCount)
        {
            this.m_IsDead.Value = true;
            this.DisableDamage();
        }
    }

    public void HandleAttack(float damage, GameObject attacker)
    {
    }

    public void DisableDamage() 
        => this.m_CanDamage = false;

    public void EnableDamage() 
        => this.m_CanDamage = true;

    #endregion Methods

}
