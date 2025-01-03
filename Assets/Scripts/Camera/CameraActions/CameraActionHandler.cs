using UnityEngine;

public interface ICameraActionHandler
{

    #region - - - - - - Methods - - - - - -

    void SetCameraAction(ICameraAction cameraAction);

    void EndCameraAction();

    #endregion Methods

}

public class CameraActionHandler : MonoBehaviour, ICameraActionHandler
{

    #region - - - - - - Fields - - - - - -

    private ICameraAction m_CurrentCameraAction;

    #endregion Fields
    
    #region - - - - - - Methods - - - - - -

    void ICameraActionHandler.SetCameraAction(ICameraAction cameraAction)
    {
        this.m_CurrentCameraAction?.EndAction();
        this.m_CurrentCameraAction = cameraAction;
        this.m_CurrentCameraAction?.StartAction();
    }

    void ICameraActionHandler.EndCameraAction()
        => this.m_CurrentCameraAction?.EndAction();

    #endregion Methods

}
