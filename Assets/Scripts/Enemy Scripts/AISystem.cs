using System;
using Enemy_Scripts.Enemy_States;
using UnityEngine;

namespace Enemy_Scripts
{
    // AI SYSTEM INFO
    // AISystem is responsible for receiving calls to tell the enemy what to perform. It should also
    // Be responsible for storing enemy data (i.e. Guard meter, remaining guard etc.) BUT
    // any enemy behaviours should be handled through the state machine
    public class AISystem : EnemyStateMachine
    {
        #region Fields and Properties
        
        //TODO: Use a scriptable object for stats instead
        [SerializeField] private float maxEnemyGuard;
        [SerializeField] private float currentGuard;
        
        //TODO: Remove later once more polish is done. These are just placeholders to test enemy states 
        public bool bIsIdle = false;
        public bool bIsLightAttacking = false;
        public bool bIsBlocking = false;
        public Material enemyMaterial;

        #endregion

        #region Unity Standard Functions

        private void Start()
        {
            //Start the enemy in an idle state
            SetState(new IdleEnemyState(this));
        }
        
        //TODO: Remove Update later once more polish is done. These are just placeholders to test enemy states
        private void Update()
        {
            if (bIsIdle)
            {
                bIsIdle = false;
                OnIdle();
            }
            if (bIsLightAttacking)
            {
                bIsLightAttacking = false;
                OnLightAttack();
            }
            if (bIsBlocking)
            {
                bIsBlocking = false;
                OnBlock();
            }
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
