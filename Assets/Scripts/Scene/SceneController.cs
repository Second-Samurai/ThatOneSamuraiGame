using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Scene
{

    public class SceneController : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField, RequiredField] private BoxCollider Collider;
        
        #endregion Fields
        
        #region - - - - - - Gizmos - - - - - -

        private void OnDrawGizmosSelected()
        {
            if (this.transform == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawCube(this.transform.position, this.Collider.bounds.size);
        }

        #endregion Gizmos

    }

}