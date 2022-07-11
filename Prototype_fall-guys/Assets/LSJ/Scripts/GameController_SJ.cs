using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_SJ : MonoBehaviour
{
    
    [SerializeField]
    private CountDown_SJ countDown;
    [SerializeField]
    private AIMove aimove;
    private float curretTime;
    [field:SerializeField]
    public float MaxTime { set; get; }
    public float CurrentTime
    {
        set => curretTime = Mathf.Clamp(value, 0, MaxTime);
        get => curretTime;
    }

    public bool IsGameOver { private set; get; }
    private void Awake()
    {
        IsGameOver = false;
    }
    // Start is called before the first frame update
    private void Start()
    {
        countDown.StartCountDown(GameStart);
        
    }

    private void GameStart()
    {
        // aimove
        StartCoroutine("OnTimeCount");
    }

    private IEnumerator OnTimeCount()
    {
        CurrentTime = MaxTime;
        while(CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;
            yield return null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
