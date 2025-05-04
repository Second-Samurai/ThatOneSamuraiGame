using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.Enumeration;
using ThatOneSamuraiGame.Scripts.Scene.Loaders;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = ThatOneSamuraiGame.Scripts.Scene.SceneManager.SceneManager;

public class LoadSceneTriggerVolume : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    [SerializeField] private GameSceneEnum m_LoadGameScene;

    private ISceneLoader m_SceneLoader;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_SceneLoader = SceneManager.Instance.SceneLoader;

        GameValidator.NotNull(this.m_SceneLoader, nameof(m_SceneLoader));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != GameTag.Player) return;
        
        this.m_SceneLoader.LoadScene(this.m_LoadGameScene, LoadSceneMode.Additive);
    }

    #endregion Unity Methods
  
}
