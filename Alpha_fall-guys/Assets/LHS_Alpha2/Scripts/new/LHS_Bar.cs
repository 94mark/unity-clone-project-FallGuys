using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LHS_Bar : MonoBehaviour
{
 
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Camera.main.WorldToScreenPoint(player.transform.position + Vector3.up * 2.8f );
    }
}
