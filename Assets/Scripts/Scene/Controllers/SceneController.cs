using ThatOneSamuraiGame.Scripts.Enumeration;
using ThatOneSamuraiGame.Scripts.Scene.Loaders;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public GameSceneEnum AssignedGameScene;
    [SerializeField, RequiredField] private BoxCollider m_Collider;
    [SerializeField, RequiredField] private SceneEnemyController m_SceneEnemyController;
    [SerializeField, RequiredField] private ObjectGroupEnablerSystem m_ObjectGroupEnabler;

    public Transform m_SceneCenter; // TODO: Clean up
    private ISceneLoader m_SceneLoader;
    private bool m_IsSceneActive;
    
    #endregion Fields

    #region - - - - - - Properties - - - - - -

    public Vector3 CenterPosition => this.m_SceneCenter.position;

    public bool IsSceneActive => this.m_IsSceneActive;

    #endregion Properties

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_SceneLoader = SceneManager.Instance.SceneLoader;
        
        GameValidator.NotNull(this.m_Collider, nameof(m_Collider), this.gameObject.name);
        GameValidator.NotNull(this.m_SceneEnemyController, nameof(m_SceneEnemyController), this.gameObject.name);
        GameValidator.NotNull(this.m_SceneLoader, nameof(m_SceneLoader), this.gameObject.name);

        ActiveSceneTrackingController _ActiveSceneTracker = SceneManager.Instance.ActiveSceneTracker;
        _ActiveSceneTracker.RegisterScene(this);
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    public void ActivateScene()
    {
        this.m_ObjectGroupEnabler.EnableObjects();
        this.m_IsSceneActive = true;
    }

    public void DeactivateScene()
    {
        this.m_ObjectGroupEnabler.DisableObjects();
        this.m_IsSceneActive = false;
    }

    public void CloseScene()
        => this.m_SceneLoader.UnloadScene(this.AssignedGameScene);
    
    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmosSelected()
    {
        if (this.transform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawCube(this.transform.position, this.m_Collider.bounds.size);
    }

    #endregion Gizmos

}
