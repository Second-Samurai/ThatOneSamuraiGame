using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    private AudioManager _audioManager;
    public AudioSource drums, violinLead, violinHarmony, flute, lowString, shaka;
   
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameManager.instance.audioManager;

        drums.volume = 0;
        drums.Play();

        violinLead.volume = 0;
        violinLead.Play();

        violinHarmony.volume = 0;
        violinHarmony.Play();

        flute.volume = 0;
        flute.Play();

        lowString.volume = 0;
        lowString.Play();

        shaka.volume = 0;
        shaka.Play();
    }

    // Update is called once per frame
    public void DrumsFade(bool Fadein) 
    {
        if (Fadein)
        {
            drums.DOFade(0, _audioManager.BGMVol);
        }
        else 
        {
            drums.DOFade(_audioManager.BGMVol, 0);
        }
        
    }

    public void ViolinLeadFade(bool Fadein)
    {
        if (Fadein)
        {
            violinLead.DOFade(0, _audioManager.BGMVol);
        }
        else
        {
            violinLead.DOFade(_audioManager.BGMVol, 0);
        }

    }

    public void ViolinHarmonyFade(bool Fadein)
    {
        if (Fadein)
        {
            violinHarmony.DOFade(0, _audioManager.BGMVol);
        }
        else
        {
            violinHarmony.DOFade(_audioManager.BGMVol, 0);
        }

    }

    public void FluteFade(bool Fadein)
    {
        if (Fadein)
        {
            flute.DOFade(0, _audioManager.BGMVol);
        }
        else
        {
            flute.DOFade(_audioManager.BGMVol, 0);
        }

    }

    public void LowStringFade(bool Fadein)
    {
        if (Fadein)
        {
            lowString.DOFade(0, _audioManager.BGMVol);
        }
        else
        {
            lowString.DOFade(_audioManager.BGMVol, 0);
        }

    }

    public void Shakafade(bool Fadein)
    {
        if (Fadein)
        {
            shaka.DOFade(0, _audioManager.BGMVol);
        }
        else
        {
            shaka.DOFade(_audioManager.BGMVol, 0);
        }

    }


}


