using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField]
    private float baseHealth;
    private float healthPoints;


    // Start is called before the first frame update
    void Start()
    {
        healthPoints = baseHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            bulletBehavior bulletScript = collision.GetComponent<bulletBehavior>();

            healthPoints -= bulletScript.GetDamage();

            if (healthPoints <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                //implement thing for adjusting smoke based on health
            }
        }

    }






}
