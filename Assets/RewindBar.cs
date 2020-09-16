using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewindBar : MonoBehaviour
{
    Slider healthSlider;

    public void UpdateRewindAmount(float amount) 
    {
        healthSlider.value = amount;
    }
}
