﻿using System;
using UnityEngine;

namespace ThatOneSamuraiGame.Legacy
{

    [Obsolete]

    public class UIFaceCamera : MonoBehaviour
    {
        Camera _camera;

        // Start is called before the first frame update
        void Start()
        {
            _camera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward,
                _camera.transform.rotation * Vector3.up);
        }
    }


}