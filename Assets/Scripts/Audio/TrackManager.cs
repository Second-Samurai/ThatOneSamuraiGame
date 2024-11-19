using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    private AudioManager _audioManager;
    public AudioClip drumClip, violinLeadClip, violinHarmonyClip, fluteClip, lowStringClip, shakaClip;
    public AudioSource drums, violinLead, violinHarmony, flute, lowString, shaka;
    public bool drumsActive, violinLeadActive, violinHarmonyActive, fluteActive, lowStringActive, shakaActive, pause;

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = AudioManager.instance;
        pause = false;

        drumsActive = false;
        drums.clip = drumClip;
        drums.loop = true;
        drums.volume = 0;
        drums.Play();

        violinLeadActive = false;
        violinLead.clip = violinLeadClip;
        violinLead.loop = true;
        violinLead.volume = 0;
        violinLead.Play();

        violinHarmonyActive = false;
        violinHarmony.clip = violinHarmonyClip;
        violinHarmony.loop = true;
        violinHarmony.volume = 0;
        violinHarmony.Play();

        fluteActive = false;
        flute.clip = fluteClip;
        flute.loop = true;
        flute.volume = 0;
        flute.Play();

        lowStringActive = false;
        lowString.clip = lowStringClip;
        lowString.loop = true;
        lowString.volume = 0;
        lowString.Play();

        shakaActive = false;
        shaka.clip = shakaClip;
        shaka.loop = true;
        shaka.volume = 0;
        shaka.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        if (drumsActive) drums.volume = _audioManager.BGMVol/2;
        if (violinHarmonyActive) violinHarmony.volume = _audioManager.BGMVol;
        if (violinLeadActive) violinLead.volume = _audioManager.BGMVol;
        if (fluteActive) flute.volume = _audioManager.BGMVol;
        if (lowStringActive) lowString.volume = _audioManager.BGMVol;
        if (shakaActive) shaka.volume = _audioManager.BGMVol;
        
    }


    public void FadeOutAll() 

    {
        DrumsFade(false);
        ViolinLeadFade(false);
        ViolinHarmonyFade(false);
        FluteFade(false);
        LowStringFade(false);
        Shakafade(false);

        drumsActive = false;
        violinHarmonyActive = false;
        violinLeadActive = false;
        fluteActive = false;
        lowStringActive = false;
        shakaActive = false;
        //drums.Stop();
        //violinLead.Stop();
        //violinHarmony.Stop();
        //flute.Stop();
        //lowString.Stop();
        //shaka.Stop();


    }

    public void DrumsFade(bool Fadein) 
    {
            TweenCallback callback = DrumsActive;
        if (Fadein)
            drums.DOFade(_audioManager.BGMVol / 2, 2).OnComplete(callback);
        else 
            drums.DOFade(0, 2);
        
    }

    public void DrumsFadeBetween(bool Fadein)
    {
        TweenCallback callback = DrumsActive;
        if (Fadein)
        {
            drums.DOFade(_audioManager.BGMVol * 2, 2).OnComplete(callback);

        }
        else
        {
            drums.DOFade(_audioManager.BGMVol / 2, 2).OnComplete(callback);
        }

    }

    public void DrumsActive() {drumsActive = !drumsActive; }

    public void ViolinLeadFade(bool Fadein)
    {
        TweenCallback callback = ViolinLeadActive;
        if (Fadein)
            violinLead.DOFade(_audioManager.BGMVol, 2).OnComplete(callback);
        else
            violinLead.DOFade(0, 2);
    }

    public void ViolinLeadActive() { violinLeadActive = !violinLeadActive; }

    public void ViolinHarmonyFade(bool Fadein)
    {
        TweenCallback callback = ViolinHarmonyActive;

        if (Fadein)
            violinHarmony.DOFade(_audioManager.BGMVol, 2).OnComplete(callback);
        else
            violinHarmony.DOFade(0, 2);
    }

    public void ViolinHarmonyActive() { violinHarmonyActive = !violinHarmonyActive; }


    public void FluteFade(bool Fadein)
    {
        TweenCallback callback = FluteActive;

        if (Fadein)
            flute.DOFade(_audioManager.BGMVol, 2).OnComplete(callback);
        else
            flute.DOFade(0, 2);
    }
    public void FluteActive() { fluteActive = !fluteActive; }
  
    public void LowStringFade(bool Fadein)
    {
        TweenCallback callback = LowStringActive;

        if (Fadein)
            lowString.DOFade(_audioManager.BGMVol, 2).OnComplete(callback);
        else
            lowString.DOFade(0, 2);
    }

    public void LowStringActive() { lowStringActive = !lowStringActive; }


    public void Shakafade(bool Fadein)
    {
        TweenCallback callback = ShakaActive;

        if (Fadein)
            shaka.DOFade(_audioManager.BGMVol, 2).OnComplete(callback);
        else
            shaka.DOFade(0, 2);
    }
    public void ShakaActive() { shakaActive = !shakaActive; }


    public void PauseAll()
    {
        pause = !pause;
        if (pause == true)
        {
            drums.Pause();
            violinLead.Pause();
            violinHarmony.Pause();
            flute.Pause();
            lowString.Pause();
            shaka.Pause();
        }
        else if (pause == false) 
        {
            drums.UnPause();
            violinLead.UnPause();
            violinHarmony.UnPause();
            flute.UnPause();
            lowString.UnPause();
            shaka.UnPause();
        }
    }

}


