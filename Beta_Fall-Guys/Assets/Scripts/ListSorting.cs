using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListSorting : MonoBehaviour
{
    public GameObject players;
    public GameObject targetPos;

    private List<GameObject> targetList = new List<GameObject>();

    private void Awake()
    {
        players = GameObject.FindWithTag("Player");
    }

    void GetInitialTarget()
    {
        if (targetList.Count >= 2)
        {
            foreach (GameObject item in targetList)
            {
                float dist = Vector3.Distance(players.transform.position, item.transform.position);

                Debug.Log(item + " : " + dist);
            }

            targetList.Sort();
        }
        else if (targetList.Count == 1)
        {
            targetPos = targetList[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Check")
        {
            GameObject targetGO = other.transform.gameObject;

            targetList.Add(targetGO);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // https://stackoverflow.com/questions/47585479/trying-to-get-the-gameobject-by-distance-in-a-list
}
