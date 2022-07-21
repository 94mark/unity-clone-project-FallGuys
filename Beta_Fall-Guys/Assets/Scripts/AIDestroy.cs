using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "DestinationPos")
        {
            Destroy(collision.gameObject);
        }
    }
}
