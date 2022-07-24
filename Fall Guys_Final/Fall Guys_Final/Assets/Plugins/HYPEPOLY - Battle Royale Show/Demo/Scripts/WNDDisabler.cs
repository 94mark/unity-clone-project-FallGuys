using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDDisabler : MonoBehaviour
{
    public bool DisableAtPosition = true;
    public Vector3 disablingPosition;
    public float timeToDisableMin, timeToDisableMax;
    bool mooving = false;
    float moovingA = 0f;
    Vector3 startPos, startScale;
    void Start()
    {
        StartCoroutine(Disabler());
    }
    void FixedUpdate()
    {
        if(mooving)
        {
            transform.localPosition = Vector3.Lerp(startPos, startPos + disablingPosition, moovingA);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, moovingA);
            if(moovingA>=1f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                moovingA += Time.fixedDeltaTime;
            }
        }
    }
    IEnumerator Disabler()
    {
        yield return new WaitForSeconds(Random.Range(timeToDisableMin, timeToDisableMax));
        if(DisableAtPosition)
        {
            mooving = true;
            startPos = transform.localPosition;
            startScale = transform.localScale;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
