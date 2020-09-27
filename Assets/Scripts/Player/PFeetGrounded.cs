using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PFeetGrounded : MonoBehaviour
{
    [Header("Player Components")]
    public Transform footBaseTransform;
    public Rigidbody playerRb;

    [Header("Motion Modifiers")]
    public float gravity = 20;
    public float stepDown = 0.5f;

    [HideInInspector] public bool canGround = true;

    private Vector3 rootVelocity;
    private Vector3 _groundPosition = Vector3.zero;
    private RaycastHit _rayHit;

    // Start is called before the first frame update
    //
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        RunGroundCheck();
    }

    /// <summary>
    /// Performs ground check methods
    /// </summary>
    public void RunGroundCheck()
    {
        if (!CheckIsGrounded())
        {
            LockToGround();
            return;
        }

        StepDown();
    }

    // Summary: Raycasts to ground returning false when in air
    //
    private bool CheckIsGrounded()
    {
        //Debug.DrawRay(footBaseTransform.position, -Vector3.up, Color.cyan);
        if (Physics.Raycast(footBaseTransform.position, -Vector3.up, out _rayHit, 3f))
        {
            _groundPosition = _rayHit.point;
            return false;
        }

        _groundPosition = Vector3.zero;
        return true;
    }

    // Summary: During runtime the movement to forced down when encountering slopes.
    //
    private void StepDown()
    {
        rootVelocity = playerRb.velocity + Vector3.down * stepDown;
        playerRb.velocity = rootVelocity;
    }

    // Summary: During motion apply gravity to player until they reach the ground
    //
    private void LockToGround()
    {
        rootVelocity = playerRb.velocity +  Vector3.down * gravity * Time.fixedDeltaTime;
        playerRb.velocity = rootVelocity;
    }
}
