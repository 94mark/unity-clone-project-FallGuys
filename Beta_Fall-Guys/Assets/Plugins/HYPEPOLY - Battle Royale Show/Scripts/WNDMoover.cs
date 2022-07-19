using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDMoover : MonoBehaviour
{
    public float distance;
    [Range(0.01f,0.5f)]
    public float speed;
    public bool reversed;
    public bool randomStartPos;
    List<Vector3> posToMoove;

    public List<Transform> objsToMoove;

    void Start()
    {
        posToMoove = new List<Vector3>();

        for (int i = 0; i < objsToMoove.Count; i++)
        {
            if (objsToMoove[i] != null)
            {
                Vector3 newStartPoint = objsToMoove[i].localPosition;

                if(!reversed)
                    newStartPoint.x -= distance;
                else
                    newStartPoint.x += distance;

                posToMoove.Add(newStartPoint);
            }
        }

        if (randomStartPos) RandomStartPos();
    }
    void RandomStartPos()
    {
        for (int i = 0; i < objsToMoove.Count; i++)
        {
            if (objsToMoove[i] != null)
            {
                objsToMoove[i].localPosition = Vector3.Lerp(objsToMoove[i].localPosition, posToMoove[i], Random.Range(0f,1f));
            }
        }
    }
    void FixedUpdate()
    {
        for (int i = 0; i < objsToMoove.Count; i++)
        {
            if (objsToMoove[i] != null)
            {
                objsToMoove[i].localPosition = Vector3.MoveTowards(objsToMoove[i].localPosition, posToMoove[i], speed);
                if (Vector3.Distance(objsToMoove[i].localPosition, posToMoove[i]) < 0.1f)
                {
                    Vector3 newPos = posToMoove[i];
                    newPos.x *= -1f;
                    posToMoove[i] = newPos;
                }
            }
        }
    }
}
