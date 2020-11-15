using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIGuardMeter : MonoBehaviour
{
    public Slider guardSlider;
    public Canvas finisherKey;

    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public RectTransform parentCanvasRect;

    private Transform _entityTransform;
    private StatHandler _statHandler;
    private RectTransform _guardTransform;

    private Vector3 _entityDir;
    private Vector3 _cameraForward;
    private Vector3 _entityPosition;
    private Vector2 _screenPosition;

    private bool _canStayOff = true;
    private float _difference;
    private float _scaledXPos;
    private float _scaledYPos;
    private float _playerToEntityDist;

    private LockOnTracker _lockOnTracker;

    // Start is called before the first frame update
    public void Init(Transform entityTransform, StatHandler statHandler, Camera camera, RectTransform parentTransform)
    {
        _lockOnTracker = GameManager.instance.lockOnTracker;
        
        this._entityTransform = entityTransform;
        this._statHandler = statHandler;

        this.parentCanvasRect = parentTransform;
        this.mainCamera = camera;

        _guardTransform = this.GetComponent<RectTransform>();
        guardSlider.maxValue = _statHandler.maxGuard;
        guardSlider.minValue = 0;
        guardSlider.value = 0;
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.rewindManager.isTravelling)
        {
            DestroyWhenEntityDead();

            if (!CheckInCameraView() || _entityTransform != _lockOnTracker.targetEnemy)
            {
                if (guardSlider.gameObject.activeInHierarchy)
                {
                    HideFinisherKey();
                    guardSlider.gameObject.SetActive(false);
                }
                return;
            }
            else
            {
                if (!guardSlider.gameObject.activeInHierarchy && _entityTransform == _lockOnTracker.targetEnemy)
                {
                    guardSlider.gameObject.SetActive(true);
                    if (guardSlider.value == guardSlider.maxValue)
                    {
                        ShowFinisherKey();
                    }
                }
            }
            SetMeterPosition();
        }
    }

    private void Update()
    {
        if (GameManager.instance.rewindManager.isTravelling)
        {
            DestroyWhenEntityDead();

            if (!CheckInCameraView() || _entityTransform != _lockOnTracker.targetEnemy)
            {
                if (guardSlider.gameObject.activeInHierarchy)
                {
                    HideFinisherKey();
                    guardSlider.gameObject.SetActive(false);
                }
                return;
            }
            else
            {
                if (!guardSlider.gameObject.activeInHierarchy && _entityTransform == _lockOnTracker.targetEnemy)
                {
                    guardSlider.gameObject.SetActive(true);
                    if (guardSlider.value == guardSlider.maxValue)
                    {
                        ShowFinisherKey();
                    }
                }
            }
            SetMeterPosition();
        }
    }


    //Summary: Updates guide meter when called through event.
    //
    public void UpdateGuideMeter()
    {
        guardSlider.maxValue = _statHandler.maxGuard;
        //Finds difference between values
        _difference = _statHandler.maxGuard - _statHandler.CurrentGuard;
        guardSlider.value = _difference;
        _canStayOff = false;
    }

    //Summary: Checks if the entity position is ahead of the camera and within distance
    //
    public bool CheckInCameraView()
    {
        if (_canStayOff) return false;

        _playerToEntityDist = Vector3.Distance(GameManager.instance.playerController.transform.position, _entityTransform.position);
        _entityDir = (_entityTransform.position - mainCamera.transform.position).normalized;
        _cameraForward = mainCamera.transform.forward.normalized;

        //Checks if distance between camera and entity goes beyond threshold
        if (_playerToEntityDist >= 20)
        {
            if (guardSlider.value == 0)
            {
                _canStayOff = true;
            }

            //Debug.Log(">> GuideMeter: is disabled");
            return false;
        }

        //Checks if the dot product is pointing behind camera
        if (Vector3.Dot(_cameraForward, _entityDir) < 0)
        {
            //Debug.Log(">> GuideMeter: is disabled");
            return false;
        }

        return true;
    }

    //Summary: This updates the position of the guide meter in UI Canvas
    //
    public void SetMeterPosition()
    {
        _entityPosition = _entityTransform.position;
        _entityPosition.y += 3.5f;

        _screenPosition = RectTransformUtility.WorldToScreenPoint(mainCamera, _entityPosition);
        _scaledXPos = parentCanvasRect.rect.width * (_screenPosition.x / Screen.width) * 1;
        _scaledYPos = parentCanvasRect.rect.height * (_screenPosition.y / Screen.height) * 1;

        _screenPosition = new Vector2(_scaledXPos, _scaledYPos);
        _guardTransform.anchoredPosition = _screenPosition;
    }

    public void ShowFinisherKey()
    {
        if (guardSlider.gameObject.activeInHierarchy && _entityTransform == _lockOnTracker.targetEnemy)
        {
            finisherKey.enabled = true;
        }
    }

    public void HideFinisherKey()
    {
        finisherKey.enabled = false;
    }

    // Summary: Destroys UI when the enemy is either missing or dead
    //
    private void DestroyWhenEntityDead()
    {
        if (_entityTransform == null)
        {
            Destroy(gameObject);
        }
    }
}
