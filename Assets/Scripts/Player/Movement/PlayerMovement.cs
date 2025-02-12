using System;
using System.Runtime.CompilerServices;
using Player.Animation;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.TargetTracking;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player.Movement
{

    public class PlayerMovementInitializerData
    {

        #region - - - - - - Properties - - - - - -

        public ICameraController CameraController { get; private set; }
        
        public ILockOnObserver LockOnObserver { get; private set; }

        #endregion Properties

        #region - - - - - - Constructors - - - - - -

        public PlayerMovementInitializerData(ICameraController cameraController, ILockOnObserver lockOnObserver)
        {
            this.CameraController = cameraController;
            this.LockOnObserver = lockOnObserver;
        }

        #endregion Constructors
  
    }
    
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : 
        PausableMonoBehaviour, 
        IInitialize<PlayerMovementInitializerData>,
        IPlayerMovement, 
        IPlayerDodgeMovement
    {
        
        #region - - - - - - Fields - - - - - -
        
        [RequiredField]
        [SerializeField]
        private PlayerAnimationComponent m_PlayerAnimationComponent;
        private Rigidbody m_Rigidbody;
        
        private ICameraController m_CameraController;
        
        // Player data containers
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementDataContainer _mPlayerMovementDataContainer;
        private PlayerTargetTrackingState m_PlayerTargetTrackingState;
        
        // Player states
        private IPlayerMovementState m_CurrentMovementState;
        private IPlayerMovementState m_NormalMovement;
        private IPlayerMovementState m_LockOnMovement;
        private IPlayerMovementState m_FinisherMovement;
        
        private bool m_IsMovementEnabled = true;
        private bool m_IsRotationEnabled = true;
        private bool m_IsSprinting = false;
        private float m_SprintDuration = 0.0f;
        private float m_RotationSpeed = 4f;
        
        #endregion Fields
        
        #region - - - - - - Initializers - - - - - -

        public void Initialize(PlayerMovementInitializerData initializerData)
        {
            this.m_CameraController = 
                initializerData.CameraController 
                ?? throw new ArgumentNullException(nameof(initializerData.CameraController));
            this.m_Rigidbody = this.GetComponent<Rigidbody>() ??
                               throw new ArgumentNullException(nameof(this.m_Rigidbody));
            
            // Bind to observers
            initializerData.LockOnObserver.OnLockOnDisable.AddListener(() => 
                ((IPlayerMovement)this).SetState(PlayerMovementStates.Normal));
        }

        #endregion Initializers
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerAnimationComponent = this.GetComponent<PlayerAnimationComponent>();
            this.m_PlayerTargetTrackingState = this.GetComponent<IPlayerState>().PlayerTargetTrackingState;

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this._mPlayerMovementDataContainer = _PlayerState.PlayerMovementDataContainer;

            // Initialize Movement States
            this.m_NormalMovement = new PlayerNormalMovement(
                this.GetComponent<IPlayerAttackHandler>(),
                this.m_PlayerAttackState,
                this.m_CameraController,
                this._mPlayerMovementDataContainer,
                this.m_PlayerAnimationComponent, 
                this.transform,
                this);
            this.m_LockOnMovement = new PlayerLockOnMovement(
                this.GetComponent<IPlayerAttackHandler>(),
                this.m_PlayerAttackState,
                this.m_PlayerAnimationComponent, 
                this.transform,
                this._mPlayerMovementDataContainer,
                this.m_PlayerTargetTrackingState,
                this);
            this.m_FinisherMovement = new PlayerFinishMovement(
                this._mPlayerMovementDataContainer,
                this.m_PlayerAnimationComponent,
                this.transform);
            
            this.m_CurrentMovementState = this.m_NormalMovement;
            
            ((IPlayerDodgeMovement)this).EnableDodge();
        }

        private void Update()
        {
            if (this.IsPaused || !this.m_IsMovementEnabled)
                return;

            this.m_CurrentMovementState.CalculateMovement();
            this.m_CurrentMovementState.ApplyMovement();
            
            if (IsSprinting())
                TickSprintDuration();
        }

        #endregion Lifecycle Methods
        
        #region - - - - - - Methods - - - - - -
        
        // --------------------------------
        // Dodge
        // --------------------------------
        
        public void Dodge()
            => this.m_CurrentMovementState.PerformDodge();

        void IPlayerDodgeMovement.EnableDodge()
            => this._mPlayerMovementDataContainer.CanDodge = true;

        void IPlayerDodgeMovement.DisableDodge()
            => this._mPlayerMovementDataContainer.CanDodge = false;
        
        // --------------------------------
        // Movement
        // --------------------------------

        public bool IsSprinting() => m_IsSprinting;

        public float GetSprintDuration() => m_SprintDuration;
        
        public void CancelMove()
        {
            StopAllCoroutines(); 
            ((IPlayerMovement)this).EnableMovement();
            ((IPlayerMovement)this).EnableRotation();
            this.m_Rigidbody.linearVelocity = Vector3.zero;
            this.m_PlayerAnimationComponent.SetRootMotion(true);
        }

        void IPlayerMovement.DisableMovement()
            => this.m_IsMovementEnabled = false;

        void IPlayerMovement.EnableMovement()
            => this.m_IsMovementEnabled = true;

        void IPlayerMovement.PreparePlayerMovement(Vector2 moveDirection)
        {
            if (moveDirection == Vector2.zero)
            {
                this.m_CurrentMovementState.PerformSprint(false);
            }
            
            this.m_CurrentMovementState.SetInputDirection(moveDirection);
        }

        void IPlayerMovement.PrepareSprint(bool isSprinting)
        {
            this.m_CurrentMovementState.PerformSprint(isSprinting);
            this.m_CameraController.SelectCamera(isSprinting 
                ? SceneCameras.FollowSprintPlayer 
                : SceneCameras.FollowPlayer);

            this.m_IsSprinting = isSprinting;

            // // Toggle sprinting animation
            // if (this.m_InputDirection == Vector2.zero || !this.m_IsSprinting)
            // {
            //     m_SprintDuration = 0.0f;
            //     m_PlayerAnimationComponent.SetSprinting(false);
            // }
            // else
            // {
            //     m_SprintDuration = 0.0f;
            //     m_PlayerAnimationComponent.SetSprinting(true);
            // }
        }
        
        private void TickSprintDuration()
        {
            m_SprintDuration += Time.deltaTime;
        }
        
        // --------------------------------
        // Rotation
        // --------------------------------

        void IPlayerMovement.DisableRotation()
            => this.m_IsRotationEnabled = false;

        void IPlayerMovement.EnableRotation()
            => this.m_IsRotationEnabled = true;
        
        // --------------------------------
        // State Management
        // --------------------------------
        
        void IPlayerMovement.SetState(PlayerMovementStates state)
        {
            // A check is performed as the swordsman implementation might cause a stackoverflow,
            //  by running this operation multiple times upon its death.
            if (this.m_CurrentMovementState != null && this.m_CurrentMovementState.GetState() == state) return;
            
            if (state == PlayerMovementStates.Normal)
                this.m_CurrentMovementState = this.m_NormalMovement;
            else if (state == PlayerMovementStates.LockOn)
                this.m_CurrentMovementState = this.m_LockOnMovement;
            else if (state == PlayerMovementStates.Finisher)
                this.m_CurrentMovementState = this.m_FinisherMovement;
            
            Debug.Log("Movement State is: " + state);
        }

        #endregion Methods
        
    }
    
}
