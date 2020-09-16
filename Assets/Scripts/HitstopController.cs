using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitstopController : MonoBehaviour
{
    IEnumerator HitstopCR(float time)
    {
        float oldTimescale = Time.timeScale;
        Time.timeScale = 0f;
        while (time > 0)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = oldTimescale;
    }

    public void Hitstop(float time)
    {
        StopAllCoroutines();
        StartCoroutine(HitstopCR(time));
    }
}
