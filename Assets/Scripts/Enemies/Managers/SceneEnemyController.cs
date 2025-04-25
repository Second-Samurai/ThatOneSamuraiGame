using System.Collections.Generic;
using System.Linq;
using ThatOneSamuraiGame;
using ThatOneSamuraiGame.Scripts.General.Services;
using UnityEngine;

public class SceneEnemyController : MonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    // TODO: Change to use getter-only properties
    // TODO: Should be managed by the scene area level
    public EnemyControlObserver EnemyObserver;
    public List<GameObject> SceneEnemyObjects;
    [SerializeField, RequiredField] private SceneObserver m_Observer;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        GameValidator.NotNull(this.EnemyObserver, nameof(EnemyObserver));
        GameValidator.NotNull(this.m_Observer, nameof(m_Observer));
        
        this.EnemyObserver.OnEnemyDeath.AddListener(this.RemoveEnemyTrackingWithinScene);
        this.m_Observer.OnSceneActive.AddListener(this.ActivateSceneEnemyController);
        
        this.CollectAllEnemiesWithinScene();
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    private void ActivateSceneEnemyController()
    {
        EnemyManager.Instance.SetActiveSceneEnemyController(this);
        for (int i = 0; i < this.SceneEnemyObjects.Count; i++)
        {
            ICommand _SetupCommand = this.SceneEnemyObjects[i].GetComponent<ICommand>();
            _SetupCommand.Execute();
        }
    }
    
    private void CollectAllEnemiesWithinScene()
    {
        List<GameObject> _EnemiesWithinScene = 
            FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(g => g.tag == GameTag.Enemy)
                .ToList();
        
        for (int i = 0; i < _EnemiesWithinScene.Count; i++)
            if (_EnemiesWithinScene[i].scene == this.gameObject.scene)
                this.SceneEnemyObjects.Add(_EnemiesWithinScene[i]);
    }
    
    private void RemoveEnemyTrackingWithinScene(GameObject enemy) 
        => this.SceneEnemyObjects.Remove(enemy);

    #endregion Methods

}
