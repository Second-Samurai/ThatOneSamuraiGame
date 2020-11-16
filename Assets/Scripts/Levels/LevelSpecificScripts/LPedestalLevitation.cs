using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In this class there is to be no background task running.
/// Instead majority of the functionality relies on coroutines and event tasks.
/// </summary>
public class LPedestalLevitation : MonoBehaviour
{
    [Header("Sound Attached")]
    public AudioSource earthquakeAudio;

    [Space]
    public Transform[] saberCrystals;
    public Transform[] walkwayTiles;
    public Transform crystalParent;

    private List<float> tileHeights = new List<float>();

    private Vector3 crystalParentPosition;
    private bool isLevitating = false;
    private bool hasTriggered = false;

    public void Start()
    {
        foreach(Transform tile in walkwayTiles)
        {
            tileHeights.Add(tile.localPosition.y);
            tile.localPosition = new Vector3(tile.localPosition.x, -4.5f, tile.localPosition.z);
        }
    }

    private void FixedUpdate()
    {
        CrystalMotion();
    }

    public void CrystalMotion()
    {
        if (!isLevitating) return;

        crystalParentPosition = crystalParent.localPosition;
        crystalParent.localPosition = crystalParentPosition + transform.up * Mathf.Sin(Time.time * 0.8f) * 0.03f;
    }

    // LEVITATING WALKWAY
    public IEnumerator LevitatePathway()
    {
        //Setup bool checks for completed translations
        bool[] hasObjectCompleted = new bool[walkwayTiles.Length];
        for (int i = 0; i < hasObjectCompleted.Length; i++)
        {
            hasObjectCompleted[i] = false;
        }

        Vector3 tilePosition;
        bool isCompleted = false;
        int index = 0;

        while (!isCompleted)
        {
            if (!hasObjectCompleted[index])
            {
                Debug.Log("Is Lerping");
                tilePosition = walkwayTiles[index].localPosition;
                walkwayTiles[index].localPosition = new Vector3(tilePosition.x, tilePosition.y + 0.07f, tilePosition.z);
                if (walkwayTiles[index].localPosition.y >= tileHeights[index]) {
                    walkwayTiles[index].localPosition = new Vector3(tilePosition.x, tileHeights[index], tilePosition.z);
                    hasObjectCompleted[index] = true;
                    index++;
                    yield return null;
                }
            }

            if (hasObjectCompleted[hasObjectCompleted.Length - 1])
            {
                Debug.Log("Has completed");
                isCompleted = true;
                yield return null;
            }

            //index++;
            yield return null;
        }
    }

    // LEVITATING SABER CRYSTALS
    public IEnumerator LevitateCrystals()
    {
        Vector3 parentPosition;

        while (crystalParent.localPosition.y < 1.3f)
        {
            parentPosition = crystalParent.localPosition;
            parentPosition = new Vector3(parentPosition.x, parentPosition.y + 7 * 0.01f, parentPosition.z);
            crystalParent.localPosition = parentPosition;
            yield return null;
        }

        isLevitating = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            StartCoroutine("LevitatePathway");
            StartCoroutine("LevitateCrystals");

            earthquakeAudio.Play();
            hasTriggered = true;
        }
    }
}
