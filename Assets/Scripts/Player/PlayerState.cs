using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.Player
{
    
    public class PlayerState : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField]
        private GameObject m_AttackTarget;

        #endregion Fields
        
        #region - - - - - - Properties - - - - - -

        public GameObject AttackTarket
        {
            get { return this.m_AttackTarget; }
        }

        #endregion Properties
        
    }
    
}