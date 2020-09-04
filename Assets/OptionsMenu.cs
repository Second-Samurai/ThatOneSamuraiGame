using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public FreeLookAddOn camera;
    Slider _sensitivitySlider;

    private void Start()
    {
        camera = GameManager.instance.playerController.gameObject.GetComponent<CameraControl>()._camScript;
        _sensitivitySlider = GetComponentInChildren<Slider>();
        _sensitivitySlider.value = camera.lookSpeed;
    }

    public void SetSensitivity(float var)
    {
        camera.SetSensitivity(var);
    }
}
