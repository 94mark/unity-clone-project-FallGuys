using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    public class MoveTo : MonoBehaviour
    {

        [SerializeField]
        private string m_Tag="Player";
        [SerializeField]
        private float speed = 3f;
        private Transform player;
        [SerializeField]
        private Vector3 m_Offset = Vector3.up;

        void Start()
        {
            GameObject go = GameObject.FindGameObjectWithTag(this.m_Tag);
            if (go != null)
                player = go.transform;

            transform.rotation = Random.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (player == null)
                return;

            Vector3 dir = (player.position+ this.m_Offset) - transform.position;
            //  dir.y = 0.0f;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
            

            // transform.LookAt(player.position + this.m_Offset);
            if (Vector3.Distance(transform.position, player.position + this.m_Offset) > 0.5f)
            {
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}