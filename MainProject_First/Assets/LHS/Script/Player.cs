using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 입력값에 따라 좌우앞뒤로 이동하고 싶다.
// shift키를 누르면 빨리 달리고 싶다.
// jump키를 누르면 뛰고 싶다.
public class Player : MonoBehaviour
{
    // 이동속도
    public float speed = 10;
    // 빨리달리기 속도
    public float runSpeed = 2f;
    // 점프 파워
    public float jumpPower = 5;

    float hAxis;
    float vAxis;

    Vector3 moveVec;

    Animator anim;
    Rigidbody rigid;

    // 점프
    bool jDown;
    bool isJump;
    // 빨리 달리기
    bool rDown;
    // 추가액션
    //bool isVictory;


    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        //Victory();


    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        jDown = Input.GetButton("Jump");
        //shift 버튼에 RunFast 추가함
        // rDown = Input.GetButton("Runfast");
        //isVictory = Input.GetButton("victory");
    }

    void Move()
    {
        // 이동하고싶다
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // 삼항연산자로 변경 가능
        // bool 형태 조건 ? trun 일 때 값 : false 일때 값
        transform.position += moveVec * (rDown ? speed * runSpeed : speed) * Time.deltaTime;

        // Move 애니메이션 true
        anim.SetBool("isMove", moveVec != Vector3.zero);
        // 빨리 달리기
        anim.SetBool("isRun", rDown);
    }

    void Turn()
    {
        // 자연스럽게 회전 = 나아가는 방향으로 바라본다
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        // jump하고 잇는 상황에서 Jump하지 않도록 방지
        // 점프를 하고 있지 않다면
        if(jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            
            // Jump Trigger true  설정
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

   /* void Victory()
    {
        anim.SetTrigger("victory");
        isVictory =  true;
    }*/

    // 바닥에 닿았을 때 다시 flase로 바꿔준다. 
    private void OnCollisionEnter(Collision collision)
    {
        // 태그가 바닥이라면 
        if(collision.gameObject.tag == "envorionment")
        {
            isJump = false;  
        }
    }
}
