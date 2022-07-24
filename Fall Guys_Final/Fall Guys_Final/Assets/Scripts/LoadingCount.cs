using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCount : MonoBehaviour
{
    public int fullmember;
    public Text countText;
    public float increasingNum;
    public GameObject pressText;

    // Start is called before the first frame update
    void Start()
    {
        // pressText.SetActive(false);
    }

    void Update()
    {
        /*if(increasingNum <= fullmember)
        {

            countText.text = increasingNum + " / " + fullmember;
        }*/
        if (increasingNum <= fullmember)
        {
            increasingNum += Time.deltaTime;
            countText.text = Mathf.Round(increasingNum) + " / " + fullmember;
            if (increasingNum >= fullmember)
            {
                pressText.SetActive(true);

                StartCoroutine("ShowReady");
            }
        }
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
