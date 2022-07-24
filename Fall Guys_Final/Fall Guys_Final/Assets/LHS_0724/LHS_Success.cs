using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_Success : MonoBehaviour
{
    public ParticleSystem winParticle;
    public ParticleSystem win;

    // Start is called before the first frame update
    void Start()
    {
        winParticle.Play();
        win.Play();

    }

    // Update is called once per frame
    void Update()
    {
        winParticle.transform.position = transform.position;
        win.transform.position = transform.position;

    }
}
