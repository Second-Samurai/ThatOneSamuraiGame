public interface ICameraState
{

    #region - - - - - - Methods - - - - - -

    void InitializeState(CameraStateContext context);

    void StartState();

    void EndState();

    bool ValidateState();

    #endregion Methods

}
