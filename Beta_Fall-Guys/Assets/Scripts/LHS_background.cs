using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_background : MonoBehaviour
{
   
    // 좌우로 이동가능한 x최대값
    public float delta = 0.02f;
    // 이동속도
    public float speed = 20f;

    // 현재위치
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = pos;
        // 좌우 이동의 최대치 및 반전 처리
        v.x += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;

    }
}
