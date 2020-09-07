using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WSwordEffect : MonoBehaviour
{
    private GameSettings _gameSettings;
    private Transform _swordmanTransform;

    // Start is called before the first frame update
    void Start()
    {
        _gameSettings = GameManager.instance.gameSettings;
    }

    /// <summary>
    /// This sets the sword holder transform to this sword
    /// </summary>
    public void SetParentTransform(Transform parentTransform)
    {
        _swordmanTransform = parentTransform;
    }

    /// <summary>
    /// Creates particle slash effect when triggered
    /// </summary>
    public void CreateSlashEffect(float slashAngle)
    {
        //Sets position vector of sword
        Vector3 spawnPosition = _swordmanTransform.position;
        spawnPosition.y = transform.position.y;

        //Sets rotation vector of sword
        Vector3 spawnRotation = _swordmanTransform.rotation.eulerAngles;
        spawnRotation.z = slashAngle; //Note angle 0 is from pure right

        GameObject slash = Instantiate(_gameSettings.swordSlash01, spawnPosition, Quaternion.Euler(spawnRotation));
        slash.transform.parent = _swordmanTransform;
        StartCoroutine(DestroyAfterTime(slash, 3f));
    }

    /// <summary>
    /// Creates particle parry effect when triggered
    /// </summary>
    public void CreateParryEffect(float slashAngle)
    {

    }

    /// <summary>
    /// Creates impact efects during trigger collision
    /// </summary>
    public void CreateImpactEffect(Transform targetPosition, HitType type) //!!!! Might need to change to an enum driven effect
    {
        Vector3 impactPosition = Vector3.zero;
        Vector3 impactRotation = _swordmanTransform.rotation.eulerAngles;

        if (type == HitType.DamageableTarget)
        {
            //Debug.Log(">> PSword: Impact Raycast triggered");
            impactPosition = RayCastToHitPoint(targetPosition);
            CreateDamageableImpact(impactPosition, impactRotation);
        }
        else
        {
            //Debug.Log(">> PSword: Impact effect triggered");
            impactPosition = this.transform.position;
            CreateGeneralImpact(impactPosition, impactRotation);
        }
    }

    //Summary: Creates particle effects relevant to player hitting damageable entity
    //
    private void CreateDamageableImpact(Vector3 impactPosition, Vector3 impactRotation)
    {
        GameObject sparkImpact = Instantiate(_gameSettings.slashImpact01, impactPosition, Quaternion.Euler(impactRotation));
        GameObject sparkFalloff = Instantiate(_gameSettings.sparkFallOff01, impactPosition, Quaternion.Euler(impactRotation));
        StartCoroutine(DestroyAfterTime(sparkImpact, 4f));
        StartCoroutine(DestroyAfterTime(sparkFalloff, 4f));
    }

    //Summary: Creates particle effects when hitting non damageable items
    //
    private void CreateGeneralImpact(Vector3 impactPosition, Vector3 impactRotation)
    {
        GameObject sparkFalloff = Instantiate(_gameSettings.sparkFallOff01, impactPosition, Quaternion.Euler(impactRotation));
        StartCoroutine(DestroyAfterTime(sparkFalloff, 4f));
    }

    private void CreateParryEffect()
    {

    }

    //Summary: Uses raycast to determine the hitpoint from player to target
    //
    private Vector3 RayCastToHitPoint(Transform hitTarget)
    {
        Vector3 startPosition = _swordmanTransform.position;
        startPosition.y = transform.position.y;
        Vector3 rayDirection = hitTarget.transform.position - startPosition;

        RaycastHit hit;
        RaycastHit[] hitResult = Physics.RaycastAll(startPosition, rayDirection, 15f);
        if(hitResult.Length == 0)
        {
            return this.transform.position;
        }

        hit = hitResult.Where(predicate: x => x.collider.GetComponent<IDamageable>() != null).FirstOrDefault();

        if (hit.collider == null)
        {
            Debug.LogWarning(">> PSword: hit raycast has returned nothing");
            return this.transform.position;
        }

        if (hit.collider.GetComponent<IDamageable>().GetEntityType() != EntityType.Player)
        {
            return this.transform.position;
        }

        return hit.point;
    }

    //Summary: Destroys after the timer completes
    //
    IEnumerator DestroyAfterTime(GameObject effect, float time)
    {
        float timer = time;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        Destroy(effect);
    }
}

public enum HitType
{
    DamageableTarget,
    GeneralTarget,
}
