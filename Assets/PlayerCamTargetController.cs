using UnityEngine;

public class PlayerCamTargetController : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public GameObject player;
    public Vector3 posOffset;
    public float rotationSpeed = 1f;
    float mouseX, mouseY;
    //public bool invertY = false;

    #endregion Fields
  
    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        if (!player) 
            player = GameManager.instance.PlayerController.gameObject;
    }

    private void LateUpdate()
    {
        Debug.Log(transform);
        Debug.Log(player);
        transform.position = player.transform.position + posOffset;
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void RotateCam(Vector2 mouseInput)
    {
        //mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        //mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseX += mouseInput.x * rotationSpeed;
        mouseY += mouseInput.y * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);
        //transform.LookAt(target);
        // target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        //if (invertY) mouseY = -mouseY;
        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
    }

    public void SetSensitivity(float sensitivity)
    {
        rotationSpeed = sensitivity;
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    #endregion Methods

}
