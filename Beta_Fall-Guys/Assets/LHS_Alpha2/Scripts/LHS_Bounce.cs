using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfoGamer
{
    public class LHS_Bounce : MonoBehaviour
    {
        [SerializeField] string playerTag;
        [SerializeField] float bounceForce;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.transform.tag == playerTag)
            {
                Rigidbody otherRB = collision.rigidbody;
                otherRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 5);

            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
