using System;
using ThatOneSamuraiGame.Scripts.Camera;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.SpecialAction
{
    
    public class PlayerSpecialAction : MonoBehaviour, IPlayerSpecialAction
    {

        #region - - - - - - Fields - - - - - -

        private ICameraController m_CameraController;
        private ICombatController m_CombatController;
        private IPlayerAttackState m_PlayerAttackState;
        private IDamageable m_PlayerDamageHandler;
        private IPlayerMovement m_PlayerMovement;
        
        private Animator m_Animator;
        private bool IsDodging = false;
        private PlayerFunctions m_PlayerFunctions;

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_Animator = this.GetComponent<Animator>();
            this.m_CameraController = this.GetComponent<ICameraController>();
            this.m_CombatController = this.GetComponent<ICombatController>();
            this.m_PlayerAttackState = this.GetComponent<IPlayerAttackState>();
            this.m_PlayerDamageHandler = this.GetComponent<IDamageable>();
            this.m_PlayerFunctions = this.GetComponent<PlayerFunctions>();
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -

        void IPlayerSpecialAction.Dodge()
        {
            
        }

        private void StartDodge()
        {
            
        }

        private void EndDodge()
        {
            
        }

        #endregion Methods
        
    }
    
}
