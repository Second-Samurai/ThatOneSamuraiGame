using ThatOneSamuraiGame.Scripts.General.Services;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ArcherSetupCommand : MonoBehaviour, ICommand
{
    public EnemyBehaviourTreeSetupHandler m_BehaviourTreeSetupHandler;
    public AnimationRigControl m_RigControl;
    public RigBuilder m_RigBuilder;
    
    public void Execute()
    {
        if (this.m_RigBuilder.enabled == false)
            this.m_RigControl.BuildRig();
        ((IInitialize)this.m_BehaviourTreeSetupHandler).Initialize();
    }
}
