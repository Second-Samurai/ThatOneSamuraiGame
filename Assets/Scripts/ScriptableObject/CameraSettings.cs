using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Camera Settings")]
public class CameraSettings : ScriptableObject
{

    #region - - - - - - Fields - - - - - -

    public VirtualCameraInfo SprintCameraInfo;
    public VirtualCameraInfo FollowCameraInfo;

    #endregion Fields

}

[Serializable]
public class VirtualCameraInfo
{

    #region - - - - - - Fields - - - - - -

    public Vector3 Damping;
    
    public Vector3 ShoulderOffset;
    
    [Range(0, 1)]
    public float CameraSide;

    public float CameraDistance;
    

    #endregion Fields

}
