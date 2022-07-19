using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDParticlesSpawner : MonoBehaviour
{
    public GameObject particle0, particle1;
    public void Particle0()
    {
        Instantiate(particle0, transform.position, new Quaternion());
    }
    public void Particle1()
    {
        Instantiate(particle1, transform.position, new Quaternion());
    }
}
