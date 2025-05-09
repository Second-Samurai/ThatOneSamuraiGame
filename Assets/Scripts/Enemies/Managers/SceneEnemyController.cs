using System.Collections.Generic;
using System.Linq;
using ThatOneSamuraiGame;
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
        // this.m_Observer.OnSceneActive.AddListener(() => this.StartCoroutine(this.ActivateSceneEnemyController()));
        
        // TODO: The placement of this is unusual
        EnemyManager.Instance.SetActiveSceneEnemyController(this);
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    // private IEnumerator ActivateSceneEnemyController()
    // {
    //     EnemyManager.Instance.SetActiveSceneEnemyController(this);
    //     for (int i = 0; i < this.SceneEnemyObjects.Count; i++)
    //     {
    //         this.SceneEnemyObjects[i].SetActive(true);
    //         ICommand _SetupCommand = this.SceneEnemyObjects[i].GetComponent<ICommand>();
    //         _SetupCommand.Execute();
    //         yield return null;
    //     }
    // }
    
    public void CollectAllEnemiesWithinScene()
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
