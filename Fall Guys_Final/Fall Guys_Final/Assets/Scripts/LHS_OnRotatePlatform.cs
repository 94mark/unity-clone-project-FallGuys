using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_OnRotatePlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {

            this.transform.parent = collision.transform;

        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
            this.transform.parent = null;
    }
}
