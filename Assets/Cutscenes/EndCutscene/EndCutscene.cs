using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndCutscene : MonoBehaviour
{
    PlayableDirector _cutsceneDirector;
    UnityEvent endCutscene;
    public SignalReceiver signalReceiver;
    bool bSkipped = false;
    public GameObject credits, boss;
    public Transform playerPoint, bossPoint;
    private BossThemeManager bossTheme;

    // Start is called before the first frame update
    void Start()
    {
        bossTheme = GameManager.instance.audioManager.BossThemeManager;
        _cutsceneDirector = GetComponent<PlayableDirector>();
        if (endCutscene == null) endCutscene = new UnityEvent();
        if (signalReceiver == null) signalReceiver = GetComponent<SignalReceiver>();
        AssignTargets();
    }



    void AssignTargets()
    {
        BindToTrack("Cinemachine Track", GameManager.instance.MainCamera.GetComponent<CinemachineBrain>());
        BindToTrack("Animation Track", GameManager.instance.PlayerController.gameObject.GetComponent<Animator>());

        BindToTrack("Activation Track", GameManager.instance.PlayerController.gameObject.GetComponent<PCombatController>().swordManager.swordEffect.gameObject);
        //endCutscene.AddListener(GameManager.instance.playerController.gameObject.GetComponent<PlayerInputScript>().EnableInput);
        //signalReceiver.ChangeReactionAtIndex(1, endCutscene);
        // signalReceiver.AddEmptyReaction(endCutscene);

    }

    void BindToTrack(string trackName, Object val)
    {
        foreach (var playableAssetOutput in _cutsceneDirector.playableAsset.outputs)
        {
            if (playableAssetOutput.streamName == trackName)
            {
                _cutsceneDirector.SetGenericBinding(playableAssetOutput.sourceObject, val);
                break;
            }
        }
    }

    public void ChangeCamPriority()
    {
        //Debug.Log("2");
        GameManager.instance.ThirdPersonViewCamera.GetComponent<ThirdPersonCamController>().SetPriority(11);
    }

    private void Update()
    {
        //if (Keyboard.current.escapeKey.wasPressedThisFrame && !bSkipped)
        //{
        //    _cutsceneDirector.time = _cutsceneDirector.duration - 3;
        //    bSkipped = true;
        //}
    }

    public void StartRewindRecording()
    {
        GameManager.instance.RewindManager.isTravelling = false;
    }

    public void PlayCutscene()
    {
        BackgroundAudio audio = AudioManager.instance.gameObject.GetComponent<BackgroundAudio>();
        audio.PauseMusic();
        bossTheme.StopAll();
        audio.Invoke("PlayMenuMusic", 3);
        Invoke("PlayClip", 3);
    }

    public void RollCredits()
    {
        credits.SetActive(true);
    }

    public void PlayClip()
    {
        DisableInput();
        GameManager.instance.PlayerController.gameObject.transform.position = playerPoint.position;
        boss.transform.position = bossPoint.position;
        _cutsceneDirector.Play();
    }

    public void ReloadGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisableInput() 
        => GameManager.instance.InputManager.DisableActiveInputControl();
}
