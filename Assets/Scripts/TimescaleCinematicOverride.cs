using UnityEngine;

public class TimescaleCinematicOverride : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public float timeScale = 1f;
    public bool isOverriding;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    void Update()
    {
        if (isOverriding) Time.timeScale = timeScale;
    }

    #endregion Unity Methods
  
}
