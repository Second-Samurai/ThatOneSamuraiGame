using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKPuppet : MonoBehaviour
{
    Animator animator;
    public Transform rHand, lHand;
    public float IKWeight = 1f, IKTargetWeight = 1f;

    public bool IKOn = false, bCanIK = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //if (IKOn)
        //{
            animator.SetIKPosition(AvatarIKGoal.RightHand, rHand.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rHand.rotation);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, IKWeight);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, lHand.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKWeight);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, lHand.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, IKWeight);
        //}
    }

    public void EnableIK()
    {
        if (bCanIK)
        {
            IKOn = true;
            IKTargetWeight = .8f;
        }
    }
    public void DisableIK() 
    {
        IKOn = false;
        IKTargetWeight = 0f;
    }

    private void Update()
    {
        if (IKOn)
        {
            if (IKWeight < IKTargetWeight)
                IKWeight += Time.deltaTime*3;
            if (IKWeight > IKTargetWeight)
                IKWeight = IKTargetWeight;
        }
        else
        {
            if (IKWeight > IKTargetWeight)
                IKWeight -= Time.deltaTime*2;
            if (IKWeight < IKTargetWeight)
                IKWeight = IKTargetWeight;
        }
    }


}
