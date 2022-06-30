using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFire : MonoBehaviour
{
    public Transform Target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;

    public Transform Projectile;
    private Transform myTransform;

    void Awake()
    {
        myTransform = transform;
    }

    void Start()
    {
         StartCoroutine(SimulateProjectile());
    }


    IEnumerator SimulateProjectile()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.5f);

            Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

            float target_Distance = Vector3.Distance(Projectile.position, Target.position);

            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

            float flightDuration = target_Distance / Vx;

            Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);

            float elapse_time = 0;

            while (elapse_time < flightDuration)
            {
                Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

                elapse_time += Time.deltaTime;

                yield return null;
            }
        }        
    }
}