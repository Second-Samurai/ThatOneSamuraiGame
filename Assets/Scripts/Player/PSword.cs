using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PSword : MonoBehaviour
{
    private GameSettings _gameSettings;
    private Transform _swordmanTransform;

    // Start is called before the first frame update
    void Start()
    {
        _gameSettings = GameManager.instance.gameSettings;
    }

    //Summary: This sets the sword holder parent of this sword
    //
    public void SetParentTransform(Transform parentTransform)
    {
        _swordmanTransform = parentTransform;
    }

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

    public void CreateImpactEffect(Transform targetPosition, HitType type) //!!!! Might need to change to an enum driven effect
    {
        Vector3 impactPosition = Vector3.zero;
        Vector3 impactRotation = _swordmanTransform.rotation.eulerAngles;

        if (type == HitType.DamageableTarget)
        {
            Debug.Log(">> PSword: Impact Raycast triggered");
            impactPosition = RayCastToHitPoint(targetPosition);
            CreateDamageableImpact(impactPosition, impactRotation);
        }
        else
        {
            Debug.Log(">> PSword: Impact effect triggered");
            impactPosition = this.transform.position;
            CreateGeneralImpact(impactPosition, impactRotation);
        }
    }

    private void CreateDamageableImpact(Vector3 impactPosition, Vector3 impactRotation)
    {
        GameObject sparkImpact = Instantiate(_gameSettings.slashImpact01, impactPosition, Quaternion.Euler(impactRotation));
        GameObject sparkFalloff = Instantiate(_gameSettings.sparkFallOff01, impactPosition, Quaternion.Euler(impactRotation));
        StartCoroutine(DestroyAfterTime(sparkImpact, 4f));
        StartCoroutine(DestroyAfterTime(sparkFalloff, 4f));
    }

    private void CreateGeneralImpact(Vector3 impactPosition, Vector3 impactRotation)
    {
        GameObject sparkFalloff = Instantiate(_gameSettings.sparkFallOff01, impactPosition, Quaternion.Euler(impactRotation));
        StartCoroutine(DestroyAfterTime(sparkFalloff, 4f));
    }

    //Summary: Uses raycast to determine the hitpoint from trigger impact
    //
    private Vector3 RayCastToHitPoint(Transform hitTarget)
    {
        Vector3 startPosition = _swordmanTransform.position;
        startPosition.y = transform.position.y;
        Vector3 rayDirection = hitTarget.transform.position - startPosition;

        RaycastHit hit;
        RaycastHit[] hitResult = Physics.RaycastAll(startPosition, rayDirection, 50f);
        hit = hitResult.Where(x => x.collider.GetComponent<IDamageable>() != null && x.collider.GetComponent<IPlayerController>() == null).First();

        if (hit.collider == null)
        {
            Debug.LogWarning(">> PSword: hit raycast has returned nothing");
            return this.transform.position;
        }

        return hit.point;
    }

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
