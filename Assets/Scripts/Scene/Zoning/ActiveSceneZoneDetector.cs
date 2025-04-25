using ThatOneSamuraiGame;
using UnityEngine;

public class ActiveSceneZoneDetector : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField, RequiredField] private SceneObserver m_SceneObserver;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
        => GameValidator.NotNull(this.m_SceneObserver, nameof(m_SceneObserver));

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != GameTag.Player) return;
        
        this.m_SceneObserver.OnSceneActive.Invoke();
    }

    #endregion Unity Methods
  
}
