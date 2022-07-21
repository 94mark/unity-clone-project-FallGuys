using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CharacterCustomization : MonoBehaviour
{
    public static CharacterCustomization Instance;
    public GameObject[] Colors;
    public GameObject[] Bodys;
    public GameObject[] Eyes;
    public GameObject[] Gloves;
    public GameObject[] Heads;
    public GameObject[] Faces;
    public GameObject[] Tails;
    int colorNum = 0;
    int bodyNum = 0;
    int eyeNum = 0;
    int gloveNum = 0;
    int headNum = 0;
    int faceNum = 0;
    int tailNum = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Init(0);
    }

    void Init(int num)
    {
        foreach(var color in Colors)
        {
            color.SetActive(false);
        }
        Colors[num].SetActive(true);

        foreach (var body in Bodys)
        {
            body.SetActive(false);
        }
        Bodys[num].SetActive(true);

        foreach (var eye in Eyes)
        {
            eye.SetActive(false);
        }
        Eyes[num].SetActive(true);

        foreach (var glove in Gloves)
        {
            glove.SetActive(false);
        }
        Gloves[num].SetActive(true);

        foreach (var head in Heads)
        {
            head.SetActive(false);
        }
        Heads[num].SetActive(true);

        foreach (var face in Faces)
        {
            face.SetActive(false);
        }
        Faces[num].SetActive(true);

        foreach (var tail in Tails)
        {
            tail.SetActive(false);
        }
        Tails[num].SetActive(true);
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            Init(0);
    }
    public void SelectColor(int num)
    {
        foreach (var color in Colors)
        {
            color.SetActive(false);
        }
        Colors[num].SetActive(true);
        colorNum = num;
    }

    public void SelectBody(int num)
    {
        foreach (var body in Bodys)
        {
            body.SetActive(false);
        }
        Bodys[num].SetActive(true);
        bodyNum = num;
    }

    public void SelectEye(int num)
    {
        foreach (var eye in Eyes)
        {
            eye.SetActive(false);
        }
        Eyes[num].SetActive(true);
        eyeNum = num;
    }

    public void SelectGlove(int num)
    {
        foreach (var glove in Gloves)
        {
            glove.SetActive(false);
        }
        Gloves[num].SetActive(true);
        gloveNum = num;
    }

    public void SelectHead(int num)
    {
        foreach (var head in Heads)
        {
            head.SetActive(false);
        }
        Heads[num].SetActive(true);
        headNum = num;
    }

    public void SelectFace(int num)
    {
        foreach (var face in Faces)
        {
            face.SetActive(false);
        }
        Faces[num].SetActive(true);
        faceNum = num;
    }

    public void SelectTail(int num)
    {
        foreach (var tail in Tails)
        {
            tail.SetActive(false);
        }
        Tails[num].SetActive(true);
        tailNum = num;
    }
}
