using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Timer : MonoBehaviour
{
    public Text timeText;
    public float time;

    private void Awake()
    {
        time = 60f;
    }

    private void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;

        timeText.text = Mathf.Ceil(time).ToString();
    }
}
