using System.Collections.Generic;
using UnityEngine;

namespace ThatOneSamuraiGame.Scripts.DebugScripts.DebugSceneInvokers
{

    public class DebugStartupInvokerQue : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        public List<GameObject> OrderedQue;

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        public void InvokeQueStartup()
        {
            foreach (GameObject _Item in this.OrderedQue)
            {
                IDebugStartupHandler _StartupHandler = _Item.GetComponent<IDebugStartupHandler>();
                _StartupHandler?.Handle();
            }
        }

        #endregion Methods

    }

}