using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewindBar : MonoBehaviour
{
    public Image rewindBar;
    public Image rewindBarBackground;

    public float maxValue = 1.0f;

    private void Start()
    {
        GameManager.instance.rewindManager.rewindUI = this;
        rewindBar.fillAmount = 0;
    }
    
    public void UpdateRewindAmount(float amount) 
    {
        if (rewindBar.fillAmount <= maxValue)
        {
            rewindBar.fillAmount = amount / 10.0f;
        }
        else if (rewindBar.fillAmount > maxValue)
        {
            rewindBar.fillAmount = maxValue;
        }
        
    }

    public void UpdateBarMax(float amount)
    {
        // float tempValue = healthSlider.fillAmount / healthSlider.maxValue;
        //
        maxValue = amount / 10.0f;
        // healthSlider.maxValue = amount;
        //
        // healthSlider.value = healthSlider.maxValue * tempValue;

    }
    private void OnEnable()
    {
        UpdateBarColor();
    }
    public void FadeIn(float alpha, float time) 
    {
        rewindBar.enabled = true;
        rewindBarBackground.DOFade(alpha, time);
        rewindBar.DOFade(alpha, time);
    }

    public void FadeOut(float alpha, float time)
    {
        rewindBarBackground.DOFade(alpha, time);
        rewindBar.DOFade(alpha, time);
        rewindBar.enabled = false;
    }


    public void UpdateBarColor() 
    {
        if (GameManager.instance.rewindManager.rewindResource > 2f)
        {
            rewindBar.color = new Color32(76, 101, 221, 255);
        }
        else if (GameManager.instance.rewindManager.rewindResource < 2f)
        {
            rewindBar.color = new Color32(221, 76, 87, 255);
        }
    }
}
