using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            bouncing b = collision.gameObject.GetComponent<bouncing>();

            Vector3 income = b.MovePos; // ภิป็บคลอ
            Vector3 normal = collision.contacts[0].normal; // นýผฑบคลอ
            b.MovePos = Vector3.Reflect(income, normal).normalized; // นÝป็บคลอ
        }
    }
}
