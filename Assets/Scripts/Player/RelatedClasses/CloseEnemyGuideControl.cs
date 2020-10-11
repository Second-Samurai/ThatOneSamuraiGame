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

    private PlayerSettings settings;

    private List<GameObject> _enemyCollection;
    private GameObject _closestEnemy;

    public void Init(PCombatController playerCombat, Transform playerTransform, Rigidbody rigidbody)
    {
        this._attachedPlayer = playerTransform;
        settings = GameManager.instance.gameSettings.playerSettings;

        _enemyCollection = new List<GameObject>();

        _slideController = new AttackSlide();
        _slideController.Init(playerCombat, rigidbody);
    }

    /// <summary>
    /// This finds the nearest forward object and steps closer in direction.
    /// </summary>
    public void MoveToNearestEnemy()
    {
        if (CheckLastAttackedEnemy()) return;

        FindClosestEnemy();

        if (_closestEnemy != null)
        {
            _lastEnemy = _closestEnemy.transform;
            _slideController.SlideToEnemy(_lastEnemy);
            _closestEnemy = null;
        }
        else
        {
            _slideController.SlideForward();
        }
    }

    // Summary: Checks whether last registered attacked enemy is valid to attack
    //
    private bool CheckLastAttackedEnemy()
    {
        if (_lastEnemy != null)
        {
            Vector3 direction = (_lastEnemy.position - _attachedPlayer.position).normalized;
            if (CheckIfForward(direction) && CheckMinDistance(_attachedPlayer.position, _lastEnemy.position, settings.minimumAttackDist))
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
        CollectNearbyObjects();

        //Checks for edge cases
        if (_enemyCollection.Count == 0) return null;
        if (_enemyCollection.Count == 1)
        {
            Vector3 direction = (_enemyCollection[0].transform.position - _attachedPlayer.position).normalized;
            if (CheckIfForward(direction)) return _enemyCollection[0];

            return null;
        }

        //Performs filtering through collection
        _enemyCollection = FilterForwardEnemies();
        GetClosestEnemy();
        _enemyCollection.Clear();

        return _closestEnemy;
    }

    // Summary: Gets the nearby objects around the player
    //
    private void CollectNearbyObjects()
    {
        List<Collider> hitColliders = new List<Collider>();
        hitColliders = Physics.OverlapSphere(_attachedPlayer.position, settings.detectionRadius).Where(x => x.GetComponent<IDamageable>() != null).ToList();
        IDamageable entities;

        //Filters through collecting the enemy entities
        for (int i = 0; i < hitColliders.Count; i++)
        {
            entities = hitColliders[i].GetComponent<IDamageable>();

            if(entities.GetEntityType() == EntityType.Enemy)
                _enemyCollection.Add(hitColliders[i].gameObject);
        }
    }

    // Summary: Filters the enemies to those in front of the player.
    //
    private List<GameObject> FilterForwardEnemies()
    {
        List<GameObject> forwardEnemies = new List<GameObject>();
        Vector3 enemyDirection = Vector3.zero;

        for (int i = 0; i < _enemyCollection.Count; i++)
        {
            enemyDirection = (_enemyCollection[i].transform.position - _attachedPlayer.position).normalized;

            if (CheckIfForward(enemyDirection))
                forwardEnemies.Add(_enemyCollection[i]);
        }

        return forwardEnemies;
    }

    // Summary: Gets the closest enemies out of the forward enemies
    //
    private GameObject GetClosestEnemy()
    {
        if (_enemyCollection.Count == 0) return null;

        //Initialise variables
        _closestEnemy = null;
        _closestEnemy = _enemyCollection[0];
        float closestDistance = Vector3.Magnitude(_attachedPlayer.position - _closestEnemy.transform.position);

        for (int i = 0; i < _enemyCollection.Count; i++)
        {
            if (CheckMinDistance(_attachedPlayer.position, _enemyCollection[i].transform.position, closestDistance))
                _closestEnemy = _enemyCollection[i];
        }

        return _closestEnemy;
    }

    // Summary: Checks the distance between two positions and whether its below the threshold
    //
    private bool CheckMinDistance(Vector3 playerPos, Vector3 enemyPos, float threshold)
    {
        return Vector3.Magnitude(playerPos - enemyPos) < threshold;
    }

    // Summary: Checks whether enemy entity is forward
    //
    private bool CheckIfForward(Vector3 enemyDirection)
    {
        return Vector3.Dot(_attachedPlayer.forward.normalized, enemyDirection) > settings.forwardDotLimit;
    }
}
