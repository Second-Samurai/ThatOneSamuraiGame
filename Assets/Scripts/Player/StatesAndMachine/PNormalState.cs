using ThatOneSamuraiGame.Scripts.Player.Attack;
using UnityEngine;

//Summary:
/* 
    This class is a child state of Player State and is responsible
    for any related functions or processes during the lifetime of
    the player. This state ONLY exists when player isn't dead or
    in rewind.
*/

public class PNormalState : PlayerState
{
    IEntity playerEntity;

    public override void BeginState()
    {
        playerEntity = this.GetComponent<IEntity>();

        /*PCombatController combatController = this.GetComponent<PCombatController>();
        combatController.Init(playerEntity.GetPlayerStats());
        combatController.UnblockCombatInputs();*/

        PlayerHealthSystem damageController = this.GetComponent<PlayerHealthSystem>();
        damageController.Init(playerEntity.GetPlayerStats());
        damageController.EnableDamage();
    }

    //NOTE: THis is only if we are using colliders with triggers
    private void OnTriggerEnter(Collider other)
    {
        if (true) return;
    }

    public override void EndState()
    {
        // PCombatController combatController = this.GetComponent<PCombatController>();
        // combatController.BlockCombatInputs();

        IPlayerAttackSystem _AttackHandler = this.GetComponent<IPlayerAttackSystem>();
        _AttackHandler.DisableAttack();

        PlayerHealthSystem damageController = this.GetComponent<PlayerHealthSystem>();
        damageController.DisableDamage();
    }
}
