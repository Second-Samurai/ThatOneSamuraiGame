using System;
using UnityEngine;

[Serializable]
public class SceneSetupContext
{

    #region - - - - - - Fields - - - - - -

    [RequiredField] public GameObject CameraControlObject;
    [RequiredField] public GameObject LockOnControlObject;

    #endregion Fields

}
