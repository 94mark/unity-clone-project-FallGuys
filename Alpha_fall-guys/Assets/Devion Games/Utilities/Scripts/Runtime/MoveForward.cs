using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    public class MoveForward : MonoBehaviour
    {

        [SerializeField]
        private float m_Speed = 5f;
        [SerializeField]
        private bool m_LookAtCameraForward=true;
        [SerializeField]
        private bool m_AutoDestruct = true;
        [SerializeField]
        private float m_DestructDelay = 10f;
        private Rigidbody m_Rigidbody;



        private void Start()
        {
            this.m_Rigidbody = GetComponent<Rigidbody>();

            this.transform.parent = null;
            if (this.m_AutoDestruct)
                Destroy(gameObject, this.m_DestructDelay);

            if (this.m_LookAtCameraForward)
            {
                Vector3 forward = Camera.main.transform.forward;
                if (forward.sqrMagnitude != 0.0f)
                {
                    forward.Normalize();
                    transform.LookAt(transform.position + forward);
                }
            }
        }

        private void FixedUpdate()
        {
            this.m_Rigidbody.velocity = transform.forward * m_Speed; 
        }

    }
}