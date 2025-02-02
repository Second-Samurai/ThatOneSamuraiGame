using ThatOneSamuraiGame.Legacy;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public PlayerCamTargetController camTargetScript;
    public AudioManager audioManager;
    Slider _sensitivitySlider;
    public Slider BGMVol;
    public Slider SFXVol;
    public Toggle indicatorToggle;

 

    private void Start()
    {
        // TODO: Change this to be the current camera control.
        // camTargetScript = GameManager.instance.PlayerController.gameObject.GetComponent<CameraControl>().camTargetScript;
        audioManager = AudioManager.instance;
        _sensitivitySlider = GetComponentInChildren<Slider>();
        _sensitivitySlider.value = camTargetScript.rotationSpeed;
        if (!indicatorToggle) indicatorToggle = GetComponentInChildren<Toggle>();
        if(PlayerPrefs.GetInt("ShowIndicators") == 1) ToggleIndicators(true);
        else ToggleIndicators(false);
        indicatorToggle.isOn = GameManager.instance.bShowAttackPopups;
        SFXVol.value = PlayerPrefs.GetFloat("SFXVolume");
        BGMVol.value = PlayerPrefs.GetFloat("BGMVolume");
    }

    public void SetSensitivity(float var)
    {
        camTargetScript.SetSensitivity(var);
    }

    public void SetBGMVolume() 
    {
        audioManager.BGMVol = BGMVol.value;
        PlayerPrefs.SetFloat("BGMVolume", BGMVol.value);
    }

    public void SetSFXVolume()
    {
        audioManager.SFXVol = SFXVol.value;
        PlayerPrefs.SetFloat("SFXVolume", SFXVol.value);
    }

    public void ToggleIndicators(bool val)
    {
        GameManager.instance.bShowAttackPopups = val;
        if(val) PlayerPrefs.SetInt("ShowIndicators", 1);
        else PlayerPrefs.SetInt("ShowIndicators", 0);
    }

   
}
