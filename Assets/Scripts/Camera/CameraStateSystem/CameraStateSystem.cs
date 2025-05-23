﻿using System.Collections.Generic;
using ThatOneSamuraiGame.GameLogging;
using ThatOneSamuraiGame.Scripts.Camera.CameraStateSystem;
using UnityEngine;

public class CameraStateSystem : MonoBehaviour, ICameraStateSystem
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private FollowPlayerCameraState m_FollowPlayerState;
    [SerializeField] private FollowSprintPlayerCameraState m_FollowSprintPlayerState;
    [SerializeField] private FreeLookCameraState m_FreeLookState;
    [SerializeField] private LockOnCameraState m_LockOnState;

    private ICameraState m_CurrentCameraState;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    public void Initialize()
    {
        CameraStateContext _Context = new()
        {
            CameraController = this.GetComponent<ICameraController>()
        };
        
        this.m_FollowPlayerState.InitializeState(_Context);
        this.m_FollowSprintPlayerState.InitializeState(_Context);
        this.m_FreeLookState.InitializeState(_Context);
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -


    public void SetState(SceneCameras selectedCamera)
    {
        // A check is performed as the swordsman implementation might cause a stackoverflow,
        //  by running this operation multiple times upon its death.
        if (this.m_CurrentCameraState != null && this.m_CurrentCameraState.GetSceneState() == selectedCamera) return;
        
        this.m_CurrentCameraState?.EndState();
        
        if(selectedCamera == SceneCameras.FollowPlayer)
            this.m_CurrentCameraState = this.m_FollowPlayerState.GetComponent<ICameraState>();
        else if (selectedCamera == SceneCameras.FollowSprintPlayer)
            this.m_CurrentCameraState = this.m_FollowSprintPlayerState.GetComponent<ICameraState>();
        else if (selectedCamera == SceneCameras.FreeLook)
            this.m_CurrentCameraState = this.m_FreeLookState.GetComponent<ICameraState>();
        else if (selectedCamera == SceneCameras.LockOn)
            this.m_CurrentCameraState = this.m_LockOnState.GetComponent<ICameraState>();
        else 
            GameLogger.LogError($"No valid camera state was found. Instead received '{selectedCamera}'");
        
        this.m_CurrentCameraState?.StartState();
    }

    public List<ICameraState> GetCameraStates()
        => new()
        {
            this.m_FollowPlayerState,
            this.m_FollowSprintPlayerState,
            this.m_FreeLookState,
            this.m_LockOnState
        };

    #endregion Methods

}

public class CameraStateContext
{

    #region - - - - - - Properties - - - - - -

    public ICameraController CameraController { get; set; }
    
    #endregion Properties
  
}
