using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHS_Bar : MonoBehaviour
{
    //public GameObject player;

    //private GameObject m_goBar;
    public GameObject bar;
    

    // Start is called before the first frame update
    void Start()
    {
        //m_goBar = GameObject.Find("Canvas/Image");
        bar.SetActive(true);
        bar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3.35f, 0));

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //bar.SetActive(true);
        //transform.position = Camera.main.WorldToScreenPoint(player.transform.position + Vector3.up * 2.8f);
        //bar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,0.8f,0));
    }
}
