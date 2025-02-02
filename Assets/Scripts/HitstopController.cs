using System.Collections;
using UnityEngine;

public class HitstopController : MonoBehaviour
{

    public bool bIsSlowing = false;

    IEnumerator HitstopCR(float time)
    {
        bIsSlowing = true;
        //float oldTimescale = Time.timeScale;
        Time.timeScale = 0f;
        while (time > 0)
        {
            time -= Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
        bIsSlowing = false;
    }

    IEnumerator SlowTimeCR(float amount, float duration)
    {
        bIsSlowing = true;
        //float oldTimescale = Time.timeScale;
        Time.timeScale = amount;
        while (duration > 0)
        {
            if (Time.timeScale != 0) duration -= Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = 1f;
        bIsSlowing = false;
    }

    public void SlowTime(float amount, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowTimeCR(amount,duration));
    }

    public void Hitstop(float time)
    {
        StopAllCoroutines();
        StartCoroutine(HitstopCR(time));
    }
    public void CancelEffects()
    {
        bIsSlowing = false;
        StopAllCoroutines();
        Time.timeScale = 1f;
    }
}
