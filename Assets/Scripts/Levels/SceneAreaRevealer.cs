using UnityEngine;

public class SceneAreaRevealer : MonoBehaviour
{
    public GameObject affectedArea;
    public Transform playerTransform;

    private bool isViewable = false;

    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = GameManager.instance.PlayerController.transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isViewable)
        {
            affectedArea.SetActive(true);
        }
        else
        {
            affectedArea.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            isViewable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>() != null)
        {
            isViewable = false;
        }
    }
}
