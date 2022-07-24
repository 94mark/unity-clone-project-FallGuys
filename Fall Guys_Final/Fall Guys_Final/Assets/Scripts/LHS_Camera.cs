using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_Camera : MonoBehaviour
{
    float x;
    float y;
    public float distance = 10;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        CameraRotate();
    }
    void CameraRotate()
    {
        // 마우스 좌우 이동 누적
        x += Input.GetAxis("Mouse X");
        // 마우스 상하 이동 누적
        y -= Input.GetAxis("Mouse Y");
        // 이동량에 따라 카메라가 바라보는 방향 조정
        transform.rotation = Quaternion.Euler(y, x, 0);
        // 돌아갈 수 있는 각도 제한
        y = Mathf.Clamp(y, -10, 30);
        // 카메라와 플레이어의 거리조정
        Vector3 reDistance = new Vector3(0f, -1.8f, distance);
        transform.position = player.transform.position - transform.rotation * reDistance;
    }
}
