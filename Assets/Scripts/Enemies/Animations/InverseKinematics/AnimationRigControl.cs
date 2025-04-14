using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;

public class AnimationRigControl : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public List<RigLayerToggle> RigWeightLayers;

    #endregion Fields

    #region - - - - - - Methods - - - - - -

    public void ActivateAllLayers()
    {
        foreach(IRigLayerControl _RigLayerControl in this.RigWeightLayers)
            _RigLayerControl.IsActive = true;
    }

    // Todo: Implement exit behaviour when the player is too far away.
    public void DeActivateAllLayers()
    {
        foreach (IRigLayerControl _RigLayerControl in this.RigWeightLayers)
            _RigLayerControl.IsActive = false;
    }

    #endregion Methods
  
}
