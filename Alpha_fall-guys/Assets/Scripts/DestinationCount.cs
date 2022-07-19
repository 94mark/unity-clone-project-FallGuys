using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationCount : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        UIManager.Instance.CurRank++;
    }
}
