using System;
using UnityEngine;
using Cinemachine;

namespace ThatOneSamuraiGame.Legacy
{

    [Obsolete]
    public class ThirdPersonCamController : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        public PlayerCamTargetController camTargetController;
        public CinemachineVirtualCamera sprintCam;

        private CinemachineVirtualCamera cam;

        #endregion Fields

        #region - - - - - - Methods - - - - - -

        public void Initialise()
        {
            if (!camTargetController)
            {
                Debug.LogWarning("Cam not set in inspector! Trying to find component in game manager");
                camTargetController = GameManager.instance.PlayerController.gameObject.transform.parent
                    .GetComponentInChildren<PlayerCamTargetController>();
            }

            cam = this.GetComponent<CinemachineVirtualCamera>();
            if (!cam.m_Follow)
            {
                Debug.LogWarning("Follow Target not set in inspector! Setting via code");
                cam.m_Follow = camTargetController.gameObject.transform;
            }
        }

        // CAMERA CONTROL BEHAVIOUR
        public void SetPriority(int var)
            => cam.m_Priority = var;

        // public void SetSensitivity(float sensitivity)
        //     => camTargetController.SetSensitivity(sensitivity);

        // SPRINT BEHAVIOUR
        public void SprintOn()
            => sprintCam.m_Priority = 15;

        // SPRINT BEHAVIOUR
        public void SprintOff()
            => sprintCam.m_Priority = 5;

        #endregion Methods

    }


}