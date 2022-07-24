using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 사용자의 입력값에 따라 좌우앞뒤로 이동하고 싶다.
// jump키를 누르면 뛰고 싶다.
public class LHS_MainPlayer : MonoBehaviour
{
    
    // 이동속도
    public float speed = 10;
    // 회전 속도
    public float rotateSpeed = 5;
    // 점프 파워
    public float jumpPower = 5;

    // 카메라
    private Camera currentCamera;
    public bool UseCameraRotation = true;

    // 점프 파티클
    public ParticleSystem dust;

    // 바
    public GameObject bar;

    // 충돌 
    public string playerTag;
    public float bounceForce;
    public ParticleSystem bounce;


    // 사운드 효과
    public AudioSource mysfx;
    public AudioClip jumpfx;
    public AudioClip bouncefx;


    Animator anim;
    Rigidbody rigid;

    // 점프 확인
    bool isJump;
    //bool isGround;
    bool jDown;
    
    bool isDie;

    float hAxis;
    float vAxis;

    Vector3 moveVec;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        // 바 활성화
        bar.SetActive(true);
        bar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3.35f, 0));
    }

    private void Start()
    {
        currentCamera = FindObjectOfType<Camera>();
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        GetInput();
        Move();
        Turn();
        Jump();
        Die();
        Expression();
    }

    void FreezeRotation()
    {
        // 충돌 했을 때 물리 회전을 안하고 싶다.
        // 스스로 도는 현상 없애기
        rigid.angularVelocity = Vector3.zero;
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        jDown = Input.GetButton("Jump");
    }

    void Move()
    {
        // 방향
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        //카메라 방향으로 돌려준다.
        if (UseCameraRotation)
        {
            //카메라의 y회전만 구해온다.
            Quaternion v3Rotation = Quaternion.Euler(0f, currentCamera.transform.eulerAngles.y, 0f);
            //이동할 벡터를 돌린다.
            moveVec = v3Rotation * moveVec;
        }

        // 움직인다
        transform.position += moveVec * speed * Time.deltaTime;

        // Move 애니메이션 true
        anim.SetBool("isMove", moveVec != Vector3.zero);
    }

    void Turn()
    {
        // 자연스럽게 회전 = 나아가는 방향으로 바라본다
        // transform.LookAt(transform.position + moveVec);
        if (hAxis == 0 && vAxis == 0)
            return;
        Quaternion newRotation = Quaternion.LookRotation(moveVec);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, newRotation, rotateSpeed * Time.deltaTime);
    }

    void Jump()
    {
        // jump하고 잇는 상황에서 Jump하지 않도록 방지
        // 점프를 하고 있지 않다면
        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;

            //anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            mysfx.PlayOneShot(jumpfx);
            dust.Play();
        }
    }

    void Die()
    {
        if (isDie)
        {
            anim.SetTrigger("doDie");
            isDie = true;
        }
    }

    // 바닥에 닿았을 때 다시 flase로 바꿔준다. (점프)
    // 충돌 시 뒤로 밀려난다 + 사운드 / 파티클 
    private void OnCollisionEnter(Collision collision)
    {
        // 바닥
        if (collision.gameObject.tag == "Floor")
        {
           // anim.SetBool("isGround", false);
           //isGround = false;
            anim.SetBool("isJump", false);
            
            isJump = false;
        }

        // 회전발판 
        else if (collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJump", false);
            
            isJump = false;
        }

        // 벽 (충돌)
        else if (collision.collider.tag == "Wall")
        {
            anim.SetTrigger("doDie");
            isDie = false;

            rigid.velocity = new Vector3(0, 0, 0);
            rigid.AddForce(Vector3.back * bounceForce, ForceMode.Impulse);

            mysfx.PlayOneShot(bouncefx);
            bounce.Play();

            bounce.transform.position = transform.position;
        }

    }

    // 감정표현
    void Expression()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetTrigger("doDance01");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetTrigger("doDance02");
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetTrigger("doVictory");
        }
    }

}



  



