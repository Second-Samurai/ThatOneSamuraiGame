using System.Collections;
using System.Collections.Generic;
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

    public void SetParentTransform(Transform parentTransform)
    {
        _swordmanTransform = parentTransform;
    }

    public void CreateSlashEffect(float slashAngle)
    {
        Vector3 spawnPosition = _swordmanTransform.position;
        spawnPosition.y = transform.position.y;

        //Sets rotation vector of sword
        Vector3 spawnRotation = _swordmanTransform.rotation.eulerAngles;
        spawnRotation.z = slashAngle; //Note angle 0 is from pure right

        GameObject slash = Instantiate(_gameSettings.swordSlash01, spawnPosition, Quaternion.Euler(spawnRotation));
        slash.transform.parent = _swordmanTransform;
        StartCoroutine(DestroyAfterTime(slash));
    }

    IEnumerator DestroyAfterTime(GameObject slashEffect)
    {
        float timer = 3f;

        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        Destroy(slashEffect);
    }
}
