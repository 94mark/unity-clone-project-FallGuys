using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AINavMesh : MonoBehaviour
{
    GameObject destPos;
    NavMeshAgent agent;

    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        destPos = GameObject.Find("RealDestPos");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.destination = destPos.transform.position;
        FreezeRotation();
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

}
