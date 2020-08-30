using System;
using Enemies.Enemy_States;
using Enemy_Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    // AI SYSTEM INFO
    // AISystem is responsible for receiving calls to tell the enemy what to perform. It should also
    // Be responsible for storing enemy data (i.e. Guard meter, remaining guard etc.) BUT
    // any enemy behaviours should be handled through the state machine
    public class AISystem : EnemyStateMachine
    {
        #region Fields and Properties

        //TODO: Remove later once more polish is done. These are just placeholders to test enemy states 
        public bool bIsIdle = false;
        public bool bIsBlocking = false;

        //ENEMY SETTINGS [See EntityStatData for list of stats]
        public EnemySettings enemySettings; // Taken from EnemySettings Scriptable object in start
        private EnemyTracker _enemyTracker;
        
        //ANIMATOR
        private Animator _animator;
        private bool _bPlayerFound = false;
        
        //NAVMESH
        public NavMeshAgent navMeshAgent;

        //Float offset added to the target location so the enemy doesn't clip into the floor 
        //because the player's origin point is on the floor
        public Vector3 floatOffset = Vector3.up * 2.0f;
        
        #endregion

        #region Unity Monobehaviour Functions

        private void Start()
        {
            // Grab the enemy settings from the Game Manager > Game Settings > Enemy Settings
            enemySettings = GameManager.instance.gameSettings.enemySettings;

            //Start the enemy in an idle state
            SetState(new IdleEnemyState(this));
            
            //Set up animator parameters
            _animator = GetComponent<Animator>();
            _animator.SetFloat("ApproachSpeedMultiplier", enemySettings.enemyData.moveSpeed);
            
            //Set up nav mesh parameters
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
        
        protected new void Update()
        {
            //TODO: Remove these ifs later once more polish is done. These are just placeholders to test enemy states
            if (bIsIdle)
            {
                bIsIdle = false;
                OnIdle();
            }
            if (bIsBlocking)
            {
                bIsBlocking = false;
                OnBlock();
            }

            base.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _enemyTracker == null)
            {
                _enemyTracker = GameManager.instance.enemyTracker;
                _enemyTracker.AddEnemy(this.transform);
                
                OnApproachPlayer();
            }
        }

        #endregion
        
        #region Enemy Utility Funcitons

        public float GetAnimationLength(string animationName)
        {
            AnimationClip animationClip = new AnimationClip();
            bool bFoundClip = false;
            
            foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == animationName)
                {
                    animationClip = clip;
                    bFoundClip = true;
                    break;
                }
            }

            if (bFoundClip)
            {
                return animationClip.length;
            }
            else
            {
                Debug.LogWarning("Animation " + animationName + " could not be found");
                return 0;
            }
        }
        
        public bool GetPlayerFound()
        {
            return _bPlayerFound;
        }

        public void SetPlayerFound(bool playerFound)
        {
            _bPlayerFound = playerFound;
            _animator.SetBool("PlayerFound", playerFound);
        }

        public void SetLightAttacking(bool isLightAttacking)
        {
            _animator.SetBool("IsLightAttacking", isLightAttacking);
        }
        
        public void SetApproaching(bool isApproaching)
        {
            _animator.SetBool("IsApproaching", isApproaching);
        }

        #endregion
        
        // ENEMY STATE SWITCHING INFO
        // Any time an enemy gets a combat maneuver called, their state will switch
        // Upon switching states, they override the EnemyState Start() method to perform their action
        
        #region Enemy Combat Manuervers
        
        public void OnLightAttack()
        {
            SetState(new LightAttackEnemyState(this));
        }

        public void OnHeavyAttack()
        {
        
        }

        public void OnSpecialAttack()
        {
        
        }

        public void OnBlock()
        {
            SetState(new BlockEnemyState(this));
        }

        public void OnParry()
        {
        
        }

        public void OnDodge()
        {
        
        }
    
        #endregion

        #region Enemy Movement

        public void OnIdle()
        {
            SetState(new IdleEnemyState(this));
        }

        public void OnPatrol()
        {
        
        }

        public void OnApproachPlayer()
        {
            SetState(new ApproachPlayerEnemyState(this));
        }

        public void OnCirclePlayer()
        {
        
        }

        public void OnEnemyStun()
        {
            
        }

        public void OnEnemyRecovery()
        {
            
        }

        public void OnEnemyDeath()
        {
            
        }

        #endregion
    }
}
