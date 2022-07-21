using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    enum Parts
    {
        Colors,
        Body,
        Eyes,
        Gloves,
        Head,
        Face,
        Tail        
    }

    [SerializeField] private GameObject[] Colors;
    [SerializeField] private GameObject[] Body;
    [SerializeField] private GameObject[] Eyes;
    [SerializeField] private GameObject[] Gloves;
    [SerializeField] private GameObject[] Head;
    [SerializeField] private GameObject[] Face;
    [SerializeField] private GameObject[] Tail;

    GameObject activecolor;

    void Modify(Parts parts, int id)
    {
       
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
