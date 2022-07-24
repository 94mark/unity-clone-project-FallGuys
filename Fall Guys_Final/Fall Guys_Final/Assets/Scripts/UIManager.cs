using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float limitTime;
    public Text textTimer;
    int min;
    float sec;
    public GameObject roundOver;
    public GameObject success;
    public GameObject failure;
    public GameObject player;
    public GameObject destPos;
    public GameObject boxTriggerPoint;
    int curRank = 0;
    public Text curRankUI;

    // LHS 파티클
    //public ParticleSystem winParticle;
    //public ParticleSystem win;


    // LHS 사운드 효과
    //public AudioSource mysfx;
    //public AudioClip winfx;
    //public AudioClip losefx;
   
    //public AudioClip losefx;


    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
     
    }
    public int CurRank
    {
        get
        {
            return curRank;
        }
        set
        {
            curRank = value;
            curRankUI.text = curRank + " / 20 ";
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        roundOver.SetActive(false);
    }

    float waitTime = 2f;
    float curretTime = 0;
    // Update is called once per frame
    void Update()
    {
        Timer();
    }


    void Timer()
    {
        limitTime -= Time.deltaTime;

        if (limitTime >= 60f)
        {
            min = (int)limitTime / 60;
            sec = limitTime % 60;
            textTimer.text = min + " : " + (int)sec;
        }
        if (limitTime < 60f)
            textTimer.text = "<color=white>" + (int)limitTime + "</color>";
        if (limitTime < 30f)
            textTimer.text = "<color=red>" + (int)limitTime + "</color>";
        if (limitTime <= 0)
        {
            textTimer.text = "<color=red>" + "Time Over" + "</color>";
            roundOver.SetActive(true);

            curretTime += Time.deltaTime;

            if (roundOver.activeSelf == true)
            {
                if (curretTime > waitTime)
                {
                    

                    if (player.transform.position.z > 560)
                    {
                        if (curretTime > 3f)
                        {
                            roundOver.SetActive(false);
                            success.SetActive(true);

                            // LHS 효과
                            //winParticle.Play();
                            //win.Play();
                            //mysfx.PlayOneShot(winfx);
                            // winSound.Play();
                            //curretTime = 2;
                            LHS_Particle.Instance.Success();

                            LHS_Particle.Instance.transform.position = player.transform.position + new Vector3(0, 4f, 0);

                        }

                    }

                        
                    else
                    {
                        if (curretTime > 3f)
                        {
                            roundOver.SetActive(false);
                            failure.SetActive(true);

                            //curretTime = 2;

                            // LHS 효과
                            //mysfx.PlayOneShot(losefx);


                        }
                        
                    }
                }
            }
        }
    }
}