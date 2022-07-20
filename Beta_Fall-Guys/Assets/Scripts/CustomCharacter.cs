using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacter : MonoBehaviour
{
    public static CustomCharacter Instance;
    public GameObject[] Characters;
    public SkinnedMeshRenderer[] Hairs;
    public SkinnedMeshRenderer[] Faces;
    public SkinnedMeshRenderer[] Bodys;
    public Material[] HairMaterials;
    public Material[] FaceMaterials;
    int CharacterNum = 0;

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

    private void Start()
    {
        Init();
    }

    void Init()
    {
        foreach (var character in Characters)
        {
            character.SetActive(false);
        }
        Characters[0].SetActive(true);
    }

    public void SelectCharacter(int num)
    {
        foreach (var character in Characters)
        {
            character.SetActive(false);
        }
        Characters[num].SetActive(true);
        CharacterNum = num;
    }

    public void SelectHairColor(int num)
    {
        Material[] material = Hairs[CharacterNum].materials;
        material[0] = HairMaterials[CharacterNum * 4 + num];
        Hairs[CharacterNum].materials = material;
    }

    public void SelectFaceColor(int num)
    {
        Material[] material = Faces[CharacterNum].materials;
        material[0] = FaceMaterials[CharacterNum * 4 + num];
        Faces[CharacterNum].materials = material;
    }
}
