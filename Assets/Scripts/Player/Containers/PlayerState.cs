using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;
using UnityEngine.Serialization;

namespace ThatOneSamuraiGame.Scripts.Player.Containers
{
    
    /// <summary>
    /// Handles the Player's data referred between components.
    /// </summary>
    public class PlayerState : MonoBehaviour, IPlayerState
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private PlayerAttackState m_PlayerAttackState;
        [FormerlySerializedAs("m_PlayerMovementState")]
        [SerializeField]
        private PlayerMovementDataContainer mPlayerMovementDataContainer;
        [SerializeField]
        private PlayerSpecialActionState m_PlayerSpecialActionState;
        [SerializeField] 
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;

        #endregion Fields

        #region - - - - - - Lifecycle Methods - - - - - -

        private void Awake()
        {
            // Initialise state if not already initialised
            this.m_PlayerAttackState ??= new PlayerAttackState();
            this.mPlayerMovementDataContainer ??= new PlayerMovementDataContainer();
            this.m_PlayerSpecialActionState ??= new PlayerSpecialActionState();
            this.m_PlayerTargetTrackingState ??= new PlayerTargetTrackingState();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Properties - - - - - -

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

        PlayerMovementDataContainer IPlayerState.PlayerMovementDataContainer
        {
            get
            {
                if (this.mPlayerMovementDataContainer != null) 
                    return this.mPlayerMovementDataContainer;
                
                Debug.LogError("PlayerMovementState was not found. Initialised default state is used.");
                this.mPlayerMovementDataContainer = new PlayerMovementDataContainer();                                  
                return this.mPlayerMovementDataContainer;
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

        PlayerTargetTrackingState IPlayerState.PlayerTargetTrackingState
        {
            get
            {
                if (this.m_PlayerTargetTrackingState != null)
                    return this.m_PlayerTargetTrackingState;
                
                Debug.LogError("PlayerTargetTrackingState was not found. Initialised default state is used.");
                this.m_PlayerTargetTrackingState = new PlayerTargetTrackingState();
                return this.m_PlayerTargetTrackingState;
            }
        }

        #endregion Properties

    }
    
}
