using System;
using UnityEngine;
using Cinemachine;

namespace ThatOneSamuraiGame.Legacy
{

    [Obsolete]

    public class LockOnTargetManager : MonoBehaviour
    {

        #region - - - - - - Fields - - - - - -

        public float lookSpeed = .5f;
        public CinemachineFreeLook cam;
        public bool _bLockedOn = false;
        public GameObject targetHolder;
        public Transform _target, _player;
        public float swapSpeed = 10f;
        public FinisherCam finisherCam;

        CinemachineBrain mainCam;
        Animator animator;

        #endregion Fields


        #region - - - - - - Unity Lifecycle Methods - - - - - -

        // void Start()
        // {
        //     cam = GetComponent<CinemachineFreeLook>();
        //     mainCam = Camera.main.GetComponent<CinemachineBrain>();
        //     // animator = GameManager.instance.PlayerController.gameObject.GetComponent<Animator>();
        //     animator = this.transform.root.GetComponent<Animator>();
        // }

        private void FixedUpdate()
        {
            // Validate whether dependencies exist
            if (!cam || !mainCam || !animator) return;

            if (_bLockedOn)
                MoveTarget();
            if ((!_bLockedOn || !animator.GetBool("LockedOn")) && cam.Equals((object)mainCam.ActiveVirtualCamera))
            {
                cam.m_Priority = 3;
            }
        }

        #endregion Unity Lifecycle Methods

        #region - - - - - - Methods - - - - - -

        public void Initialise()
        {
            cam = GetComponent<CinemachineFreeLook>();
            mainCam = Camera.main.GetComponent<CinemachineBrain>();
            animator = this.transform.root.GetComponent<Animator>();
        }

        void MoveTarget()
        {
            //if (targetHolder.transform.position != _target.position)
            //    targetHolder.transform.Translate(_target.position - targetHolder.transform.position * swapSpeed * Time.deltaTime);

        }


        public void SetTarget(Transform target, Transform player)
        {

            // targetHolder.transform.position = target.position;
            cam.LookAt = target.transform;
            _bLockedOn = true;
            _target = target;
            _player = player;
        }

        public void ClearTarget()
        {
            _target = null;
            _player = null;
            _bLockedOn = false;
        }

        public void GuardBreakCam(Transform target)
        {
            finisherCam.TransitionToCamera(target);
        }

        public void EndGuardBreakCam()
        {
            finisherCam.LeaveCamera();
        }

        #endregion Methods

    }


}