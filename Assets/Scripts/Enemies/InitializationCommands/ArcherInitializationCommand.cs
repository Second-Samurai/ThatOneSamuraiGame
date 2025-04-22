using MBT;
using UnityEngine;

public class ArcherInitializationCommand : MonoBehaviour, IInitialize
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private Blackboard m_BehaviourTree;

    private BoolVariable m_IsDead;
    private EnemyControlObserver m_EnemyObserver;

    #endregion Fields
  
    #region - - - - - - Initializers - - - - - -

    // Should be only called during Start()
    public void Initialize()
    {
        this.m_EnemyObserver = EnemyManager.Instance.EnemyObserver;

        GameValidator.NotNull(this.m_BehaviourTree, nameof(m_BehaviourTree));
        GameValidator.NotNull(this.m_EnemyObserver, nameof(m_EnemyObserver));

        this.m_IsDead = this.m_BehaviourTree.GetVariable<BoolVariable>(ArcherBehaviourTreeConstants.IsDead);
        
        this.BindActiveToggleMethods();
    }

    #endregion Initializers

    #region - - - - - - Methods - - - - - -

    private void BindActiveToggleMethods()
    {
        this.m_EnemyObserver.OnEnemyStart.AddListener(() => this.m_IsDead.Value = true);
        this.m_EnemyObserver.OnEnemyStop.AddListener(() => this.m_IsDead.Value = false);
    }

    #endregion Methods
  
}
