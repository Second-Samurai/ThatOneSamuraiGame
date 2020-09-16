using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindBar : MonoBehaviour
{
    Slider healthSlider;

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
}
