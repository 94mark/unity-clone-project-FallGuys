using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceMouse : MonoBehaviour
{
    public float force = 500.0f;

    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown");

        Rigidbody rb;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force, ForceMode.Force);
        rb.useGravity = true;
    }
}

