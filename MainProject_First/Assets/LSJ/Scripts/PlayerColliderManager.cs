using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderManager : MonoBehaviour
{
    [SerializeField] GameOverManager gameOverManager;
    // Start is called before the first frame update

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("destination"))
        {
            gameOverManager.SetGameOver();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
