using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_SJ : MonoBehaviour
{
    private Rigidbody rigid;

    public int JumpPower;
    public int MoveSpeed;

    private bool IsJumping;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        IsJumping = false;
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Translate((new Vector3(h, 0, v) * MoveSpeed) * Time.deltaTime);
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!IsJumping)
            {
                IsJumping = true;
                rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);             
            }
            else
            {
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
        }
    }
}
