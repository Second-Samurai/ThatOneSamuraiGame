using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Enemy_Scripts.Enemy_States
{
    public class ApproachPlayerEnemyState : EnemyState
    {
        private float _approachDuration = 5.0f;
        
        //Class constructor
        public ApproachPlayerEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            // Placeholder Behaviour, place actions here
            // Debug.Log("is approching");
            // AISystem.enemyMaterial.color = Color.green;

            AISystem.enemyMovementTweener = AISystem.enemyTransform.DOMove(AISystem.targetTransform.transform.position + AISystem.floatOffset, _approachDuration);
            
            yield return new WaitForSeconds(_approachDuration);

            AISystem.enemyMovementTweener = null;
        }
    }
}
