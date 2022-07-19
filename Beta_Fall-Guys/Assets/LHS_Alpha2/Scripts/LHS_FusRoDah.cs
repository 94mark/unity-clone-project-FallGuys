using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_FusRoDah : MonoBehaviour
{
    [SerializeField]
    Rigidbody spine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            spine.AddForce(new Vector3(0f, 100f, -100f), ForceMode.Impulse);
        }
    }
}
