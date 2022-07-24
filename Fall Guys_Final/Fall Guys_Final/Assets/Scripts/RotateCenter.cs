using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCenter : MonoBehaviour
{
    public float rotatingSpeed = 130;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotatingSpeed * Time.deltaTime);

    }
}
