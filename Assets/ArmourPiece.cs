using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPiece : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider col;
    public bool destroyed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
    }

    public void DropPiece()
    {
        rb.isKinematic = false;
        col.enabled = true;
        transform.parent = null;
        destroyed = true;
    }

}
