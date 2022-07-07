using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMove : MonoBehaviour
{
    GameObject destPos;
    public float speed = 3f;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destPos = GameObject.Find("DestinationPos");
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = destPos.transform.position;    
    }
}
