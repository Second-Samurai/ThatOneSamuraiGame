using UnityEngine;

public class UnchildObjects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DetachChildren();
    }
     
}
