using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_Respawn2 : MonoBehaviour
{
    [SerializeField] float spawnValue;
    [SerializeField] GameObject player;

    //[SerializeField] private Transform player;
    [SerializeField] Transform respawnPoint;

    //떨어졌을때 구현을 위한
    Animator anim;

    private RaycastHit hit;
    private int layerMask;
    public float distance = 10;
    AudioSource resp;

    void Awake()
    {
        anim = player.GetComponentInChildren<Animator>();
        layerMask = 1 << 7;
        resp = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        /*
        if (player.transform.position.y < -spawnValue)
        {
            DownPlayer();
        }
        */
        
        // 플레이어를 기준으로 레이를 쐈는데
        // RespawnTrigger랑 거리가 Distance 차이라면
        // DownPlayer를 실행 시키고 싶다
        // DownPlayer 애니메이션도 실행시키고 싶다.

        if (Physics.Raycast(player.transform.position, -player.transform.up, out hit, distance, layerMask))
        {
            //RespSound();
            DownPlayer();
            resp.Play();
        }
        
    }

    void DownPlayer()
    {
        anim.SetBool("isFalling", true);
        //resp.Play();
    }

    // RaspawnTrigger에 충돌했을때 리스폰 지점으로 돌아가고 싶다
    // 애니메이션도 끄고 싶다.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            
            anim.SetBool("isFalling", false);

            player.transform.position = respawnPoint.transform.position;
            //player.transform.GetChild(0).transform.position = new Vector3(0, 0.09f, 0);
            // 변환변경사항을 물리엔진에 적용
            //Physics.SyncTransforms();
            
        }
    }
    /*public void RespSound()
    {
        AudioSource resp = GetComponent<AudioSource>();
        resp.Play();
    }
    */
}
