using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Input Settings")]
public class InputSettings : ScriptableObject
{
    [Header("Mouse and Peripherals")]
    [Range(0.1f, 3f)]
    public float mouseSensitivity = 0;

}
