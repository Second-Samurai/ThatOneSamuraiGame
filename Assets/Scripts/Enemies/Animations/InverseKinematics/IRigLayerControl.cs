﻿public interface IRigLayerControl
{
    
    #region - - - - - - Properties - - - - - -

    bool IsActive { get; set; }

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    void DisableLayer();

    #endregion Methods

}
