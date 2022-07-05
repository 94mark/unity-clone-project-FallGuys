using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotPlate_SJ : MonoBehaviour
{
    public float rotSpeed = 100f;

    void Update()
    {
        transform.Rotate(new Vector3(0, rotSpeed * Time.deltaTime, 0));
    }
}
