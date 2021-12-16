using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehavior : MonoBehaviour
{

    public Rigidbody2D Rb;

    public float travelSpeed = 1;

    [SerializeField]
    private float damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        Rb.velocity = transform.right * travelSpeed;

        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject, 0);
    }

    public float GetDamage()
    {
        return damage;
    }
}
