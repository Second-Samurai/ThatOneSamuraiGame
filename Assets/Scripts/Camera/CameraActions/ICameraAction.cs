/// <summary>
/// Interface for specific actions that modify the Camera's values. These are independent from the states.
/// </summary>
/// <remarks>Can be used on any state but are effective through the FreeLookState.</remarks>
public interface ICameraAction
{

    #region - - - - - - Methods - - - - - -

    void StartAction();

    void UpdateAction();
    
    void EndAction();

    #endregion Methods

}
