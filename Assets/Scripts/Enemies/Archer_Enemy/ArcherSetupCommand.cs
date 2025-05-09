using ThatOneSamuraiGame.Scripts.General.Services;
using UnityEngine;

public class ArcherSetupCommand : MonoBehaviour, ICommand
{
    public EnemyBehaviourTreeSetupHandler m_BehaviourTreeSetupHandler;
    
    public void Execute()
    {
        ((IInitialize)this.m_BehaviourTreeSetupHandler).Initialize();
    }
}
