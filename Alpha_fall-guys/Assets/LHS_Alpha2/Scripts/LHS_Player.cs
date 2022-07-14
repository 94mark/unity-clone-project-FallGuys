using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 입력값에 따라 좌우앞뒤로 이동하고 싶다.
// jump키를 누르면 뛰고 싶다.
public class LHS_Player : MonoBehaviour
{
    // 이동속도
    public float speed = 10;

    // 점프 파워
    public float jumpPower = 5;

    float hAxis;
    float vAxis;

    Vector3 moveVec;

    private Camera currentCamera;
    public bool UseCameraRotation = true;

    Animator anim;
    Rigidbody rigid;

    // 점프
    bool jDown;
    bool isJump;

    bool isDie;

    // 캐릭터 오브젝트
    public GameObject charObj;
    // 랙돌 오브젝트
    public GameObject ragdollObj;
    // 힘을 가할 rigidbody
    public Rigidbody spine;



    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentCamera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Expression();
        Die();
  
    }

    //충돌 했을 때 물리 회전을 안하고 싶다.
    // 스스로 도는 현상 없애기
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }


    private void FixedUpdate()
    {
        FreezeRotation();
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
            //Debug.Log(currentCamera.transform.eulerAngles.y.ToString());
        }

        transform.position += moveVec * speed * Time.deltaTime;

        // Move 애니메이션 true
        anim.SetBool("isMove", moveVec != Vector3.zero);

        
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

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Die()
    {
        if (isDie)
        {
            //rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
            anim.SetTrigger("doDie");
            isDie = true;
        }
    }

    // 바닥에 닿았을 때 다시 flase로 바꿔준다. 
    private void OnCollisionEnter(Collision collision)
    {
        // 태그가 바닥이라면 
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

        else if (collision.gameObject.tag == "Platform")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

        else if (collision.collider.tag == "Wall")
        {
            anim.SetTrigger("doDie");
            isDie = false;
            ChangeRagdoll();
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

    public void ChangeRagdoll()
    {
        CopyCharacterTransformToRagdoll(charObj.transform, ragdollObj.transform);

        charObj.SetActive(false);
        ragdollObj.SetActive(true);
        //spine.velocity = GetComponent<Rigidbody>().velocity;
        spine.AddForce(new Vector3(0f, 100f, -100f), ForceMode.Impulse);
    }

    private void CopyCharacterTransformToRagdoll(Transform origin, Transform ragdoll)
    {
        for (int i = 0; i < origin.childCount; i++)
        {
            if (origin.childCount != 0)
            {
                CopyCharacterTransformToRagdoll(origin.GetChild(i), ragdoll.GetChild(i));
            }

            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
    }
}

  



