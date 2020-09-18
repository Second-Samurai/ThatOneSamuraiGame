using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WSwordEffect : MonoBehaviour
{
    private GameSettings _gameSettings;
    private Transform _swordmanTransform;

    //Effect scripts
    private SwordImpactEffect _impactEffector;
    private ParryEffect _parryEffect;


    /// <summary>
    /// This sets the sword holder transform to this sword
    /// </summary>
    public void Init(Transform parentTransform)
    {
        _gameSettings = GameManager.instance.gameSettings;
        _swordmanTransform = parentTransform;

        _impactEffector = new SwordImpactEffect();
        _impactEffector.Init(_gameSettings, this, parentTransform);

        _parryEffect = new ParryEffect();
        _parryEffect.Init();
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
    public void CreateParryEffect(float slashAngle, Transform targetPosition, ParryType type)
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
            impactPosition = _impactEffector.RayCastToHitPoint(targetPosition);
            _impactEffector.CreateDamageableImpact(impactPosition, impactRotation);
        }
        else
        {
            impactPosition = this.transform.position;
            _impactEffector.CreateGeneralImpact(impactPosition, impactRotation);
        }
    }

    //Summary: Destroys after the timer completes
    //
    public IEnumerator DestroyAfterTime(GameObject effect, float time)
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

