using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Enumeration;
using UnityEngine;
using SceneManager = ThatOneSamuraiGame.Scripts.Scene.SceneManager.SceneManager;

public class LoadSceneTriggerVolume : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    private ActiveSceneTrackingController m_SceneTracker;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -
    [SerializeField] private GameSceneEnum m_LoadGameScene;

    private void Start()
    {
        this.m_SceneTracker = SceneManager.Instance.ActiveSceneTracker;

        GameValidator.NotNull(this.m_SceneTracker, nameof(m_SceneTracker));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != GameTag.Player) return;
        
        this.m_SceneTracker.Loa
    }

    #endregion Unity Methods
  
}
