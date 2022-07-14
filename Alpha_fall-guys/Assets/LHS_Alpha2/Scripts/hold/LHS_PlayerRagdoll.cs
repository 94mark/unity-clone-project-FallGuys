using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 입력값에 따라 좌우앞뒤로 이동하고 싶다.
// jump키를 누르면 뛰고 싶다.
public class LHS_PlayerRagdoll : MonoBehaviour
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
    //Rigidbody rigid;

    // 점프
    bool jDown;
    bool isJump;
    bool isDie;

    //************** 래그돌 *************//
    // 뼈의 좌표 변수
    // Bones Details
    private Rigidbody[] bones;
    private Quaternion[] rotations;

    // Hips
    private Vector3 hipsPosition;
    public Transform hips;

    // Animator
    private Animator animator;

    // 래그돌 상태
    private bool activateRagdoll;
    public float rotationSpeed = 3, movementSpeed = 0.33f;

    // Start is called before the first frame update
    void Awake()
    {
        //rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentCamera = FindObjectOfType<Camera>();

        //************** 래그돌 *************//
        // 모든 뼈의 변수에 저장하여 제어
        // 자식 리지드바디를 가져와서 모든 뼈에 넣는다.
        bones = GetComponentsInChildren<Rigidbody>();

        // 회전
        rotations = new Quaternion[bones.Length];

        // 연결된 Animator 구성요소가 있는지 확인한 후
        // 해당 구성요소에 Animator 변수를 할당
        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Expression();
        //Die();
        EnableRagdoll();
        DisableRagdoll();
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
            //rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    /*void Die()
    {
        if (isDie)
        {
            //rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
            anim.SetTrigger("doDie");
            isDie = true;
        }
    }*/

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
            //anim.SetTrigger("doDie");
            //isDie = false;
           

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

    //************** 래그돌 *************//
    public void EnableRagdoll()
    {
        UpdateRagdollBones();
        activateRagdoll = true;
        // 0초 후 애니메이터 사용 안함
        if (animator)
            StartCoroutine(ToggleAnimator(false, 0));
        else
            Debug.LogWarning("There's no Animator component assigned.");
    }

    public void DisableRagdoll()
    {
        activateRagdoll = false;
        // 1.5초에서 애니메이터 사용
        if (animator)
            StartCoroutine(ToggleAnimator(true, 1.5f));
        else
            Debug.LogWarning("There's no Animator component assigned.");
    }

    private void UpdateRagdollBones()
    {
        // hips 위치를 현재 캐릭터 위치로 설정
        hipsPosition = transform.position;
        // 그리고 현재 엉덩이 위치에서 y위치를 얻습니다.
        hipsPosition.y = hips.position.y;
        // Update the rotations array // 회전 배열 업데이트
        for (int i = 0; i < bones.Length; i++)
            rotations[i] = bones[i].transform.rotation;
    }

    private IEnumerator ToggleAnimator(bool actv, float time)
    {
        // 시간을 기다린 다음 다음 애니메이터를 actv로 설정
        // Wait for "time" seconds and then set animator to "actv"
        yield return new WaitForSeconds(time);
        animator.enabled = actv;
    }

    private void FixedUpdate()
    { 
        if (activateRagdoll)
        {
            // 래그돌 효과를 활성화하려면 모든 골격에서 Kinematic을 false로 설정
            // Set isKinematic to false in all bones to activated the Ragdoll effect
            for (int i = 0; i<bones.Length; i++)
                if (bones[i].isKinematic)
                    bones[i].isKinematic = false;
        }
        // 래그돌이 비활성화되면 iskinematic 기능을 활성화
        else
        {
             for (int i = 0; i < bones.Length; i++)
             {
                 // 중력 및 충돌에 영향을 받지 않도록 각 뼈의 키네마틱을 참으로 설정합니다.
                 // Set each bone's isKinematic to true so it won't be affected by gravity and collision
                 if (!bones[i].isKinematic)
                 bones[i].isKinematic = true;
                 // 그런 다음 각 뼈의 회전을 이전 회전으로 업데이트합니다.
                 // Then we update the rotation of each bone to its previous rotation
                 bones[i].transform.rotation = Quaternion.Lerp(bones[i].transform.rotation, rotations[i], Time.deltaTime * rotationSpeed);
                 // 그리고 엉덩이를 래그돌 전 마지막 위치로 옮겨주세요
                 // 활성화 전 마지막 좌표를 뼈 회전과 엉덩이 위치를 변경
                 // And move the hips to the last position before Ragdoll
                 hips.position = Vector3.MoveTowards(hips.position, hipsPosition, Time.deltaTime * movementSpeed);
             }
        }
    }

    // 래그돌이 활성상태인지 공개방법
    // Public method to check if the Ragdoll is active
    public bool isRagdollActive()
    {
        return this.activateRagdoll;
    }
}





