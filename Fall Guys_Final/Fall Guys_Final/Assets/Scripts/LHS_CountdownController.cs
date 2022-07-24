using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 3, 2 ,1 , GO!를 한 후 게임을 시작하고 싶다
// 게임이 멈춰 있는다
// 1초 간격으로 3, 2, 1이 나온다
// 바뀔 때마다 UI를 껐다 켜줬다 한다
// 시간이 다 지나면 GO! UI를 끄고
// 다시 게임을 켜준다

public class LHS_CountdownController : MonoBehaviour
{
    public int countdownTime = 4;

    public Text countdownDisplay;
    public GameObject anim;

    public GameObject Num_A;   //1번
    public GameObject Num_B;   //2번
    public GameObject Num_C;   //3번
    public GameObject Num_GO;
    //public GameObject Bar;
    
    // 텍스트 효과
    Animator animator;

    // 사운드 효과
    public AudioSource mysfx;
    public AudioClip startsfx;
    public AudioClip gosfx;

    private void Awake()
    {
        
        animator = anim.GetComponent<Animator>();
        StartCoroutine(CountdownToStart());

        Num_A.SetActive(false); //1번
        Num_B.SetActive(false); //2번
        Num_C.SetActive(false); //3번
        Num_GO.SetActive(false);

        Time.timeScale = 0;
    }

    //코루틴 함수 사용
    IEnumerator CountdownToStart()
    {
        //Bar.SetActive(true);

        while (countdownTime > 0)
        {
            //Bar.SetActive(true);

            ChangeImage();

            countdownDisplay.text = countdownTime.ToString();

            // 1초 대기
            yield return new WaitForSecondsRealtime(1f);

            countdownTime--;
        }

        // 끝나면 게임이 시작
        countdownDisplay.text = "GO!";
        //Num_GO.SetActive(false);

        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(1f);

        countdownDisplay.gameObject.SetActive(false);
    }

    void ChangeImage()
    {
        int i = countdownTime;

        if (i == 4)
        {
            Num_C.SetActive(true);
            animator.SetBool("Num3", true);
            mysfx.PlayOneShot(startsfx);

        }

        if (i == 3)
        {
            //Num_C.SetActive(false);
            Num_B.SetActive(true);
            //animator.SetBool("Num3", true);
            mysfx.PlayOneShot(startsfx);
        }

        if (i == 2)
        {
            //Num_B.SetActive(false);
            Num_A.SetActive(true);
            //animator.SetBool("Num3", true);
            mysfx.PlayOneShot(startsfx);
        }

        if (i == 1)
        {
            //Num_A.SetActive(false);
            Num_GO.SetActive(true);
            //animator.SetBool("Num3", true);
            mysfx.PlayOneShot(gosfx);
        }

    }
}

