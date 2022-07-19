using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_Respawn : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnValue;
    [SerializeField] GameObject player;

    private void Update()
    {
        if(player.transform.position.y < -spawnValue)
        {
            RespawnPoint();
        }
    }

    void RespawnPoint()
    {
        transform.position = spawnPoint.position;
    }
}
