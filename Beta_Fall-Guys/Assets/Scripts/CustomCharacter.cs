using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacter : MonoBehaviour
{
    private int colorID;
    private int bodysID;
    private int eyesID;
    private int glovesID;
    private int headsID;
    private int facesID;
    private int tailsID;

    [SerializeField] public GameObject[] Colors;
    [SerializeField] public GameObject[] Bodys;
    [SerializeField] public GameObject[] Eyes;
    [SerializeField] public GameObject[] Gloves;
    [SerializeField] public GameObject[] Heads;
    [SerializeField] public GameObject[] Faces;
    [SerializeField] public GameObject[] Tails;


    public void SelectColor(int num)
    {
        foreach (var color in Colors)
        {
            color.SetActive(false);
        }
        Colors[num].SetActive(true);
        colorID = num;
    }

    public void SelectBody(int num)
    {
        foreach (var body in Bodys)
        {
            body.SetActive(false);
        }
        Bodys[num].SetActive(true);
        bodysID = num;
    }

    public void SelectEye(int num)
    {
        foreach (var eye in Eyes)
        {
            eye.SetActive(false);
        }
        Eyes[num].SetActive(true);
        eyesID = num;
    }

    public void SelectGlove(int num)
    {
        foreach (var glove in Gloves)
        {
            glove.SetActive(false);
        }
        Gloves[num].SetActive(true);
        glovesID = num;
    }

    public void SelectHead(int num)
    {
        foreach (var head in Heads)
        {
            head.SetActive(false);
        }
        Heads[num].SetActive(true);
        headsID = num;
    }

    public void SelectFace(int num)
    {
        foreach (var face in Faces)
        {
            face.SetActive(false);
        }
        Faces[num].SetActive(true);
        facesID = num;
    }

    public void SelectTail(int num)
    {
        foreach (var tail in Tails)
        {
            tail.SetActive(false);
        }
        Tails[num].SetActive(true);
        tailsID = num;
    }
    public void SelectColors(bool isForward)
    {
        if(isForward)
        {
            if(colorID == Colors.Length - 1)
            {
                colorID = 0;
            }
            else
            {
                colorID++;
            }
        }
        else
        {
            if(colorID == 0)
            {
                colorID = Colors.Length - 1;
            }
            else
            {
                colorID--;
            }
        }
        SetItem("Colors");
    }

    public void SelectBodys(bool isForward)
    {
        if (isForward)
        {
            if (bodysID == Bodys.Length - 1)
            {
                bodysID = 0;
            }
            else
            {
                bodysID++;
            }
        }
        else
        {
            if (bodysID == 0)
            {
                bodysID = Bodys.Length - 1;
            }
            else
            {
                bodysID--;
            }
        }
        SetItem("Bodys");
    }

    public void SelectEyes(bool isForward)
    {
        if (isForward)
        {
            if (eyesID == Eyes.Length - 1)
            {
                eyesID = 0;
            }
            else
            {
                eyesID++;
            }
        }
        else
        {
            if (eyesID == 0)
            {
                eyesID = Eyes.Length - 1;
            }
            else
            {
                eyesID--;
            }
        }
        SetItem("Eyes");
    }

    public void SelectGloves(bool isForward)
    {
        if (isForward)
        {
            if (glovesID == Gloves.Length - 1)
            {
                glovesID = 0;
            }
            else
            {
                glovesID++;
            }
        }
        else
        {
            if (glovesID == 0)
            {
                glovesID = Gloves.Length - 1;
            }
            else
            {
                glovesID--;
            }
        }
        SetItem("Gloves");
    }

    public void SelectHeads(bool isForward)
    {
        if (isForward)
        {
            if (headsID == Heads.Length - 1)
            {
                headsID = 0;
            }
            else
            {
                headsID++;
            }
        }
        else
        {
            if (headsID == 0)
            {
                headsID = Heads.Length - 1;
            }
            else
            {
                headsID--;
            }
        }
        SetItem("Heads");
    }

    public void SelectFaces(bool isForward)
    {
        if (isForward)
        {
            if (facesID == Faces.Length - 1)
            {
                facesID = 0;
            }
            else
            {
                facesID++;
            }
        }
        else
        {
            if (facesID == 0)
            {
                facesID = Faces.Length - 1;
            }
            else
            {
                facesID--;
            }
        }
        SetItem("Faces");
    }
    public void SelectTails(bool isForward)
    {
        if (isForward)
        {
            if (tailsID == Tails.Length - 1)
            {
                tailsID = 0;
            }
            else
            {
                tailsID++;
            }
        }
        else
        {
            if (tailsID == 0)
            {
                tailsID = Tails.Length - 1;
            }
            else
            {
                tailsID--;
            }
        }
        SetItem("Tails");
    }

    private  void SetItem(string type)
    {
        switch(type)
        {
            case "Colors":
                SelectColor(0);
                break;
            case "Bodys":
                SelectBody(0);
                break;
            case "Eyes":
                SelectEye(0);
                break;
            case "Gloves":
                SelectGlove(0);
                break;
            case "Heads":
                SelectHead(0);
                break;
            case "Faces":
                SelectFace(0);
                break;
            case "Tails":
                SelectTail(0);
                break;
        }
    }
}
