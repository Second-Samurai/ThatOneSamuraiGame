using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Identification and filtering of nearest enemies
/// </summary>
public class CloseEnemyGuideControl
{
    private Transform _attachedPlayer;
    private Transform _lastEnemy = null;
    private AttackSlide _slideController;

    private PlayerSettings _settings;

    public void Init(PCombatController playerCombat, Transform playerTransform, Rigidbody rigidbody)
    {
        this._attachedPlayer = playerTransform;
        _settings = GameManager.instance.gameSettings.playerSettings;

        _slideController = new AttackSlide();
        _slideController.Init(playerCombat, rigidbody);
    }

    /// <summary>
    /// This finds the nearest forward object and steps closer in direction.
    /// </summary>
    public void MoveToNearestEnemy()
    {
        if (CheckLastAttackedEnemy()) return;

        GameObject result = FindClosestEnemy();

        if (result != null)
        {
            _lastEnemy = result.transform;
            _slideController.SlideToEnemy(_lastEnemy);
        }
    }

    // Summary: Checks whether last registered attacked enemy is valid to attack
    //
    private bool CheckLastAttackedEnemy()
    {
        if (_lastEnemy != null)
        {
            Vector3 direction = (_lastEnemy.position - _attachedPlayer.position).normalized;
            if (CheckIfForward(direction) && CheckMinDistance(_attachedPlayer.position, _lastEnemy.position, _settings.minimumAttackDist))
            {
                _slideController.SlideToEnemy(_lastEnemy);
                return true;
            }
        }

        return false;
    }

    // Summary: Gets the closest enemy from collection of nearby enemies.
    // 
    private GameObject FindClosestEnemy()
    {
        List<GameObject> enemyCollection = new List<GameObject>();
        enemyCollection = GetNearbyObjects();

        //Checks for edge cases
        if (enemyCollection.Count == 0) return null;
        if (enemyCollection.Count == 1)
        {
            Vector3 direction = (enemyCollection[0].transform.position - _attachedPlayer.position).normalized;
            if (CheckIfForward(direction)) return enemyCollection[0];

            return null;
        }

        //Performs filtering through collection
        enemyCollection = FilterForward(enemyCollection);
        if (enemyCollection.Count == 0) return null;

        GameObject closest = GetClosestEnemy(enemyCollection);
        if (closest == null) return null;

        return closest;
    }

    // Summary: Gets the nearby objects around the player
    //
    private List<GameObject> GetNearbyObjects()
    {
        List<Collider> hitColliders = Physics.OverlapSphere(_attachedPlayer.position, _settings.detectionRadius).Where(x => x.GetComponent<IDamageable>() != null).ToList();
        List<GameObject> nearbyEnemies = new List<GameObject>();
        IDamageable entities;

        //Filters through collecting the enemy entities
        for (int i = 0; i < hitColliders.Count; i++)
        {
            entities = hitColliders[i].GetComponent<IDamageable>();

            if(entities.GetEntityType() == EntityType.Enemy)
                nearbyEnemies.Add(hitColliders[i].gameObject);
        }

        return nearbyEnemies;
    }

    // Summary: Filters the enemies to those in front of the player.
    //
    private List<GameObject> FilterForward(List<GameObject> nearbyEnemies)
    {
        List<GameObject> forwardEnemies = new List<GameObject>();
        Vector3 enemyDirection = Vector3.zero;

        for (int i = 0; i < nearbyEnemies.Count; i++)
        {
            enemyDirection = (nearbyEnemies[i].transform.position - _attachedPlayer.position).normalized;

            if (CheckIfForward(enemyDirection))
                forwardEnemies.Add(nearbyEnemies[i]);
        }

        return forwardEnemies;
    }

    // Summary: Gets the closest enemies out of the forward enemies
    //
    private GameObject GetClosestEnemy(List<GameObject> forwardEnemies)
    {
        GameObject nearestEnemy = forwardEnemies[0];
        float closestDistance = Vector3.Distance(_attachedPlayer.position, forwardEnemies[0].transform.position);

        for (int i = 0; i < forwardEnemies.Count; i++)
        {
            if (CheckMinDistance(_attachedPlayer.position, forwardEnemies[i].transform.position, closestDistance))
                nearestEnemy = forwardEnemies[i];
        }

        return nearestEnemy;
    }

    // Summary: Checks the distance between two positions and whether its below the threshold
    //
    private bool CheckMinDistance(Vector3 playerPos, Vector3 enemyPos, float threshold)
    {
        return Vector3.Distance(playerPos, enemyPos) < threshold;
    }

    // Summary: Checks whether enemy entity is forward
    //
    private bool CheckIfForward(Vector3 enemyDirection)
    {
        //Debug.Log(">> Dot Product Result: " + Vector3.Dot(_attachedPlayer.forward.normalized, enemyDirection));
        return Vector3.Dot(_attachedPlayer.forward.normalized, enemyDirection) > _settings.forwardDotLimit;
    }
}
