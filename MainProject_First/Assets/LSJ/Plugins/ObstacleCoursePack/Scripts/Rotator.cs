using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float speed = 3f;


    // Update is called once per frame
    void Update()
    {
		transform.Rotate(0f, 0f, speed * Time.deltaTime / 0.01f, Space.Self);
	}
}
