public interface IRigLayerControl
{
    
    #region - - - - - - Properties - - - - - -

    bool IsActive { get; set; }

    #endregion Properties
  
    #region - - - - - - Methods - - - - - -

    void SetDefaultRigValues();

    #endregion Methods
    
}
