using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.InputSystem;

public class OpeningCutscene : MonoBehaviour
{
    PlayableDirector _cutsceneDirector;
    UnityEvent endCutscene;
    public SignalReceiver signalReceiver;
    bool bSkipped = false;

    // Start is called before the first frame update
    void Start()
    {
        _cutsceneDirector = GetComponent<PlayableDirector>();
        if (endCutscene == null) endCutscene = new UnityEvent();
        if (signalReceiver == null) signalReceiver = GetComponent<SignalReceiver>();
        AssignTargets();
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

    public void ChangeCamPriority()
        => GameManager.instance.ThirdPersonViewCamera.GetComponent<ThirdPersonCamController>().SetPriority(11);

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !bSkipped) 
        {
            _cutsceneDirector.time = _cutsceneDirector.duration - 3;
            bSkipped = true;
        } 
    } 
}
