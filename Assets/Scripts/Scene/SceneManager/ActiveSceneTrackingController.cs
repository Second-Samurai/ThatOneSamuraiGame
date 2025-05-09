using System.Collections.Generic;
using ThatOneSamuraiGame.Scripts.Base;
using ThatOneSamuraiGame.Scripts.Scene.Loaders;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;

public class ActiveSceneTrackingController : PausableMonoBehaviour
{

    #region - - - - - - Fields - - - - - -

    public List<SceneController> m_TrackedSceneControllers;
    public Transform m_PlayerTransform;
    private ISceneLoader m_SceneLoader;

    public float m_ActiveRadius;
    public float m_DeActivationRadius;
    public float m_TrackingRadius;

    public bool m_IsGizmosInWireframe;

    #endregion Fields

    #region - - - - - - Unity Methods - - - - - -

    private void Start()
    {
        this.m_SceneLoader = SceneManager.Instance.SceneLoader;

        GameValidator.NotNull(this.m_PlayerTransform, nameof(m_PlayerTransform));
        GameValidator.NotNull(this.m_SceneLoader, nameof(m_SceneLoader));

        SceneManager.Instance.ActiveSceneTracker = this;
    }

    private void Update()
    {
        if (this.IsPaused) return;

        for (int i = 0; i < this.m_TrackedSceneControllers.Count; i++)
        {
            SceneController _TrackedSceneController = this.m_TrackedSceneControllers[i];
            Vector3 _SceneCenter = _TrackedSceneController.CenterPosition;
            
            if (this.IsWithinTrackingRadius(_SceneCenter))
            {
                if (this.IsWithinActivationRadius(_SceneCenter) && !_TrackedSceneController.IsSceneActive)
                    _TrackedSceneController.ActivateScene();
                else if (this.IsWithinDeActivationRadius(_SceneCenter) && _TrackedSceneController.IsSceneActive)
                    _TrackedSceneController.DeactivateScene();
            }
            else
            {
                this.m_TrackedSceneControllers.Remove(_TrackedSceneController);
                _TrackedSceneController.CloseScene();
            }
        }
    }

    #endregion Unity Methods
  
    #region - - - - - - Methods - - - - - -

    public void RegisterScene(SceneController sceneController)
        => this.m_TrackedSceneControllers.Add(sceneController);

    private bool IsWithinTrackingRadius(Vector3 sceneCenter) =>
        (this.m_PlayerTransform.position - sceneCenter).sqrMagnitude <
        this.m_TrackingRadius * this.m_TrackingRadius;

    private bool IsWithinActivationRadius(Vector3 sceneCenter) 
        => (this.m_PlayerTransform.position - sceneCenter).sqrMagnitude < this.m_ActiveRadius * this.m_ActiveRadius;

    private bool IsWithinDeActivationRadius(Vector3 sceneCenter)
        => (this.m_PlayerTransform.position - sceneCenter).sqrMagnitude < this.m_DeActivationRadius * this.m_ActiveRadius;

    #endregion Methods

    #region - - - - - - Gizmos - - - - - -

    private void OnDrawGizmosSelected()
    {
        if (this.m_PlayerTransform == null) return;

        Gizmos.color = Color.green;
        if (this.m_IsGizmosInWireframe)
            Gizmos.DrawWireSphere(this.m_PlayerTransform.position, this.m_ActiveRadius);
        else
            Gizmos.DrawSphere(this.m_PlayerTransform.position, this.m_ActiveRadius);

        Gizmos.color = Color.yellow;
        if (this.m_IsGizmosInWireframe)
            Gizmos.DrawWireSphere(this.m_PlayerTransform.position, this.m_DeActivationRadius);
        else
            Gizmos.DrawSphere(this.m_PlayerTransform.position, this.m_DeActivationRadius);

        Gizmos.color = Color.red;
        if (this.m_IsGizmosInWireframe)
            Gizmos.DrawWireSphere(this.m_PlayerTransform.position, this.m_TrackingRadius);
        else
            Gizmos.DrawSphere(this.m_PlayerTransform.position, this.m_TrackingRadius);

        Gizmos.color = Color.blue;
        foreach (SceneController _TrackedSceneController in this.m_TrackedSceneControllers)
            Gizmos.DrawSphere(_TrackedSceneController.transform.position, 5f);
    }

    #endregion Gizmos
  
}
