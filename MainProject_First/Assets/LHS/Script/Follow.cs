using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카메라가 타겟을 따라다니고 싶다.
public class Follow : MonoBehaviour
{
    // 카메라가 따라가야 할 타겟
    public Transform target;
    // 위치 오프셋(고정값)
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
