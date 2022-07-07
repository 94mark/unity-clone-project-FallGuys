using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_SJ : MonoBehaviour
{
    [SerializeField]
    private CountDown_SJ countDown;
    // Start is called before the first frame update
    private void Start()
    {
        countDown.StartCountDown(GameStart);  
    }

    private void GameStart()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
