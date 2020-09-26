using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamTargetController : MonoBehaviour
{
    public GameObject player;
    public Vector3 posOffset;
    public float rotationSpeed = 1f;
    float mouseX, mouseY; 
     
    private void LateUpdate()
    {
        transform.position = player.transform.position + posOffset;
        RotateCam();
    }

    public void RotateCam(Vector2 mouseInput)
    {
        //mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        //mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseX += mouseInput.x * rotationSpeed;
        mouseY += mouseInput.y * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);
        //transform.LookAt(target);
        // target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        transform.rotation = Quaternion.Euler(0, mouseX, 0);
    }
}
