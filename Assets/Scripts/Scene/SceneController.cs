using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Scene
{

    public class SceneController : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField, RequiredField] private BoxCollider m_Collider;
        [SerializeField, RequiredField] private SceneEnemyController m_SceneEnemyController;
        
        #endregion Fields

        #region - - - - - - Unity Methods - - - - - -

        private void Start()
        {
            GameValidator.NotNull(this.m_Collider, nameof(m_Collider));
            GameValidator.NotNull(this.m_SceneEnemyController, nameof(m_SceneEnemyController));
        }

        #endregion Unity Methods
  
        #region - - - - - - Gizmos - - - - - -

        private void OnDrawGizmosSelected()
        {
            if (this.transform == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawCube(this.transform.position, this.m_Collider.bounds.size);
        }

        #endregion Gizmos

    }

}