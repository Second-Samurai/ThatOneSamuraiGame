using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CloseEnemyGuideControl
{
    private Transform _attachedPlayer;
    private Transform _lastEnemy = null;

    public void Init(Transform playerTransform)
    {
        this._attachedPlayer = playerTransform;
    }

    /// <summary>
    /// This finds the nearest forward object and steps closer in direction.
    /// </summary>
    public void MoveToNearestEnemy()
    {
        //Check if last enemy is null else slide to player
        Debug.Log(">> Closest Enemy: " + FindClosestEnemy());
        //TODO: Check if last enerfy is still valid to attack
    }

    // Summary: Modifies player's movement to slide closer during attack to enemy
    //
    private void SlideToEnemy(Transform closestEnemy)
    {

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
        List<Collider> hitColliders = Physics.OverlapSphere(_attachedPlayer.position, 10).Where(x => x.GetComponent<IDamageable>() != null).ToList();
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
        float calcDistance = 0;

        for (int i = 0; i < forwardEnemies.Count; i++)
        {
            calcDistance = Vector3.Distance(_attachedPlayer.position, forwardEnemies[i].transform.position);

            if (calcDistance < closestDistance)
                nearestEnemy = forwardEnemies[i];
        }

        return nearestEnemy;
    }

    // Summary: Checks whether enemy entity is forward
    //
    private bool CheckIfForward(Vector3 enemyDirection)
    {
        Debug.Log(">> Dot Product Result: " + Vector3.Dot(_attachedPlayer.forward.normalized, enemyDirection));
        return Vector3.Dot(_attachedPlayer.forward.normalized, enemyDirection) > 0.7f;
    }

    // Summary: Checks whether enemy entitiy is on the same elevation
    //
    private bool CheckIfSameElevation(Transform enemyTransform)
    {
        float heightDist = Vector3.Distance(_attachedPlayer.position, enemyTransform.position);
        if (heightDist > 1.5f) return false;

        return true;
    }

}
