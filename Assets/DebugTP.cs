﻿using UnityEngine;
using UnityEngine.InputSystem;

public class DebugTP : MonoBehaviour
{
    public Transform player, village;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame && Keyboard.current.shiftKey.IsPressed())
        {
            player.position = transform.position;
            AudioManager.instance.backgroundAudio.FadeScore();
        }

        if (Keyboard.current.oKey.wasPressedThisFrame && Keyboard.current.shiftKey.IsPressed())
        {
            player.position = village.position;
        }
        if (Keyboard.current.lKey.wasPressedThisFrame && Keyboard.current.shiftKey.IsPressed())
        {
            player.position = village.position;
        }

    }
}
