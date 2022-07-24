using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDParticlesDestroyer : MonoBehaviour
{
    ParticleSystem ParticleSystem;

    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if(!ParticleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
