using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwingPlatform : MonoBehaviour
{
    Vector3 firstPos;
    Vector3 beforePos;
    Vector3 nowPos;

    void Start()
    {
        firstPos = this.gameObject.transform.position;
        Debug.Log(firstPos);
    }

    void Update()
    {

    }

    // 플레이어가 위치한 좌표값 변수 설정
    // 플레이어가 이동한 좌표값 변수 설정
    // 이동한 만큼 플랫폼의 회전값 변화
}
