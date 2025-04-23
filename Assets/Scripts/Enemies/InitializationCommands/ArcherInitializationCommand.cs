using MBT;
using ThatOneSamuraiGame.Scripts.General.Services;
using UnityEngine;

public class ArcherInitializationCommand : MonoBehaviour, ICommand
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private Blackboard m_BehaviourTree;

    private BoolVariable m_IsActive;
    private EnemyControlObserver m_EnemyObserver;

    #endregion Fields
  
    #region - - - - - - Methods - - - - - -
    
    public void Execute()
    {
        this.m_EnemyObserver = EnemyManager.Instance.EnemyObserver;

        GameValidator.NotNull(this.m_BehaviourTree, nameof(m_BehaviourTree));
        GameValidator.NotNull(this.m_EnemyObserver, nameof(m_EnemyObserver));

        this.m_IsActive = this.m_BehaviourTree.GetVariable<BoolVariable>(EnemyBehaviourTreeConstants.IsActive);
        this.BindActiveToggleMethods();
    }

    private void BindActiveToggleMethods()
    {
        this.m_EnemyObserver.OnEnemyStart.AddListener(() => this.m_IsActive.Value = true);
        this.m_EnemyObserver.OnEnemyStop.AddListener(() => this.m_IsActive.Value = false);
    }

    #endregion Methods

}
