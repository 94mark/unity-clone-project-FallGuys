using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaitingCharacterMove : MonoBehaviour
{
    public GameObject destPos;
    float speed = 1.8f;
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        dir = destPos.transform.position - transform.position;
        transform.position += dir * speed * Time.deltaTime;
    }
}
