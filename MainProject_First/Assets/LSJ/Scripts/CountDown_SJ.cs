using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CountDown_SJ : MonoBehaviour
{
    [System.Serializable]
    private class CountDownEvent : UnityEvent { }
    private CountDownEvent endOfCountDown;

    private TextMeshProUGUI textCountDown;
    private AudioSource audioSource;

    [SerializeField]
    private int maxFontSize;
    [SerializeField]
    private int minFontSize;

    private void Awake()
    {
        endOfCountDown = new CountDownEvent();
        textCountDown = GetComponent<TextMeshProUGUI>();
        // audioSource = GetComponent<AudioSource>();
    }

    public void StartCountDown(UnityAction action, int start = 3, int end = 1)
    {
        StartCoroutine(OnCountDown(action, start, end));
    }

    private IEnumerator OnCountDown(UnityAction action, int start, int end)
    {
        endOfCountDown.AddListener(action);

        while (start > end - 1)
        {
            // audioSource.Play();
            textCountDown.text = start.ToString();
            yield return StartCoroutine("OnFontAnimation");
            start--;
        }

        endOfCountDown.Invoke();
        endOfCountDown.RemoveListener(action);
        gameObject.SetActive(false);
    }

    private IEnumerator OnFontAnimation()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;

            textCountDown.fontSize = Mathf.Lerp(maxFontSize, minFontSize, percent);

            yield return null;
        }
    }
}
