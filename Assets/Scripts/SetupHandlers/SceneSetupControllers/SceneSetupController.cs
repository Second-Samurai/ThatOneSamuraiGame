using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers.SetupHandlers;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.SetupHandlers.SceneSetupControllers
{

    public class SceneSetupController : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        [SerializeField] private List<ISetupHandler> m_SetupHandlers;

        #endregion Fields

    }

}