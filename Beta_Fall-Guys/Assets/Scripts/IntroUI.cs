using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{
    public GameObject missionUI;
    public GameObject missionPos;
    Vector3 dir;
    public float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        dir = missionPos.transform.position - missionUI.transform.position;
        missionUI.transform.position += dir * speed * Time.deltaTime;
    }
}
