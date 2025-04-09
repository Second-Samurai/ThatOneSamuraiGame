using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

public class IKTargetAssigner : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    private Transform m_AimTransform;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        GameObject _Player = SceneManager.Instance.SceneState.ActivePlayer;
        IKLookAtAimTargetProvider _IKTargetProvider = _Player.GetComponent<IKLookAtAimTargetProvider>();
        this.m_AimTransform = _IKTargetProvider.GetAimTarget();
    }

    private void Update()
    {
        if (this.IsPaused) return;

        this.transform.position = this.m_AimTransform.position;
    }

    #endregion Unity Methods
  
}
