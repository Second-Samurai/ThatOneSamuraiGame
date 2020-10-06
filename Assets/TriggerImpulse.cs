using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerImpulse : MonoBehaviour
{

    Cinemachine.CinemachineImpulseSource impulseSource;
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<Cinemachine.CinemachineImpulseSource>();
        cam = Camera.main;
    }

    public void FireImpulse()
    {
        impulseSource.GenerateImpulse(cam.transform.forward);
    }
}
