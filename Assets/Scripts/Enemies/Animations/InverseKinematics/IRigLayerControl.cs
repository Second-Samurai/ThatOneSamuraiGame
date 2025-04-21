public interface IRigLayerControl
{
    
    #region - - - - - - Properties - - - - - -

    bool IsActive { get; set; }

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    void SetDefaultRigValues(); // TODO: Check whether we need this

    void DisableLayer();

    #endregion Methods

}
