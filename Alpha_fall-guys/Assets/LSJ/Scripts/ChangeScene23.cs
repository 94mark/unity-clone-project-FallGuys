using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene23 : MonoBehaviour
{
    // Start is called before the first frame update
    public void SceneChange()
    {
        SceneManager.LoadScene("InGame_map.v1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
