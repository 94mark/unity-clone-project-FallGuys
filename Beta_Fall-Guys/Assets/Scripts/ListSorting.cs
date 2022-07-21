using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ListSorting : MonoBehaviour
{
    public GameObject targetPos;
    public List<GameObject> runnerList = new List<GameObject>();
   // List<string> runnerNames = new List<string>();
    [SerializeField] List<float> distanceList = new List<float>();

    private void Start()
    {
       
    }

    void GetInitialTarget()
    {
        if (runnerList.Count >= 20)
        {
            distanceList.Clear();
            foreach (GameObject item in runnerList)
            {
                float dist = Vector3.Distance(targetPos.transform.position, item.transform.position);

                distanceList.Add(dist);
                //Debug.Log(item + " : " + dist);
            }

            runnerList = WinnerSort(distanceList, runnerList);

            string result = "";
            foreach(GameObject runner in runnerList)
            {
                result += runner.name + "\r\n";
            }
            print(result);
        }
        //else if (targetList.Count == 1)
        //{
        //    targetPos = targetList[0];
        //}
    }


    List<GameObject> WinnerSort(List<float> distances, List<GameObject> players)
    {
       
        for(int i = 0; i < distances.Count - 1; i++)
        {
            for(int j = 1; j < distances.Count; j++ )
            {
                if(distances[i] > distances[j])
                {
                    float temp = distances[i];
                    distances[i] = distances[j];
                    distances[j] = temp;

                    GameObject tempObject = players[i];
                    players[i] = players[j];
                    players[j] = tempObject;
                }
            }
        }

        return players;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            GameObject targetGO = other.transform.gameObject;

            //targetList.Add(targetGO);
        }
    }
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        GetInitialTarget();
    }
}
