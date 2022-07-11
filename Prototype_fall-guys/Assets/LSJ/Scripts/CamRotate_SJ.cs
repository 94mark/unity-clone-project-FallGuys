using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate_SJ : MonoBehaviour
{
    float rx;
    float ry;
    public float rotSpeed = 200;

    public Transform body;

    Vector3 nearPosition;
    Vector3 farPosition;
    float wheelValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 카메라의 가장 가까운 위치를 플레이어를 기준으로 한 현재 위치로 지정
        nearPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스의 입력 값을 이용해서
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        rx += rotSpeed * my * Time.deltaTime;
        ry += rotSpeed * mx * Time.deltaTime;

        // rx의 회전 각을 제한하고 싶다 -80도 ~ + 80도
        rx = Mathf.Clamp(rx, -80, 80);

        // 회전하고 싶다
        transform.eulerAngles = new Vector3(-rx, ry, 0);

        body.transform.eulerAngles = new Vector3(0, ry, 0);


        // 케메라의 정면 방향을 기준으로 뒤쪽 방향의 5m 지점으로 지정
        farPosition = nearPosition + transform.forward * -5;
        // 마우스 스크롤 휠로 이동
        wheelValue -= Input.GetAxis("Mouse ScrollWheel");
        wheelValue = Mathf.Clamp(wheelValue, 0, 1.0f); // 0~1 사이로 제한

        // 시작 위치 - 종료 위치 중의 임의의 지점
        Vector3 camPosition = Vector3.Lerp(nearPosition, farPosition, wheelValue);

        transform.localPosition = camPosition;
    }
}
