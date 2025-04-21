using System;
using Player.Animation;
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Input;
using UnityEngine;

public interface IDamageable {

    #region - - - - - - Methods - - - - - -

    EntityType GetEntityType();
    
    bool CheckCanDamage(); // Redundant

    void OnEntityDamage(float damage, GameObject attacker, bool unblockable); // TODO: Make redundant

    void HandleAttack(float damage, GameObject attacker);
    
    void DisableDamage();
    
    void EnableDamage();

    #endregion Methods
  
}

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private bool GodMode;
    [SerializeField] private bool m_CanDamage;

    private StatHandler playerStats;
    private BlockingAttackHandler m_BlockingAttackHandler;
    private ParryAttackHandler m_ParryAttackHandler;
    private PlayerAnimationComponent m_PlayerAnimationComponent;
    private ILockOnSystem m_LockOnSystem;

    #endregion Fields

    #region - - - - - - Initializers - - - - - -

    public void Init(StatHandler playerStats)
    {
        this.m_PlayerAnimationComponent = this.GetComponent<PlayerAnimationComponent>();
        this.m_LockOnSystem = this.GetComponent<ILockOnSystem>();
        this.m_BlockingAttackHandler = this.GetComponent<BlockingAttackHandler>();
        this.m_ParryAttackHandler = this.GetComponent<ParryAttackHandler>();
        this.playerStats = playerStats;
    }

    #endregion Initializers

    #region - - - - - - Unity Methods - - - - - -

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameTag.Player)
        {
            // TODO: Flesh out more behaviour to affect on player
            Debug.Log("Player has been damaged");
        }
    }

    #endregion Unity Methods
  
    #region - - - - - - Damage Methods - - - - - -

    // ---------------------------------------------------
    // This implementation feels inappropriate. It is not based on any event but rather direct method calls from
    // associated classes.
    // ---------------------------------------------------
    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        // if (!m_CanDamage) return;
        //
        // if (!unblockable)
        // {
        //     this.m_ParryAttackHandler.HandleParryHit(attacker, damage, out bool _IsHitParried);
        //     this.m_BlockingAttackHandler.HandleBlockHit(out bool _IsHitBlocked);
        //     
        //     if (!_IsHitParried && !_IsHitBlocked)
        //         this.KillPlayer();
        // }
        // else
        //     this.KillPlayer();
    }

    public void HandleAttack(float damage, GameObject attacker)
    {
        if (!this.m_CanDamage) return;

        if (this.m_ParryAttackHandler.IsParrying)
            this.m_ParryAttackHandler.PerformParry(attacker);
        else if (this.m_BlockingAttackHandler.IsBlocking)
            this.m_BlockingAttackHandler.HandleBlockHit(out bool _);
        else
            this.KillPlayer();
    }

    #endregion Damage Methods

    #region - - - - - - Methods - - - - - -
    
    public void KillPlayer()
    {
        // play anim
        this.m_PlayerAnimationComponent.SetDead(true);
        
        // Activate input switch to rewind
        IInputManager _InputManager = GameManager.instance.InputManager;
        _InputManager.SwitchToRewindControls();
        this.m_LockOnSystem.EndLockOn();
    }

    /* Summary: This disables the damage from this component.
     *          But can be only used when in a state that does
     *          not require it.*/
    public void DisableDamage() 
        => m_CanDamage = false;

    public void EnableDamage()
    {
        if (GodMode) return;
        m_CanDamage = true;
    }

    public bool CheckCanDamage() 
        => m_CanDamage;

    public EntityType GetEntityType() 
        => EntityType.Player;

    #endregion Methods
  
}
