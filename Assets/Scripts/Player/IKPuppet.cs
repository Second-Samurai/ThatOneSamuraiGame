using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPuppet : MonoBehaviour
{
    Animator animator;
    public Transform rHand, lHand;
    public float IKWeight = 1f;

    public bool IKOn = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (IKOn)
        {
            animator.SetIKPosition(AvatarIKGoal.RightHand, rHand.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);
        }
    }


}
