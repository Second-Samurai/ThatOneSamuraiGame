using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArmourPiece : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider col;
    public bool destroyed = false;
    public ParticleSystem particles;
    public Vector3 originPos;
    public quaternion originRot;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        particles = GetComponentInChildren<ParticleSystem>();
        originPos = transform.localPosition;
        originRot = transform.localRotation;
    }

    public void DropPiece()
    {
        rb.isKinematic = false;
        col.enabled = true;
        transform.parent = null;
        destroyed = true;
        particles.Play();
    }

}
