using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LTeleportationRings : MonoBehaviour
{
    public enum RingState
    {
        Idle,
        OnEnter,
        OnLeave
    };

    [Header("Rings")]
    public Transform outerRing;
    public Transform innerRing;
    public Transform teleportationPoint;

    [Header("Ring Starting Values")]
    public Vector3 startingPosition;
    public float startingXRotation;

    [Header("Ring End Values")]
    public float innerEndYPosition;
    public float innerEndXRotation;
    public float outerEndYPosition;
    public float outerEndXRotation;

    public RingState ringState;
    public bool canBeUsed = false;
    public bool isActive = false;
    private bool hasAnimated = false;

    private LTeleportationRings destinationRing;
    private UITeleportation teleportationUI;

    private float outerYPosition;
    private float innerYPosition;

    public bool canInnerRotate = true;
    public bool canOuterRotate = true;

    private float refY;
    private float refYY;

    // Start is called before the first frame update
    void Start()
    {
        //leportationUI = this.GetComponent<UITeleportation>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        OnActive();
        AnimateRingLevitation();
    }

    private void OnActive()
    {
        if (isActive)
        {
            transform.Rotate(0, 15, 0);
        }
    }

    private void AnimateRingLevitation()
    {
        if (ringState == RingState.OnEnter)
        {
            AnimateOn();
        } 
        else if(ringState == RingState.OnLeave)
        {
            AnimateClose();
        }
    }

    private void AnimateOn()
    {
        outerYPosition = 0;
        innerYPosition = 0;

        if (canOuterRotate)
        {
            outerRing.transform.Rotate(0.4f, 0, 0);
            if (outerRing.transform.eulerAngles.x >= (0 + outerEndXRotation))
            {
                outerRing.transform.rotation = Quaternion.Euler(new Vector3(0 + outerEndXRotation, outerRing.eulerAngles.y, outerRing.eulerAngles.z));
                canOuterRotate = false;
            }
        }

        outerYPosition = Mathf.SmoothDamp(outerRing.transform.localPosition.y, outerEndYPosition, ref refY, 0.7f);
        outerRing.localPosition = new Vector3(outerRing.localPosition.x, outerYPosition, outerRing.localPosition.z);

        if (canInnerRotate)
        {
            innerRing.transform.Rotate(-0.4f, 0, 0);
            if (innerRing.transform.eulerAngles.x <= (360 + innerEndXRotation))
            {
                innerRing.transform.rotation = Quaternion.Euler(new Vector3(360 + innerEndXRotation, innerRing.eulerAngles.y, innerRing.eulerAngles.z));
                canInnerRotate = false;
            }
        }

        innerYPosition = Mathf.SmoothDamp(innerRing.transform.localPosition.y, innerEndYPosition, ref refYY, 0.7f);
        innerRing.localPosition = new Vector3(innerRing.localPosition.x, innerYPosition, innerRing.localPosition.z);

        //Debug.Log(outerYPosition);
        //Debug.Log(Mathf.Ceil(innerYPosition));

        if (!canOuterRotate && !canInnerRotate && outerYPosition >= (outerEndYPosition-0.5f) && innerYPosition >= (innerEndYPosition - 0.5f))
        {
            Debug.Log("Is still running");
            ringState = RingState.Idle;
        }
    }

    private void AnimateClose()
    {
        outerYPosition = 0;
        innerYPosition = 0;

        if (canOuterRotate)
        {
            outerRing.transform.Rotate(-0.4f, 0, 0);
            if (outerRing.transform.eulerAngles.x <= 0)
            {
                outerRing.transform.rotation = Quaternion.Euler(new Vector3(0, outerRing.eulerAngles.y, outerRing.eulerAngles.z));
                canOuterRotate = false;
            }
        }

        outerYPosition = Mathf.SmoothDamp(outerRing.transform.localPosition.y, -1, ref refY, 0.7f);
        outerRing.localPosition = new Vector3(outerRing.localPosition.x, outerYPosition, outerRing.localPosition.z);

        if (canInnerRotate)
        {
            innerRing.transform.Rotate(0.4f, 0, 0);
            if (innerRing.transform.eulerAngles.x >= 360)
            {
                innerRing.transform.rotation = Quaternion.Euler(new Vector3(0, innerRing.eulerAngles.y, innerRing.eulerAngles.z));
                canInnerRotate = false;
            }
        }

        innerYPosition = Mathf.SmoothDamp(innerRing.transform.localPosition.y, -1, ref refYY, 0.7f);
        innerRing.localPosition = new Vector3(innerRing.localPosition.x, innerYPosition, innerRing.localPosition.z);

        if (Mathf.Floor(outerYPosition) == -1 && Mathf.Floor(innerYPosition) == -1)
        {
            ringState = RingState.Idle;
            canInnerRotate = false;
            canOuterRotate = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeUsed) return;

        //isPlayerDetected = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!canBeUsed) return;

        //isPlayerDetected = false;
    }
}

