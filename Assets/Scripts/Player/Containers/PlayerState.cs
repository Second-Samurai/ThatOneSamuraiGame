using NUnit.Framework.Constraints;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Containers
{
    
    /// <summary>
    /// Handles the Player's data referred between components.
    /// </summary>
    public class PlayerState : MonoBehaviour, IPlayerState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private GameObject m_AttackTarget;
        private bool m_CanOverrideMovement = false;
        
        [SerializeField]
        private PlayerAttackState m_PlayerAttackState;
        [SerializeField]
        private PlayerMovementState m_PlayerMovementState;
        [SerializeField]
        private PlayerSpecialActionState m_PlayerSpecialActionState;
        [SerializeField]
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;

        #endregion Fields 
        
        #region - - - - - - Properties - - - - - -

        GameObject IPlayerState.AttackTarket 
            => this.m_AttackTarget;

        bool IPlayerState.CanOverrideMovement
        {
            get => this.m_CanOverrideMovement;
            set => this.m_CanOverrideMovement = value;
        }

        PlayerAttackState IPlayerState.PlayerAttackState
        {
            get
            {
                if (this.m_PlayerAttackState != null) 
                    return this.m_PlayerAttackState;
                
                Debug.LogError("PlayerAttackState was not found. Initialised default state is used.");
                this.m_PlayerAttackState = new PlayerAttackState();
                return this.m_PlayerAttackState;
            }
        }

        PlayerMovementState IPlayerState.PlayerMovementState
        {
            get
            {
                if (this.m_PlayerMovementState != null) 
                    return this.m_PlayerMovementState;
                
                Debug.LogError("PlayerMovementState was not found. Initialised default state is used.");
                this.m_PlayerMovementState = new PlayerMovementState();                                  
                return this.m_PlayerMovementState;
            }
        }

        PlayerSpecialActionState IPlayerState.PlayerSpecialActionState
        {
            get
            {
                if (this.m_PlayerSpecialActionState != null)
                    return this.m_PlayerSpecialActionState;
                
                Debug.LogError("PlayerSpecialActionState was not found. Initialised default state is used.");
                this.m_PlayerSpecialActionState = new PlayerSpecialActionState();
                return this.m_PlayerSpecialActionState;
            }
        }

        #endregion Properties

    }
    
}