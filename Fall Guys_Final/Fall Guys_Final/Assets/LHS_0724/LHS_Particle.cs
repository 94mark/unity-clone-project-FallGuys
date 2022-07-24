using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHS_Particle : MonoBehaviour
{
    public ParticleSystem winParticle;
    public ParticleSystem win;

    // UI
    public GameObject winUI;

    // ¿Àµð¿À
    //AudioSource winSound;

    public static LHS_Particle Instance;
    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {

        //winSound = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Success()
    {
        winUI.SetActive(true);
        winParticle.Play();
        win.Play();

        //winSound.Play();
    }
}
