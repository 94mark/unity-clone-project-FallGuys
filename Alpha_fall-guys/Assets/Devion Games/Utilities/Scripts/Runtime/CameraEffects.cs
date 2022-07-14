using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    /// <summary>
    /// Camera Effects
    /// </summary>
    public class CameraEffects : MonoBehaviour
    {
        public Vector3 amount = new Vector3(1f, 1f, 0);
        public float duration = 1;
        public float speed = 10;
        public AnimationCurve curve = AnimationCurve.EaseInOut(0, 1, 1, 0);
        public bool deltaMovement = true;

        protected Camera m_Camera;
        protected float m_Time = 0;
        protected Vector3 m_LastPosition;
        protected Vector3 m_NextPosition;
        protected float m_LastFieldOfView;
        protected float m_NextFieldOfView;

        private void Awake()
        {
            m_Camera = GetComponent<Camera>();
        }

        public static void Shake(float duration = 1f, float speed = 10f, Vector3? amount = null, Camera camera = null, bool deltaMovement = true, AnimationCurve curve = null)
        {

            var instance = ((camera != null) ? camera : Camera.main).gameObject.AddComponent<CameraEffects>();
            instance.duration = duration;
            instance.speed = speed;
            if (amount != null)
                instance.amount = (Vector3)amount;
            if (curve != null)
                instance.curve = curve;
            instance.deltaMovement = deltaMovement;
            instance.ResetCamera();
            instance.m_Time = duration;
        }

        private void LateUpdate()
        {
            if (m_Time > 0)
            {
                m_Time -= Time.deltaTime;
                if (m_Time > 0)
                {
                    m_NextPosition = (Mathf.PerlinNoise(m_Time * speed, m_Time * speed * 2) - 0.5f) * amount.x * transform.right * curve.Evaluate(1f - m_Time / duration) +
                              (Mathf.PerlinNoise(m_Time * speed * 2, m_Time * speed) - 0.5f) * amount.y * transform.up * curve.Evaluate(1f - m_Time / duration);
                    m_NextFieldOfView = (Mathf.PerlinNoise(m_Time * speed * 2, m_Time * speed * 2) - 0.5f) * amount.z * curve.Evaluate(1f - m_Time / duration);

                    m_Camera.fieldOfView += (m_NextFieldOfView - m_LastFieldOfView);
                    m_Camera.transform.Translate(deltaMovement ? (m_NextPosition - m_LastPosition) : m_NextPosition);

                    m_LastPosition = m_NextPosition;
                    m_LastFieldOfView = m_NextFieldOfView;
                }
            }
        }

        private void ResetCamera()
        {
            m_Camera.transform.Translate(deltaMovement ? -m_LastPosition : Vector3.zero);
            m_Camera.fieldOfView -= m_LastFieldOfView;
            m_LastPosition = m_NextPosition = Vector3.zero;
            m_LastFieldOfView = m_NextFieldOfView = 0f;
        }
    }
}