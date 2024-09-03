using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAdditiveTimer : MonoBehaviour
{
    public float time = 0;

    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
    }
}
