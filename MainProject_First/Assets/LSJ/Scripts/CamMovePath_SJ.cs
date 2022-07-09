using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovePath_SJ : MonoBehaviour
{
	public bool bDebug = true;
	public float Radius = 2.0f;
	public Transform[] transPos;
	public float speed = 2.0f;
	public float mass = 5.0f;
	public bool isLooping = true;
	private float curSpeed;
	private int curPathIndex;
	private float pathLength;
	private Vector3 targetPoint;

	Vector3 velocity;
	void Start()
	{
		pathLength = Length;
		curPathIndex = 0;
		velocity = transform.forward;
	}

	void Update()
	{
		curSpeed = speed * Time.deltaTime;
		targetPoint = GetPoint(curPathIndex);
		if (Vector3.Distance(transform.position, targetPoint) < Radius)
		{
			if (curPathIndex < pathLength - 1)
			{
				curPathIndex++;
			}
			else if (isLooping)
			{
				curPathIndex = 0;
			}
			else
			{
				return;
			}
		}
		if (curPathIndex >= pathLength)
		{
			return;
		}
		if (curPathIndex >= pathLength - 1 && !isLooping)
		{
			velocity += Steer(targetPoint, true);
		}
		else
		{
			velocity += Steer(targetPoint);
		}

		transform.position += velocity;
		transform.rotation = Quaternion.LookRotation(velocity);
	}

	public Vector3 Steer(Vector3 target, bool bFinalPoint = false)
	{
		Vector3 desiredVelocity = (target - transform.position);
		float dist = desiredVelocity.magnitude;

		desiredVelocity.Normalize();

		if (bFinalPoint && dist < 10.0f)
		{
			desiredVelocity *= (curSpeed * (dist / 10.0f));
		}
		else
		{
			desiredVelocity *= curSpeed;
		}

		Vector3 steeringForce = desiredVelocity - velocity;
		Vector3 acceleration = steeringForce / mass;

		return acceleration;
	}

	public float Length
	{
		get
		{
			return transPos.Length;
		}
	}
	public Vector3 GetPoint(int index)
	{
		return transPos[index].position;
	}
	void OnDrawGizmos()
	{

		if (!bDebug)
		{
			return;
		}
		for (int i = 0; i < transPos.Length; i++)
		{
			if (i + 1 < transPos.Length)
			{
				Debug.DrawLine(transPos[i].position, transPos[i + 1].position, Color.red);
			}
		}
	}
}
