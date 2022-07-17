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
    GameObject player;
    public GameObject destPos;
    int curRank = 0;
    public Text curRankUI;
    public GameObject missionUI;
    public GameObject missionPos;
    Vector3 dir;
    float speed = 3;

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

    // Update is called once per frame
    void Update()
    {

        dir = missionPos.transform.position - missionUI.transform.position;
        missionUI.transform.position += dir * speed * Time.deltaTime;
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
        }
    }

}
