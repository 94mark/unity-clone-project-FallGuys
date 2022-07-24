using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    public ParticleSystem bounce;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.other.gameObject);

        bounce.Play();
        bounce.transform.position = transform.position;
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
