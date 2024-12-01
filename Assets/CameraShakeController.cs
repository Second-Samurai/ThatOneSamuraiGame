using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[Obsolete]
public class CameraShakeController : MonoBehaviour
{
    CinemachineBrain mainCamera;
    public CinemachineFreeLook cam;
    public CinemachineBasicMultiChannelPerlin noise;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<CinemachineBrain>();
    }

    public void ShakeCamera(float amount)
    {
        cam = mainCamera.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineFreeLook>();
        //noise = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        StopAllCoroutines();
        if(noise != null) 
            StartCoroutine(CameraShakeCR(amount));
    }

    IEnumerator CameraShakeCR(float amount)
    {
        noise.m_AmplitudeGain = amount;
        while(noise.m_AmplitudeGain > 0)
        {
            noise.m_AmplitudeGain -= Time.deltaTime;
            if (noise.m_AmplitudeGain < 0) 
                noise.m_AmplitudeGain = 0;
            yield return null;
        }
    }
}
