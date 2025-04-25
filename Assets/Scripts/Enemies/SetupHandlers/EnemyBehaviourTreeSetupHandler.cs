using MBT;
using UnityEngine;

/// <summary>
/// Responsible for common setup handling logic for enemy behaviour trees
/// </summary>
public class EnemyBehaviourTreeSetupHandler : MonoBehaviour, IInitialize
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private Blackboard m_BehaviourTree;

    private BoolVariable m_IsActive;
    private EnemyControlObserver m_EnemyObserver;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -
    void IInitialize.Initialize()
    {
        this.m_EnemyObserver = EnemyManager.Instance.SceneEnemyController.EnemyObserver;

        GameValidator.NotNull(this.m_BehaviourTree, nameof(m_BehaviourTree));
        GameValidator.NotNull(this.m_EnemyObserver, nameof(m_EnemyObserver));

        this.m_IsActive = this.m_BehaviourTree.GetVariable<BoolVariable>(EnemyBehaviourTreeConstants.IsActive);
        this.BindActiveToggleMethods();
    }

    #endregion Initializers
  
    #region - - - - - - Methods - - - - - -

    private void BindActiveToggleMethods()
    {
        this.m_EnemyObserver.OnEnemyStart.AddListener(() => this.m_IsActive.Value = true);
        this.m_EnemyObserver.OnEnemyStop.AddListener(() => this.m_IsActive.Value = false);
    }
    
    #endregion Methods

}
