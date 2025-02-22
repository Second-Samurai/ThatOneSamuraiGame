using Player.Animation;
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Input;
using UnityEngine;

public interface IDamageable {
    EntityType GetEntityType();
    bool CheckCanDamage();

    void OnEntityDamage(float damage, GameObject attacker, bool unblockable);
    void DisableDamage();
    void EnableDamage();
}

public class PDamageController : MonoBehaviour, IDamageable
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private bool GodMode;
    [SerializeField] private bool _canDamage;

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

    #region - - - - - - Damage Methods - - - - - -

    public void OnEntityDamage(float damage, GameObject attacker, bool unblockable)
    {
        if (!_canDamage) return;

        if (!unblockable)
        {
            this.m_ParryAttackHandler.HandleParryHit(attacker, damage, out bool _IsHitParried);
            this.m_BlockingAttackHandler.HandleBlockHit(out bool _IsHitBlocked);
            
            if (!_IsHitParried && !_IsHitBlocked)
                this.KillPlayer();
        }
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
        => _canDamage = false;

    public void EnableDamage()
    {
        if (GodMode) return;
        _canDamage = true;
    }

    public bool CheckCanDamage() 
        => _canDamage;

    public EntityType GetEntityType() 
        => EntityType.Player;

    #endregion Methods
  
}
