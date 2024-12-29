using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Responsible for finisher cutscene camera movements.
/// </summary>
public class PlayerFinisherCutsceneDirector : PlayableDirector
{

    #region - - - - - - Methods - - - - - -

    public void BindToTrack(string trackName, Object val)
    {
        foreach (var playableAssetOutput in this.playableAsset.outputs)
        {
            if (playableAssetOutput.streamName != trackName) continue;
            
            this.SetGenericBinding(playableAssetOutput.sourceObject, val);
            break;
        }
    }

    #endregion Methods
  
}
