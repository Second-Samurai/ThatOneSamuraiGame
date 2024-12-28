using System;
using System.Collections;
using UnityEngine;

public class CameraRollAction : ICameraAction
{

    #region - - - - - - Fields - - - - - -

    private readonly IFreelookCameraController m_FreeLookCamera;
    private readonly MonoBehaviour m_RefMonoBehaviour;

    #endregion Fields
  
    #region - - - - - - Constructors - - - - - -

    public CameraRollAction(IFreelookCameraController freeLookCamera, MonoBehaviour refMonoBehaviour)
    {
        this.m_FreeLookCamera = freeLookCamera ?? throw new ArgumentNullException(nameof(freeLookCamera));
        this.m_RefMonoBehaviour = refMonoBehaviour ?? throw new ArgumentNullException(nameof(refMonoBehaviour));
    }

    #endregion Constructors
  
    #region - - - - - - Methods - - - - - -

    public void StartAction()
    {
        this.m_RefMonoBehaviour.StopAllCoroutines();
        this.m_RefMonoBehaviour.StartCoroutine(this.RollCamera());
    }

    public void UpdateAction()
    {
    }

    public void EndAction()
    {
        this.m_RefMonoBehaviour.StopAllCoroutines();
        this.m_RefMonoBehaviour.StartCoroutine(this.ResetCameraRoll());
    }

    private IEnumerator RollCamera()
    {
        while (this.m_FreeLookCamera.DutchAngle > -10)
        {
            this.m_FreeLookCamera.DutchAngle -= Time.deltaTime * 3f;
            this.m_FreeLookCamera.FieldOfView -= Time.deltaTime * 3f;
            
            yield return null;
        }
    }

    private IEnumerator ResetCameraRoll()
    {
        while (this.m_FreeLookCamera.DutchAngle < 0)
        {
            this.m_FreeLookCamera.DutchAngle += Time.deltaTime * 30f;
            
            if (this.m_FreeLookCamera.FieldOfView < 40)
                this.m_FreeLookCamera.FieldOfView += Time.deltaTime * 100f;
            else 
                this.m_FreeLookCamera.FieldOfView = 60f;

            yield return null;
        }

        this.m_FreeLookCamera.DutchAngle = 0f;
    }

    #endregion Methods

}
