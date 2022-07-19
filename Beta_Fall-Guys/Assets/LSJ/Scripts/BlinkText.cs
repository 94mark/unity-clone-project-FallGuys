using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public GameObject readText;
    void Awake()
    {
        readText.SetActive(false);
        StartCoroutine(ShowReady());
    }

    IEnumerator ShowReady()
    {
        int count = 0;
        while (count < 100)
        {
            readText.SetActive(true);
            yield return new WaitForSeconds(.5f);
            readText.SetActive(false);
            yield return new WaitForSeconds(.5f);
            count++;
        }
    }
    // Update is called once per frame
}
