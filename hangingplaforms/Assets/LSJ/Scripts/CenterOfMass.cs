using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterOfMass : MonoBehaviour
{
    public Vector3 CenterOfMass2;
    public bool Awake;
    protected Rigidbody rb;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.centerOfMass = CenterOfMass2;
        rb.WakeUp();
        Awake = !rb.IsSleeping();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * CenterOfMass2, 0.5f);
    }
}
