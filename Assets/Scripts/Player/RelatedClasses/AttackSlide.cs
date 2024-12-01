using System.Collections;
using UnityEngine;

/// <summary>
/// Handling slide manuvers to target enemy during attacks
/// </summary>
public class AttackSlide
{
    private PCombatController _combatController;
    private Rigidbody _playerRB;
    private PlayerSettings _settings;
    private float _targetDistance;

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
        if (CheckWithinMinDist(targetEnemy))
            return;

        _combatController.StopCoroutine(SlideOverTime(targetEnemy));
        _combatController.StartCoroutine(SlideOverTime(targetEnemy));
    }

    /// <summary>
    /// When there are no enemies the player will just slide forward.
    /// </summary>
    public void SlideForward()
    {
        _combatController.StopCoroutine(JustSlideForward());
        _combatController.StartCoroutine(JustSlideForward());
    }

    // Summary: Checks whether the player is within the minimum dist against enemy
    //
    private bool CheckWithinMinDist(Transform targetEnemy)
    {
        _targetDistance = Vector3.Magnitude(_playerRB.transform.position - targetEnemy.position);
        return _targetDistance <= _settings.minimumAttackDist;
    }

    private void FaceTowardsEnemy(Transform targetEnemy)
    {
        Vector3 direction = targetEnemy.position - _playerRB.transform.position;
        float angle = Vector3.Angle(_playerRB.transform.forward, direction);
        angle *= Vector3.Dot(_playerRB.transform.right.normalized, direction.normalized) < 0 ? -1 : 1;

        _playerRB.transform.Rotate(0, angle, 0);
    }

    // Summary: Coroutine for sliding player to forward target
    //
    private IEnumerator SlideOverTime(Transform targetEnemy)
    {
        float velocity = _settings.slideSpeed;

        FaceTowardsEnemy(targetEnemy);

        while (velocity > 0)
        {
            if (CheckWithinMinDist(targetEnemy)) yield break;

            velocity -= _settings.slideSpeed * _settings.slideDuration;
            _playerRB.position += _playerRB.transform.forward * velocity;
            yield return null;
        }
    }

    private IEnumerator JustSlideForward()
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
