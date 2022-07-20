using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckPoints : MonoBehaviour
{
    // 실시간으로 변하는 랭킹을 표시하고 싶다
    // 목적지와 플레이어 사이의 distance를 구한다
    // distance가 가장 가까운 순서대로 1등 ~ 7등까지 넣는다
    // Trigger
    // 필요 속성 : 목적지, 플레이어 위치, 거리 
    public Transform[] Players;
    public Transform dest;
    public GameObject Rank1;
    public GameObject Rank2;
    public GameObject Rank3;
    public GameObject Rank4;
    public GameObject Rank5;
    public GameObject Rank6;
    public GameObject Rank7;

    // distance를 기준으로 소팅한 값을 list에 저장 -> 
    void Update()
    {
        Players = Players.OrderBy((dest) => (dest.position - transform.position).sqrMagnitude).ToArray();
        // Debug.Log(Players[i].name.ToString();
    }
    // Start is called before the first frame update
    /*void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player"))
        {
            return;
        }
        else if(other.CompareTag("Player"))
        {
            // distance가 가장 작은 값을 저장
            // 가장 작은 값을 가진 플레이어 이름 저장
            // 1등 자리에 올려주기
            float distance = Vector3.Distance(playerTransform.position, dest.position);

            for(int i = 0; i < Players.Length; i++)
            {
                Players[i] = 
                // sorting 방식 : 순위 계산하는 클래스, 리스트 2개, 거리값 저장, 오름차순/내림차순
                // 딕셔너리 리스트
            }
            
        }

        // if(transform == playerTransform.GetComponent)*/
}
