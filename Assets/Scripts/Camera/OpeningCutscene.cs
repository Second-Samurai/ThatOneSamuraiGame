using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
using SceneManager = ThatOneSamuraiGame.Scripts.Scene.SceneManager.SceneManager;

public class OpeningCutscene : MonoBehaviour
{
    PlayableDirector _cutsceneDirector;
    UnityEvent endCutscene;
    public SignalReceiver signalReceiver;
    bool bSkipped = false;

    private ICameraController m_CameraController;

    // Start is called before the first frame update
    void Start()
    {
        _cutsceneDirector = GetComponent<PlayableDirector>();
        if (endCutscene == null) endCutscene = new UnityEvent();
        if (signalReceiver == null) signalReceiver = GetComponent<SignalReceiver>();
        AssignTargets();

        this.m_CameraController = SceneManager.Instance.CameraController
            ?? throw new ArgumentNullException(nameof(SceneManager.Instance.CameraController));
    }

   

    void AssignTargets()
    {
        BindToTrack("Cinemachine Track", GameManager.instance.MainCamera.GetComponent<CinemachineBrain>());
        BindToTrack("Animation Track", GameManager.instance.PlayerController.gameObject.GetComponent<Animator>());
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

    // public void ChangeCamPriority()
    //     => GameManager.instance.ThirdPersonViewCamera.GetComponent<ThirdPersonCamController>().SetPriority(11);
    //
    
    public void ChangeCameraToFollow()
        => this.m_CameraController.SelectCamera(SceneCameras.FollowPlayer);

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !bSkipped) 
        {
            _cutsceneDirector.time = _cutsceneDirector.duration - 3;
            bSkipped = true;
        } 
    } 
}
