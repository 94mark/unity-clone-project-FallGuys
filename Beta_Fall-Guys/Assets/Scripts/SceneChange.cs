using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "WaitingPlayers")
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Intro");
            }
        }

        if (SceneManager.GetActiveScene().name == "Intro")
        {
            if(Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("InGame");
            }
        }

        if (SceneManager.GetActiveScene().name == "InGame")
        {
            if(Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("Ending");
            }
        }
    }

    public void LoginSceneChange()
    {
        SceneManager.LoadScene("WaitingPlayers");
    }
    
    /*public void LobbySceneChange()
    {
        SceneManager.LoadScene("WaitingPlayers");
    }*/

    public void WaitingPalyersSceneChange()
    {
        SceneManager.LoadScene("Intro");
    }

    public void IntroSceneChange()
    {
        SceneManager.LoadScene("InGame");
    }

    public void InGameSceneChange()
    {
        SceneManager.LoadScene("Ending");
    }
}
