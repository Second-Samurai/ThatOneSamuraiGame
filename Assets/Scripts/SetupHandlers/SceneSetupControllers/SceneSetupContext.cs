using System;
using UnityEngine;

[Serializable]
public class SceneSetupContext
{

    #region - - - - - - Fields - - - - - -

    [Header("Camera Dependencies")]
    [RequiredField] public CameraController CameraController;
    
    [Header("LockOn Dependencies")]
    [RequiredField] public LockOnSystem LockOnSystem;
    [RequiredField] public LockOnObserver LockOnObserver;

    #endregion Fields

}
