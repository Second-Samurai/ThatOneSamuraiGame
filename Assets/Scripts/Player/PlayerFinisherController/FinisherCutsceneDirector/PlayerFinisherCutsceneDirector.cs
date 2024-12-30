using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Responsible for finisher cutscene camera movements.
/// </summary>
public class PlayerFinisherCutsceneDirector : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [RequiredField]
    [SerializeField] 
    private PlayableDirector m_PlayableDirector;

    #endregion Fields
  
    #region - - - - - - Methods - - - - - -

    public void BindToTrack(string trackName, Object val)
    {
        foreach (var playableAssetOutput in this.m_PlayableDirector.playableAsset.outputs)
        {
            if (playableAssetOutput.streamName != trackName) continue;
            
            this.m_PlayableDirector.SetGenericBinding(playableAssetOutput.sourceObject, val);
            break;
        }
    }

    public void Play()
        => this.m_PlayableDirector.Play();

    #endregion Methods

}
