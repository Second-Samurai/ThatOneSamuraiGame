using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SwordImpactEffect
{
    private GameSettings _gameSettings;
    private WSwordEffect _swordControl;
    private Transform _swordHolder;

    public void Init(GameSettings settings, WSwordEffect effectControl, Transform parentTransform)
    {
        this._gameSettings = settings;
        this._swordControl = effectControl;
        this._swordHolder = parentTransform;
    }

    //Summary: Creates particle effects when hitting non damageable items
    //
    public void CreateGeneralImpact(Vector3 impactPosition, Vector3 impactRotation)
    {
        GameObject sparkFalloff = GameObject.Instantiate(_gameSettings.sparkFallOff01, impactPosition, Quaternion.Euler(impactRotation));
        _swordControl.StartCoroutine(_swordControl.DestroyAfterTime(sparkFalloff, 4f));
    }

    //Summary: Creates particle effects relevant to player hitting damageable entity
    //
    public void CreateDamageableImpact(Vector3 impactPosition, Vector3 impactRotation)
    {
        GameObject sparkImpact = GameObject.Instantiate(_gameSettings.slashImpact01, impactPosition, Quaternion.Euler(impactRotation));
        GameObject sparkFalloff = GameObject.Instantiate(_gameSettings.sparkFallOff01, impactPosition, Quaternion.Euler(impactRotation));
        _swordControl.StartCoroutine(_swordControl.DestroyAfterTime(sparkImpact, 4f));
        _swordControl.StartCoroutine(_swordControl.DestroyAfterTime(sparkFalloff, 4f));
    }

    //Summary: Uses raycast to determine the hitpoint from player to target
    //
    public Vector3 RayCastToHitPoint(Transform hitTarget)
    {
        Vector3 startPosition = _swordHolder.position;
        startPosition.y = _swordControl.transform.position.y;
        Vector3 rayDirection = hitTarget.transform.position - startPosition;

        RaycastHit hit;
        RaycastHit[] hitResult = Physics.RaycastAll(startPosition, rayDirection, 15f);
        if (hitResult.Length == 0)
        {
            return this._swordControl.transform.position;
        }

        hit = hitResult.Where(predicate: x => x.collider.GetComponent<IDamageable>() != null).FirstOrDefault();

        if (hit.collider == null)
            return this._swordControl.transform.position;

        if (hit.collider.GetComponent<IDamageable>().GetEntityType() != EntityType.Player)
        {
            return this._swordControl.transform.position;
        }

        return hit.point;
    }
}

public enum HitType
{
    DamageableTarget,
    GeneralTarget,
}
