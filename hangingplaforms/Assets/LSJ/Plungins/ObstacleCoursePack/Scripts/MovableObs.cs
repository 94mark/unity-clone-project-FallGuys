using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObs : MonoBehaviour
{
	public float distance = 5f; //Distance that moves the object
	public bool horizontal = true; //If the movement is horizontal or vertical
	public float speed = 3f;
	public float offset = 0f; //If yo want to modify the position at the start 

	private bool isForward = true; //If the movement is out
	private Vector3 startPos;
   
    void Awake()
    {
		startPos = transform.position;
		if (horizontal)
			transform.position += Vector3.right * offset;
		else
			transform.position += Vector3.forward * offset;
	}

    // Update is called once per frame
    void Update()
    {
		if (horizontal)
		{
			if (isForward)
			{
				if (transform.position.x < startPos.x + distance)
				{
					transform.position += Vector3.right * Time.deltaTime * speed;
				}
				else
					isForward = false;
			}
			else
			{
				if (transform.position.x > startPos.x)
				{
					transform.position -= Vector3.right * Time.deltaTime * speed;
				}
				else
					isForward = true;
			}
		}
		else
		{
			if (isForward)
			{
				if (transform.position.z < startPos.z + distance)
				{
					transform.position += Vector3.forward * Time.deltaTime * speed;
				}
				else
					isForward = false;
			}
			else
			{
				if (transform.position.z > startPos.z)
				{
					transform.position -= Vector3.forward * Time.deltaTime * speed;
				}
				else
					isForward = true;
			}
		}
    }
}
