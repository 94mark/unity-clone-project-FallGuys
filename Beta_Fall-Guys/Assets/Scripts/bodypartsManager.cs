using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bodypartsManager : MonoBehaviour
{
    public GameObject[] colors;
    // Start is called before the first frame update
    void Start()
    {
        colors[0].SetActive(true);
    }

    public void Btnup(int num)
    {
        if (colors[0].activeSelf == true)
        {
            colors[0].SetActive(false);
            colors[num++].SetActive(true);
        }
            
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
