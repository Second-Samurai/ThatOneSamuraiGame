using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewindBar : MonoBehaviour
{
    public Slider healthSlider;
    public Image rewindBar;
    public Image rewindBarBackground;

    private void Start()
    {
        GameManager.instance.rewindManager.rewindUI = this;
        healthSlider = GetComponent<Slider>();
    }

    public void Bebug() 
    {
        Debug.Log("OI");

    }
    public void UpdateRewindAmount(float amount) 
    {
        healthSlider.value = amount;
        Debug.Log(amount);
    }

    public void UpdateBarMax(float amount)
    {
        float tempValue = healthSlider.value / healthSlider.maxValue;

        healthSlider.maxValue = amount;

        healthSlider.value = healthSlider.maxValue * tempValue;

    }
    private void OnEnable()
    {
        UpdateBarColor();
    }
    public void FadeIn() 
    {
        rewindBar.DOFade(1f, 1f);
        rewindBarBackground.DOFade(1, 1f);
    }

    public void FadeOut()
    {
        rewindBar.DOFade(0, 1f);
        rewindBarBackground.DOFade(0, 1f);

    }

    public void UpdateBarColor() 
    {
        if(GameManager.instance.rewindManager.rewindResource > 2f)
        {
            rewindBar.color = new Color32(76, 101, 221, 100);
        }
        else if (GameManager.instance.rewindManager.rewindResource < 2f)
        {
            rewindBar.color = new Color32(221, 76, 87, 100);
        }
    }
}
