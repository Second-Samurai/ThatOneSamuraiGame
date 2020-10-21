using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPiece : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider col;
    public bool destroyed = false;
    public ParticleSystem particles;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        particles = GetComponentInChildren<ParticleSystem>();
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
