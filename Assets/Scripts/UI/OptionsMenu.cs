using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public PlayerCamTargetController camTargetScript;
    Slider _sensitivitySlider; 

    private void Start()
    {
        camTargetScript = GameManager.instance.playerController.gameObject.GetComponent<CameraControl>().camTargetScript;
        _sensitivitySlider = GetComponentInChildren<Slider>();
        _sensitivitySlider.value = camTargetScript.rotationSpeed;
    }

    public void SetSensitivity(float var)
    {
        camTargetScript.SetSensitivity(var);
    }
}
