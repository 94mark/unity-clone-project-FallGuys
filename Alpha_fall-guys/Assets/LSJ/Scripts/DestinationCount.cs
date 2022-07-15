using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationCount : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        UIManager.Instance.CurRank++;
    }
}
