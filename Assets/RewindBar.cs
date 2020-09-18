using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindBar : MonoBehaviour
{
    Slider healthSlider;
    public Image rewindBar;

    private void Start()
    {
        GameManager.instance.rewindManager.rewindUI = this;
        healthSlider = GetComponent<Slider>();
    }

    public void UpdateRewindAmount(float amount) 
    {
        healthSlider.value = amount;
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

    public void UpdateBarColor() 
    {
        if (GameManager.instance.rewindManager.rewindResource <= 2f)
        {
            rewindBar.GetComponent<Image>().color = new Color32(221, 76, 87, 100);
        }
        else 
        {
            rewindBar.GetComponent<Image>().color = new Color32(76, 101, 221, 100);
        }
    }
}
