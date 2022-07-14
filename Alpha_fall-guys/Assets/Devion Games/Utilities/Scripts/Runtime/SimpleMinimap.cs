using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    public class SimpleMinimap : MonoBehaviour
    {
        [SerializeField]
        private string m_PlayerTag = "Player";
        [SerializeField]
        private bool m_RotateWithPlayer=false;

        private Transform m_CameraTransform;
        private Transform m_PlayerTransform;

        private void Start()
        {
            this.m_CameraTransform = transform;
            this.m_PlayerTransform = GameObject.FindGameObjectWithTag(this.m_PlayerTag).transform;
        }

        private void Update()
        {
            Vector3 position = this.m_PlayerTransform.position;
            position.y = this.m_CameraTransform.position.y;
            this.m_CameraTransform.position = position;
            if (this.m_RotateWithPlayer) {
                this.m_CameraTransform.rotation = this.m_PlayerTransform.rotation;
            }
        }
    }
}