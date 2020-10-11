using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public PlayerCamTargetController camTargetScript;
    Slider _sensitivitySlider;
    public Toggle indicatorToggle;

    private void Start()
    {
        camTargetScript = GameManager.instance.playerController.gameObject.GetComponent<CameraControl>().camTargetScript;
        _sensitivitySlider = GetComponentInChildren<Slider>();
        _sensitivitySlider.value = camTargetScript.rotationSpeed;
        if (!indicatorToggle) indicatorToggle = GetComponentInChildren<Toggle>();
        if(PlayerPrefs.GetInt("ShowIndicators") == 1) ToggleIndicators(true);
        else ToggleIndicators(false);
        indicatorToggle.isOn = GameManager.instance.bShowAttackPopups;
    }

    public void SetSensitivity(float var)
    {
        camTargetScript.SetSensitivity(var);
    }

    public void ToggleIndicators(bool val)
    {
        GameManager.instance.bShowAttackPopups = val;
        if(val) PlayerPrefs.SetInt("ShowIndicators", 1);
        else PlayerPrefs.SetInt("ShowIndicators", 0);
    }
}
