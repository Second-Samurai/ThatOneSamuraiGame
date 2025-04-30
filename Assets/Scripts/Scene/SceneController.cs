using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Scene
{

    public class SceneController : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField, RequiredField] private BoxCollider m_Collider;
        [SerializeField, RequiredField] private SceneEnemyController m_SceneEnemyController;

        public Transform m_SceneCenter; // TODO: Clean up
        
        #endregion Fields

        #region - - - - - - Properties - - - - - -

        public Vector3 CenterPosition => this.m_SceneCenter.position;

        #endregion Properties
  
        #region - - - - - - Unity Methods - - - - - -

        private void Start()
        {
            GameValidator.NotNull(this.m_Collider, nameof(m_Collider));
            GameValidator.NotNull(this.m_SceneEnemyController, nameof(m_SceneEnemyController));
        }

        #endregion Unity Methods

        #region - - - - - - Methods - - - - - -

        public void ActivateScene()
        {
            
        }

        public void DeactivateScene()
        {
            
        }
        
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

}