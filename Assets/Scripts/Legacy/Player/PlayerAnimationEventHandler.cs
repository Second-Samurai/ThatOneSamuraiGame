﻿using System;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Player.Attack;
using ThatOneSamuraiGame.Scripts.Player.Containers;
using ThatOneSamuraiGame.Scripts.Player.Movement;
using ThatOneSamuraiGame.Scripts.Player.SpecialAction;
using Unity.Collections;

namespace ThatOneSamuraiGame.Scripts.Player.Animation
{
    
    /// <summary>
    /// Responsible for handling animation events for the player.
    /// </summary>
    [Obsolete]
    public class PlayerAnimationEventHandler : PausableMonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        private IDamageable m_PlayerDamage;
        private IPlayerAttackSystem m_PlayerAttackHandler;
        // private PlayerFunctions m_PlayerFunctions;
        private BlockingAttackHandler m_BlockingAttackHandler;
        private IPlayerMovement m_PlayerMovement;
        
        // Player states
        private PlayerAttackState m_PlayerAttackState;
        private PlayerMovementDataContainer _mPlayerMovementDataContainer;
        private PlayerSpecialActionState m_PlayerSpecialActionState;

        #endregion Fields
        
        #region - - - - - - Lifecycle Methods - - - - - -

        private void Start()
        {
            this.m_PlayerAttackHandler = this.GetComponent<IPlayerAttackSystem>();
            this.m_PlayerDamage = this.GetComponent<IDamageable>();
            // this.m_PlayerFunctions = this.GetComponent<PlayerFunctions>();
            this.m_BlockingAttackHandler = this.GetComponent<BlockingAttackHandler>();
            this.m_PlayerMovement = this.GetComponent<IPlayerMovement>();

            IPlayerState _PlayerState = this.GetComponent<IPlayerState>();
            this.m_PlayerAttackState = _PlayerState.PlayerAttackState;
            this._mPlayerMovementDataContainer = _PlayerState.PlayerMovementDataContainer;
            this.m_PlayerSpecialActionState = _PlayerState.PlayerSpecialActionState;
        }

        #endregion Lifecycle Methods

        #region - - - - - - PlayerAttack Animation Events - - - - - -

        // public void DisableRotation()
        //     => this.m_PlayerMovement.DisableRotation();
        //
        // public void EnableRotation()
        //     => this.m_PlayerMovement.EnableRotation();
        
        // public void EndParryStunState()
        //     => this.m_PlayerAttackState.ParryStunned = false;
        //
        // public void StartParryStunState()
        //     => this.m_PlayerAttackState.ParryStunned = true;

        #endregion PlayerAttack Animation Events
        
        #region - - - - - - PlayerMovement Animation Events  - - - - - -
        
        // ======== EVENT CALLED ========
        
        // 1stAttackEdit - 00:00
        // public void DisableMovement() 
        //     => this.m_PlayerMovement.DisableMovement();
        
        // public void EnableMovement() 
        //     => this.m_PlayerMovement.EnableMovement();

        // public void LockMoveInput() // This is not being used anywhere
        // { 
        //     if (this._mPlayerMovementDataContainer.IsMovementLocked)
        //         return;
        //
        //     this._mPlayerMovementDataContainer.IsMovementLocked = true;
        //     this.StartDodging(); // Note: I find it unusual that Dodging is invoked when not moving the character.
        // }
        //
        // public void UnlockMoveInput()
        // {
        //     if (!this._mPlayerMovementDataContainer.IsMovementLocked)
        //         return;
        //
        //     this._mPlayerMovementDataContainer.IsMovementLocked = false;
        //     this.EndDodging();
        // }

        #endregion PlayerMovement Animation Events

        #region - - - - - - PlayerSpecialAction Animation Events - - - - - -
        
        // 1stAttackEdit - 00:02
        // public void BlockDodge() 
        //     => this.m_PlayerSpecialActionState.CanDodge = false;

        // TODO: Remove resets from happening exclusively from the AnimationEventHandler.
        // 1stRecoveryEdit - 00:00
        // public void ResetDodge()
        // {
        //     this.m_PlayerSpecialActionState.CanDodge = true;
        //     this.m_PlayerSpecialActionState.IsDodging = false;
        // }
        //
        // public void StartDodging()
        // {
        //     this.m_PlayerAttackState.CanAttack = false;
        //     this.m_PlayerSpecialActionState.IsDodging = true;
        //     this.m_PlayerAttackState.IsHeavyAttackCharging = false;
        //     this.m_PlayerAttackState.IsWeaponSheathed = false;
        //     
        //     this.m_PlayerDamage.DisableDamage();
        //     this.m_BlockingAttackHandler.DisableBlock();
        //     this.m_PlayerAttackHandler.ResetAttack();
        // }
        //
        // public void EndDodging()
        // {
        //     this.m_PlayerAttackState.CanAttack = true;
        //     // this.m_PlayerSpecialActionState.IsDodging = false;
        //     
        //     this.m_PlayerDamage.EnableDamage();
        //     this.m_BlockingAttackHandler.EnableBlock();
        //     this.m_PlayerAttackHandler.ResetAttack();
        // }

        #endregion PlayerSpecialAction Animation Events

    }
    
}