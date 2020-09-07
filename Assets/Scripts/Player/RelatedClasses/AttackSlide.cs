using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//---IMPROVEMENTS--- /////////////////////////////////////
//TODO: During slide instead of just forward, it slides towards the enemy.

/// <summary>
/// Handling slide manuvers to target enemy during attacks
/// </summary>
public class AttackSlide
{
    private PCombatController _combatController;
    private Rigidbody _playerRB;
    private PlayerSettings _settings;

    /// <summary>
    /// Attack slide Init function
    /// </summary>
    public void Init(PCombatController controller, Rigidbody playerRB)
    {
        _combatController = controller;
        _playerRB = playerRB;
        _settings = GameManager.instance.gameSettings.playerSettings;
    }

    // Summary: Trigger sliding towards intended enemies
    //
    public void SlideToEnemy(Transform targetEnemy)
    {
        if (CheckWithinMinDist(targetEnemy)) return;

        _combatController.StopCoroutine(SlideOverTime());
        _combatController.StartCoroutine(SlideOverTime());
    }

    // Summary: Checks whether the player is within the minimum dist against enemy
    //
    private bool CheckWithinMinDist(Transform targetEnemy)
    {
        float distance = Vector3.Distance(_playerRB.transform.position, targetEnemy.position);

        return distance <= _settings.minimumAttackDist;
    }

    // Summary: Coroutine for sliding player to forward target
    //
    private IEnumerator SlideOverTime()
    {
        float velocity = _settings.slideSpeed;

        while (velocity > 0)
        {
            velocity -= _settings.slideSpeed * _settings.slideDuration;
            _playerRB.position += _playerRB.transform.forward * velocity;
            yield return null;
        }
    }
}
