using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = Vector3.right * h + Vector3.forward * v;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    }
}
