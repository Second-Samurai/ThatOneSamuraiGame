using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public static class RigPrewarmer
{

    #region - - - - - - Methods - - - - - -

    public static IEnumerator Prewarm(GameObject rigRoot)
    {
        if (rigRoot == null)
            yield break;

        bool _WasActive = rigRoot.activeSelf;

        if (!_WasActive)
            rigRoot.SetActive(true);

        yield return null;

        var _RigBuilder = rigRoot.GetComponent<RigBuilder>();
        if (_RigBuilder != null)
            _RigBuilder.Build();

        yield return null;

        if (!_WasActive)
            rigRoot.SetActive(false);
    }

    #endregion Methods
  
  
}
