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
        currentRotation = doorPivot.rotation.eulerAngles;
        doorPivot.Rotate(0, rotationSpeed * Time.fixedDeltaTime, 0);

        Debug.Log(Quaternion.Euler(currentRotation).eulerAngles);

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
