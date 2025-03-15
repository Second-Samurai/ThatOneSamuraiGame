using System;
using ThatOneSamuraiGame.Scripts.Scene.SceneManager;
using UnityEngine;
using UnityEngine.UI;

// TODO: Seperate into a seperate file
public interface IFloatingEnemyGuardMeter
{

    #region - - - - - - MyRegion - - - - - -

    void ShowFinisherKey();

    void HideFinisherKey();

    #endregion MyRegion

}

[Obsolete]
public class UIGuardMeter : MonoBehaviour, IFloatingEnemyGuardMeter
{
    public Slider guardSlider;
    public Canvas finisherKey;

    [HideInInspector] public Camera mainCamera;
    [HideInInspector] public RectTransform parentCanvasRect;

    private Transform _entityTransform;
    private StatHandler _statHandler;
    private RectTransform _guardTransform;
    private Transform m_TargetEnemyTransform;

    private Vector3 _entityDir;
    private Vector3 _cameraForward;
    private Vector3 _entityPosition;
    private Vector2 _screenPosition;

    private bool _canStayOff = true;
    private float _difference;
    private float _scaledXPos;
    private float _scaledYPos;
    private float _playerToEntityDist;

    // Start is called before the first frame update
    public void Init(Transform entityTransform, StatHandler statHandler, Camera camera, RectTransform parentTransform)
    {
        // Wrapping field assignment in closure to avoid boilerplate
        ILockOnObserver _LockOnObserver = SceneManager.Instance.LockOnObserver 
            ?? throw new ArgumentNullException(nameof(SceneManager.Instance.LockOnObserver));
        _LockOnObserver.OnNewLockOnTarget.AddListener(target => this.m_TargetEnemyTransform = target);
        
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
        //TODO - Ailin 14/10/24 refactor this out of update
        DestroyWhenEntityDead();

        if (!CheckInCameraView() || _entityTransform != this.m_TargetEnemyTransform)
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
            if (!guardSlider.gameObject.activeInHierarchy && _entityTransform == this.m_TargetEnemyTransform)
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

        _playerToEntityDist = Vector3.Distance(GameManager.instance.PlayerController.transform.position, _entityTransform.position);
        _entityDir = (_entityTransform.position - mainCamera.transform.position).normalized;
        _cameraForward = mainCamera.transform.forward.normalized;

        //Checks if distance between camera and entity goes beyond threshold
        if (_playerToEntityDist >= 20)
        {
            if (guardSlider.value == 0)
                _canStayOff = true;

            return false;
        }

        //Checks if the dot product is pointing behind camera
        if (Vector3.Dot(_cameraForward, _entityDir) < 0)
            return false;

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
        if (guardSlider.gameObject.activeInHierarchy && _entityTransform == this.m_TargetEnemyTransform)
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
