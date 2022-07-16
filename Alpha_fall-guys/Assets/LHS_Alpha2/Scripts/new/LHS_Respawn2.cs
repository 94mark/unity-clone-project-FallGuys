using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_Respawn2 : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player.transform.position = respawnPoint.transform.position;
            // 변환변경사항을 물리엔진에 적용
            //Physics.SyncTransforms();
           
        }
    }
}
