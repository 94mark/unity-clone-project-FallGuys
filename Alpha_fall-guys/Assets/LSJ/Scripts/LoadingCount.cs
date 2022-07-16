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
        // Text countText = GetComponent<Text>();
        // StartCoroutine("Count");

        // pressText.SetActive(false);
    }

    void Update()    {

        /*if(increasingNum <= fullmember)
        {

            countText.text = increasingNum + " / " + fullmember;
        }*/
        if (increasingNum <= fullmember)
        {
            increasingNum += Time.deltaTime;
            countText.text = Mathf.Round(increasingNum) + " / " + fullmember;
            /*if(increasingNum == fullmember)
            {
                pressText.SetActive(true);
            }*/
        }
    }

    /* IEnumerator Count(float target, float current)
    {
        float duration = 0.5f;
        float offset = (target - current) / duration;

        while(current < target)
        {
            current += offset * Time.deltaTime;
            countText.text = ((int)current).ToString();
            yield return null;
        }

        current = target;
        countText.text = ((int)current).ToString();
    }*/
}
