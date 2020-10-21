using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugTP : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            player.position = transform.position;
        }
    }
}
