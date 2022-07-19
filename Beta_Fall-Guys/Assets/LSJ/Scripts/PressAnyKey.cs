using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressAnyKey : MonoBehaviour
{
    public GameObject pressText;
    void Awake()  
    {
        pressText.SetActive(false);
        StartCoroutine(ShowReady());
    }

    IEnumerator ShowReady()
    {
        int count = 0;
        while (count < 100)
        {
            pressText.SetActive(true);
            yield return new WaitForSeconds(.5f);
            pressText.SetActive(false);
            yield return new WaitForSeconds(.5f);
            count++;
        }
    }
}
