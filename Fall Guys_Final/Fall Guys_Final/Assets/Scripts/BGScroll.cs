using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour
{
    public float speed = 0.2f;
    Material mat;


    // Start is called before the first frame update
    void Start()
    {
        // 머터리얼을 찾고 싶다
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mat = mr.material;

  
    }

    // Update is called once per frame
    void Update()
    {
        // 배경을 스크롤 하고 싶다
        // P =  P0 + vt
        mat.mainTextureOffset += Vector2.down * speed * Time.deltaTime;

    
    }
}
