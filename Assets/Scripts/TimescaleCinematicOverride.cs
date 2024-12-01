using UnityEngine;

public class TimescaleCinematicOverride : MonoBehaviour
{
    public float timeScale = 1f;
    public bool isOverriding = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOverriding) Time.timeScale = timeScale;
    }
}
