using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LDoorWays : MonoBehaviour
{
    public Transform doorPivot;

    [Space]
    public float rotationSpeed = 1f;
    public float openAngle;
    public float closedAngle;

    [Header("Switches")]
    public bool isOpeningDoor;
    public bool isClosingDoor;

    private Vector3 currentRotation;
    private float completionAngle;

    private bool isPerformingAction = false;
    private bool inReverse;

    public BossThemeManager bossTheme;

    private BackgroundAudio _backgroundAudio;
    private bool isplaying = true;
    private bool played = false;

    void Awake()
    {
        if (isOpeningDoor)
        {
            completionAngle = openAngle > 0 ? openAngle : openAngle + 360;
            inReverse = rotationSpeed > 0 ? false : true;
        }
        else
        {
            completionAngle = closedAngle > 0 ? closedAngle : closedAngle + 360;
            inReverse = rotationSpeed > 0 ? false : true;
        }
    }

    private void Start()
    {
        bossTheme = GameManager.instance.audioManager.BossThemeManager;
        _backgroundAudio = GameManager.instance.audioManager.backgroundAudio;
    }

    void FixedUpdate()
    {
        if (isOpeningDoor && isPerformingAction)
        {
            OpenDoor();
        }
        else if (isClosingDoor && isPerformingAction)
        {
            CloseDoor();
        }
    }

    // Summary: Rotates the door on its pivot
    //
    private void OpenDoor()
    {
        currentRotation = doorPivot.rotation.eulerAngles;
        //currentRotation.y = Mathf.Lerp(currentRotation.y, openAngle, rotationDelta);
        //doorPivot.rotation = Quaternion.Euler(currentRotation);

        if (doorPivot.rotation.eulerAngles.y == completionAngle)
        {
            isPerformingAction = false;
        }
    }

    // Summary: Closes the door on its pivot
    //s
    private void CloseDoor()
    {

        if (isplaying == true && played == false) 
        {
            _backgroundAudio.PlayClose();
            isplaying = false;
        }
        if (_backgroundAudio.doorSource.isPlaying == false && isplaying == false && played == false) 
        {

            _backgroundAudio.PlaySlam();
            played = true;

        }

        bossTheme.gameObject.SetActive(true);
        currentRotation = doorPivot.rotation.eulerAngles;
        doorPivot.Rotate(0, rotationSpeed * Time.fixedDeltaTime, 0);

        //catches reverse angles
        if (inReverse)
        {
            if (doorPivot.rotation.eulerAngles.y <= completionAngle)
            {
                doorPivot.rotation = Quaternion.Euler(0, completionAngle, 0);
                isPerformingAction = false;
                return;
            }
        } else
        {
            if (doorPivot.rotation.eulerAngles.y >= completionAngle)
            {
                doorPivot.rotation = Quaternion.Euler(0, completionAngle, 0);
                isPerformingAction = false;
                return;
            }
        }
        
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            isPerformingAction = true;
        }
    }
}
