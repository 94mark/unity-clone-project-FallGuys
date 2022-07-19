using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WNDDispenser : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject particlePrefab;
    public float shootingDelay;
    public float power;
    public float randomPowerMinMultiplier, randomPowerMaxMultiplier;
    Transform arrowsParent;
    GameObject currentArrow;

    //Arrow Creating Variables (Animation of scaling when arrow instantiated)
    bool arrowReady = true;
    float arrowSize = 0f;
    float timeToShoot = 0f;
    float delayToSpawn = 0f;

    void Start()
    {
        Transform[] childs = GetComponentsInChildren<Transform>();
        foreach(Transform tr in childs)
        {
            if(tr.gameObject.name.Contains("[Projectile Parent]"))
            {
                arrowsParent = tr;
                break;
            }
        }
        CreateArrow();
    }
    public void CreateArrow()
    {
        arrowReady = false;
        arrowSize = 0f;
        currentArrow = GameObject.Instantiate(projectilePrefab, arrowsParent);
        if (currentArrow.GetComponent<WNDPrefabVariator>() != null)
            currentArrow.GetComponent<WNDPrefabVariator>().RandomPrefab();
        currentArrow.GetComponent<Rigidbody>().isKinematic = true;
        currentArrow.transform.localScale = Vector3.zero;
        timeToShoot = shootingDelay;
    }
    public void Shoot()
    {
        GameObject fx = Instantiate(particlePrefab, arrowsParent.transform.position, new Quaternion());
        fx.transform.localEulerAngles = Vector3.zero;

        currentArrow.transform.parent = null;
        currentArrow.GetComponent<Rigidbody>().isKinematic = false;
        currentArrow.GetComponent<Rigidbody>().AddForce(transform.up * (power*Random.Range(randomPowerMinMultiplier,randomPowerMaxMultiplier)), ForceMode.Impulse);
        delayToSpawn = 1f;
    }
    private void FixedUpdate()
    {
        if(!arrowReady)
        {
            arrowSize += (Time.fixedDeltaTime* 4f);
            currentArrow.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, arrowSize);
            if (arrowSize >= 1f) 
            {
                arrowReady = true;
            }
        }
        else if(delayToSpawn <= 0f)
        {
            if (timeToShoot > 0f) timeToShoot -= Time.fixedDeltaTime;
            else
            {
                Shoot();
            }
        }
        else
        {
            delayToSpawn -= Time.fixedDeltaTime;
            if(delayToSpawn <= 0f)
            {
                CreateArrow();
            }
        }
    }
}
