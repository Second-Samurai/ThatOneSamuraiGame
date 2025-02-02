using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PostProcessingController : MonoBehaviour
{
    public Volume defaultVolume, rewindVolume;
    ChromaticAberration _ca;
    public ColorAdjustments _colorAdjustments;
    LensDistortion _ld;

    // Start is called before the first frame update
    void Start()
    {
        rewindVolume.profile.TryGet<LensDistortion>(out _ld);
        //volume = GetComponent<Volume>();
        //volume.profile.TryGet<ChromaticAberration>(out _ca);
        //volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments);
    }

    public void SetChromaticAberration(float f)
    {
        _ca.intensity.value = f;
    }

    public void EnableRewindColourFilter()
    {
        rewindVolume.enabled = true;
        defaultVolume.enabled = false;
    }

    public void DisableRewindColourFilter()
    {
        rewindVolume.enabled = false;
        defaultVolume.enabled = true;
    }

    public void WarpLensToTargetAmount(float f)
    {
        StopAllCoroutines();
        StartCoroutine(Warp(f));
    }
    
    public IEnumerator Warp(float f)
    {
        if(_ld.intensity.value < f)
        {
            while (_ld.intensity.value != f)
            {
                if (_ld.intensity.value < f) _ld.intensity.value += Time.unscaledDeltaTime;
                else if (_ld.intensity.value > f) _ld.intensity.value = f;
                yield return null;
            }
        }
        else if (_ld.intensity.value > f)
        {
            while (_ld.intensity.value != f)
            {
                if (_ld.intensity.value > f) _ld.intensity.value -= Time.unscaledDeltaTime;
                else if (_ld.intensity.value < f) _ld.intensity.value = f;
                yield return null;
            }
        }

    }
}
