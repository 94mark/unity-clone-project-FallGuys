using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AINavMesh : MonoBehaviour
{
    GameObject destPos;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destPos = GameObject.Find("DestinationPos");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.destination = destPos.transform.position;
    }
}
