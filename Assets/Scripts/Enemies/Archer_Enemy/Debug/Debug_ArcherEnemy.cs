using System.Collections.Generic;
using System.Linq;
using MBT;
using ThatOneSamuraiGame;
using UnityEngine;

public class Debug_ArcherEnemy : IDebugCommandRegistrater
{

    #region - - - - - - Methods - - - - - -

    public void RegisterCommand(IDebugCommandSystem debugCommandSystem)
    {
        DebugCommand _TriggerArcherTurn = new DebugCommand(
            "archer_triggerturn", 
            "Triggers all archers in view of camera to turn.", 
            "archer_triggerturn", 
            this.TriggerArcherTurn);
        DebugCommand _TriggerDeath = new DebugCommand(
            "archer_triggerdeath", 
            "Triggers all archers in view of camera to die.", 
            "archer_triggerdeath", 
            this.TriggerDeath);
        
        debugCommandSystem.RegisterCommand(_TriggerArcherTurn);
        debugCommandSystem.RegisterCommand(_TriggerDeath);
    }
    
    private void Activate 

    private void TriggerArcherTurn()
    {
        List<ArcherAnimationReciever> _AffectedAnimationRecievers = this.GetAllArchersInCameraView()
            .Select(g => g.GetComponent<ArcherAnimationReciever>())
            .Where(par => par != null)
            .ToList();

        foreach (ArcherAnimationReciever _Receiver in _AffectedAnimationRecievers)
        {
            _Receiver.CompleteTurnClip();
        }
    }

    private void TriggerDeath()
    {
        List<Blackboard> _AffectedArcherBlackboards = this.GetAllArchersInCameraView()
            .Select(g => g.GetComponentInChildren<Blackboard>())
            .Where(par => par != null)
            .ToList();
        
        foreach (Blackboard _BehaviourTree in _AffectedArcherBlackboards)
        {
            BoolVariable _IsDeadReference =
                _BehaviourTree.GetVariable<BoolVariable>(ArcherBehaviourTreeConstants.IsDead);
            _IsDeadReference.Value = true;
        }
    }

    private List<GameObject> GetAllArchersInCameraView()
    {
        List<GameObject> _ObjectsInView = new();

        foreach (GameObject _Object in GameObject.FindGameObjectsWithTag(GameTag.Enemy))
        {
            if (!_Object.activeInHierarchy || _Object.transform == null) continue;

            // Check if inside camera view and in front of the camera
            Vector3 _ViewportPos = Camera.main.WorldToViewportPoint(_Object.transform.position);
            bool _InView =
                _ViewportPos.z > 0 &&
                _ViewportPos.x >= 0 && _ViewportPos.x <= 1 &&
                _ViewportPos.y >= 0 && _ViewportPos.y <= 1;

            if (_InView)
                _ObjectsInView.Add(_Object);
        }

        return _ObjectsInView;
    }

    #endregion Methods
  
}
