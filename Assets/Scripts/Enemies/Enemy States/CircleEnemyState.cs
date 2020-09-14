using System.Collections;
using UnityEngine;

namespace Enemies.Enemy_States
{
    enum StrafeDirection
    {
        LEFT,
        RIGHT
    }
    
    public class CircleEnemyState : EnemyState
    {
        private Vector3 _target;
        private Vector3 _direction;
        private float _rotationSpeed = 4f;
        private StrafeDirection _strafeDirection = StrafeDirection.LEFT;
        
        //Class constructor
        public CircleEnemyState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override IEnumerator BeginState()
        {
            Debug.Log("Switched to circle state");
            
            // Stop the navMeshAgent from tracking
            AISystem.navMeshAgent.isStopped = true;
            
            ResetAnimationBools();
            
            AISystem.animator.SetBool("IsStrafing", true);
            
            // Perform strafing in the decided direction
            if (_strafeDirection == StrafeDirection.LEFT)
            {
                _direction = Vector3.left.normalized;
                AISystem.animator.SetFloat("StrafeDirectionX", -1.0f);
            }
            else if (_strafeDirection == StrafeDirection.RIGHT)
            {
                _direction = Vector3.right.normalized;
                AISystem.animator.SetFloat("StrafeDirectionX", 1.0f);
            }
            
            yield break;
        }
        
        public override void Tick()
        {
            Transform transform = AISystem.transform;

            // Get the true target point (float offset is added to get a more accurate player-enemy target point)
            _target = AISystem.enemySettings.GetTarget().position + AISystem.floatOffset;
            
            // Set the rotation of the enemy
            Vector3 lookDir = _target - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, _rotationSpeed);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}
