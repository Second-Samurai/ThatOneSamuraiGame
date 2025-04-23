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

    public Transform CenterTrackingPoint;
    public Vector3 m_TrackingBounds;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        GameValidator.NotNull(this.EnemyObserver, nameof(EnemyObserver));
        
        this.EnemyObserver.OnEnemyDeath.AddListener(this.RemoveEnemyTrackingWithinScene);
        this.CollectAllEnemiesWithinScene();
    }

    #endregion Unity Methods

    #region - - - - - - Methods - - - - - -

    private void CollectAllEnemiesWithinScene()
    {
        GameObject[] _EnemiesWithinScene = GameObject.FindGameObjectsWithTag(GameTag.Enemy);
        for (int i = 0; i < _EnemiesWithinScene.Length; i++)
            if (_EnemiesWithinScene[i].scene == this.gameObject.scene)
                this.SceneEnemyObjects.Add(_EnemiesWithinScene[i]);
    }

    private void RemoveEnemyTrackingWithinScene(GameObject enemy) 
        => this.SceneEnemyObjects.Remove(enemy);

    #endregion Methods
  
    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmosSelected()
    {
        if (this.CenterTrackingPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawCube(this.CenterTrackingPoint.position, this.m_TrackingBounds);
    }

    #endregion Gizmos
  
}
