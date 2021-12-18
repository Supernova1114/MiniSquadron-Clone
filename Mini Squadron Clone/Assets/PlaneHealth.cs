using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneHealth : MonoBehaviour
{

    //Health
    [SerializeField]
    private int baseHealth;
    private int healthPoints;
    private double healthPercentage;
    

    //Smoke
    [SerializeField]
    ParticleSystem[] smokeList = new ParticleSystem[4]; //A list of smokeTypes


    // Start is called before the first frame update
    void Start()
    {
        healthPoints = baseHealth;

        for (int i=0; i<smokeList.Length; i++)
        {
            smokeList[i].Stop();
        }

        smokeList[0].Play();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("PlayerBullet"))
        {
            bulletBehavior bulletScript = collision.GetComponent<bulletBehavior>();

            healthPoints -= bulletScript.GetDamage();

            healthPercentage = healthPoints / (double)(baseHealth);

            print("HealthPercent: " + healthPercentage);

            if (healthPercentage <= 0.75 && healthPercentage > 0.5)
            {
                SwitchSmoke(0, 1);
            }
            else if (healthPercentage <= 0.5 && healthPercentage > 0.25)
            {
                SwitchSmoke(1, 2);
            }
            else if (healthPercentage <= 0.25 && healthPercentage > 0)
            {
                SwitchSmoke(2, 3);
            }
            else if (healthPoints <= 0)
            {
                // spriteRenderer.enabled = false;
                // planeCollider.enabled = false;

                // particleSmoke.Stop();
                for (int i=0; i<smokeList.Length; i++)
                {
                    smokeList[i].Stop();
                }

                SendMessage("Fall");
                
            }
           
        }

    }


    private void SwitchSmoke(int offIndex, int onIndex)
    {
        smokeList[offIndex].Stop();
        smokeList[onIndex].Play();
    }






}
