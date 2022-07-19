using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDPrefabVariator : MonoBehaviour
{
    public int selectedType = 0;
    public int selectedColor = 0;
    public int selectedStyle = 0;
    public int selectedPillars = 0;

    public List<Transform> types;

    public bool partsInitialized = false;

    public Texture headerLogo;

    public bool typesSelector = false;

    public bool isDynamicFence = false;

    public float fenceLength = 10f;

    public float fencePillarsFrequency = 2f;

    public ParticleSystem myPs;
    public bool hasStyle
    {
        get
        {
            if(isDynamicFence)
            {
                return true;
            }
            else
            {
                if (types[selectedType].GetChild(selectedColor).childCount > 1) return true;
                else return false;
            }
        }
    }

    public void InitializeParts()
    {
        if (!partsInitialized)
        {
            Transform[] childList = GetComponentsInChildren<Transform>();
            types = new List<Transform>();

            foreach (Transform tr in childList)
            {
                if (tr.name.Contains("[Type]"))
                {
                    types.Add(tr);
                }
            }

            if (types.Count == 0)
            {
                types.Add(transform);
            }
            else
            {
                typesSelector = true;
            }

            partsInitialized = true;
        }
    }
    public void ChooseType(bool next)
    {
        InitializeParts();

        if (next)
        {
            selectedType++;
            if (selectedType >= types.Count) selectedType = 0;
        }
        else
        {
            selectedType--;
            if (selectedType < 0) selectedType = types.Count - 1;
        }

        Refresh();
    }
    public void RandomType()
    {
        InitializeParts();
        selectedType = Random.Range(0, types.Count);
        Refresh();
    }
    public void ChooseColor(bool next)
    {
        InitializeParts();

        if (next)
        {
            selectedColor++;
            if (selectedColor >= types[selectedType].childCount)
                selectedColor = 0;
            else if (types[selectedType].GetChild(selectedColor).gameObject.name.Contains("[Ignore]"))
                selectedColor = 0;
        }
        else
        {
            selectedColor--;
            if (selectedColor < 0)
            {
                if(types[selectedType].GetChild(types[selectedType].childCount-1).gameObject.name.Contains("[Ignore]"))
                    selectedColor = types[selectedType].childCount - 2;
                else
                    selectedColor = types[selectedType].childCount - 1;
            }
        }

        Refresh();
    }
    public void RandomColor()
    {
        InitializeParts();
        selectedColor = Random.Range(0, types[selectedType].childCount);
        Refresh();
    }
    public void ChooseStyle(bool next)
    {
        InitializeParts();

        if (next)
        {
            selectedStyle++;
            //if(!isDynamicFence)
            //    if (selectedStyle >= types[selectedType].GetChild(selectedColor).childCount) selectedStyle = 0;
            //else
            //    if (selectedStyle >= types[selectedType].GetChild(selectedColor).GetChild(0).childCount) selectedStyle = 0;
        }
        else
        {
            selectedStyle--;
            if (!isDynamicFence)
                if (selectedStyle < 0) selectedStyle = types[selectedType].GetChild(selectedColor).childCount - 1;
                else
                if (selectedStyle < 0) selectedStyle = types[selectedType].GetChild(selectedColor).GetChild(0).childCount - 1;
        }

        Refresh();
    }
    public void ChoosePillars(bool next)
    {
        InitializeParts();

        if (next)
        {
            selectedPillars++;

            if (selectedPillars >= types[selectedType].GetChild(selectedColor).GetChild(1).childCount) selectedPillars = 0;
        }
        else
        {
            selectedPillars--;

            if (selectedPillars < 0) selectedPillars = types[selectedType].GetChild(selectedColor).GetChild(1).childCount - 1;
        }

        Refresh();
    }
    public void RandomStyle()
    {
        InitializeParts();
        if (!isDynamicFence)
            selectedStyle = Random.Range(0, types[selectedType].GetChild(selectedColor).childCount);
        else
            selectedStyle = Random.Range(0, types[selectedType].GetChild(selectedColor).GetChild(0).childCount);
        Refresh();
    }
    public void RandomPillars()
    {
        InitializeParts();
        if (!isDynamicFence)
            selectedPillars = Random.Range(0, types[selectedType].GetChild(selectedColor).childCount);
        else
            selectedPillars = Random.Range(0, types[selectedType].GetChild(selectedColor).GetChild(1).childCount);
        Refresh();
    }
    public void RandomPrefab()
    {
        InitializeParts();
        RandomType();
        RandomColor();
        RandomStyle();
        Refresh();
    }
    void Refresh()
    {
        for (int i = 0; i < types.Count; i++)
        {
            if (i != selectedType)
            {
                types[i].gameObject.SetActive(false);
            }
            else
            {
                types[i].gameObject.SetActive(true);

                if (selectedColor >= types[i].childCount) selectedColor = 0;

                for (int clr = 0; clr < types[i].childCount; clr++)
                {
                    if (clr != selectedColor)
                    {
                        if(!types[i].GetChild(clr).gameObject.name.Contains("[Ignore]"))
                        types[i].GetChild(clr).gameObject.SetActive(false);
                    }
                    else
                    {
                        types[i].GetChild(clr).gameObject.SetActive(true);

                        if (!isDynamicFence)
                        {
                            if (selectedStyle >= types[i].GetChild(clr).childCount) selectedStyle = 0;

                            for (int stl = 0; stl < types[i].GetChild(clr).childCount; stl++)
                            {
                                if (stl != selectedStyle)
                                {
                                    types[i].GetChild(clr).GetChild(stl).gameObject.SetActive(false);
                                }
                                else
                                {
                                    types[i].GetChild(clr).GetChild(stl).gameObject.SetActive(true);

                                    if (myPs != null)
                                    {
                                        myPs.transform.parent = types[i].GetChild(clr).GetChild(stl);
                                        var sh = myPs.shape;
                                        sh.meshRenderer = types[i].GetChild(clr).GetChild(stl).GetComponent<MeshRenderer>();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (selectedStyle >= types[i].GetChild(clr).GetChild(0).childCount) selectedStyle = 0;

                            for (int stl = 0; stl < types[i].GetChild(clr).GetChild(0).childCount; stl++)
                            {
                                if (stl != selectedStyle)
                                {
                                    types[i].GetChild(clr).GetChild(0).GetChild(stl).gameObject.SetActive(false);
                                }
                                else
                                {
                                    types[i].GetChild(clr).GetChild(0).GetChild(stl).gameObject.SetActive(true);
                                }
                            }

                            if (selectedPillars >= types[i].GetChild(clr).GetChild(1).childCount) selectedPillars = 0;

                            for (int stl = 0; stl < types[i].GetChild(clr).GetChild(1).childCount; stl++)
                            {
                                if (stl != selectedPillars)
                                {
                                    types[i].GetChild(clr).GetChild(1).GetChild(stl).gameObject.SetActive(false);
                                }
                                else
                                {
                                    types[i].GetChild(clr).GetChild(1).GetChild(stl).gameObject.SetActive(true);
                                }
                            }

                            RefreshPillars();
                        }
                    }
                }
            }
        }
    }

    public void RefreshPillars()
    {
        Transform mainPart = types[selectedType].GetChild(selectedColor).GetChild(0).GetChild(selectedStyle).transform;
        mainPart.localScale = new Vector3(1f, 1f, 1f * (fenceLength / 18f));

        int currentPillarsCount = types[selectedType].GetChild(selectedColor).GetChild(1).GetChild(selectedPillars).childCount;
        for (int i = currentPillarsCount - 1; i >= 0; i--)
        {
            DestroyImmediate(types[selectedType].GetChild(selectedColor).GetChild(1).GetChild(selectedPillars).GetChild(i).gameObject);
        }

        GameObject pillarPrefab = types[selectedType].GetChild(selectedColor).GetChild(1).GetChild(selectedPillars).gameObject;
        Transform pillarsParent = types[selectedType].GetChild(selectedColor).GetChild(1).GetChild(selectedPillars);

        float halfSize = fenceLength / 2f;

        float frequency = halfSize / (float)((int)fencePillarsFrequency + 1);
        List<Transform> pillars = new List<Transform>();

        if (fencePillarsFrequency > 0f)
        {
            for (float i = frequency; i <= halfSize; i += frequency)
            {
                GameObject newPillar = Instantiate(pillarPrefab, pillarsParent);
                newPillar.transform.localPosition = new Vector3(0f, 0f, i);
                pillars.Add(newPillar.transform);
                newPillar.transform.parent = null;

                newPillar = Instantiate(pillarPrefab, pillarsParent);
                newPillar.transform.localPosition = new Vector3(0f, 0f, -i);
                pillars.Add(newPillar.transform);
                newPillar.transform.parent = null;
            }
        }
        else if(fencePillarsFrequency > -1f)
        {
            GameObject newPillar = Instantiate(pillarPrefab, pillarsParent);
            newPillar.transform.localPosition = new Vector3(0f, 0f, halfSize);
            pillars.Add(newPillar.transform);
            newPillar.transform.parent = null;

            newPillar = Instantiate(pillarPrefab, pillarsParent);
            newPillar.transform.localPosition = new Vector3(0f, 0f, -halfSize);
            pillars.Add(newPillar.transform);
            newPillar.transform.parent = null;
        }

        foreach (Transform tr in pillars)
        {
            tr.parent = pillarsParent;
        }
    }
}
