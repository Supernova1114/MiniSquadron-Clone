using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D Rb;

    //private float horizontal;
    //private float vertical;

    private Vector2 targetRotation = Vector2.zero;

    public float rotationSpeed = 1;
    public float velocity = 1;

    

    //public float controllerDeadzone = 0;

    //bool flipSprite = false;

    public Animator animator;

    public Transform player;

    private bool tickAI = true;

    //public Collider2D behaviorCollider;

    private float collisionNum = 0;

    private int faceDirection = 0;//left -1 or right 1

    public float nonTrackingRotation = 1;

    public bool rotateTowardsPlayer = false;

    public float unitCircleInterp = 0;

    public float hitRotationAmount = 0;

    public float healthPoints = 100;

    public ParticleSystem particleSmoke;
    public ParticleSystem particleExplode;

    public GameObject planeBody;
    public Collider2D planeCollider;

    public SpriteRenderer spriteRenderer;

    public GameObject enemyBullet;

    public LayerMask detectionLayer;
    public float detectionDistance;
    public float detectionInterval;

    public float shotCooldown;
    private float currentShotCooldown = 0;




    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        planeCollider = GetComponent<Collider2D>();
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //print(horizontal + " " + vertical);

        //print(transform.rotation.eulerAngles);

        if (rotateTowardsPlayer)
        {
            targetRotation = (player.position - transform.position).normalized;
        }

        if (transform.rotation.eulerAngles.z > 315 || transform.rotation.eulerAngles.z < 45)
        {
            animator.SetBool("Rotation", false);
        }

        if (transform.rotation.eulerAngles.z > 135 && transform.rotation.eulerAngles.z < 225)
        {
            animator.SetBool("Rotation", true);
        }


        if (transform.rotation.eulerAngles.z > 270 || transform.rotation.eulerAngles.z < 90)
        {
            faceDirection = 1;
        }

        if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
        {
            faceDirection = -1;
        }


        if (tickAI)
        {
            tickAI = false;
            StartCoroutine("MoveAI");

        }

        //print(faceDirection + " " + transform.rotation.eulerAngles.z);


    }


    private bool tryUnitRotate = false;
    IEnumerator MoveAI()
    {
        int random = Random.Range(1,3);//1 or 2

        yield return new WaitForSeconds(random);

        if ((player.position - transform.position).magnitude > 1 || tryUnitRotate == false)//if greater than 1 away from player
        {
            StartCoroutine("NormalMovement");
        }
        else
        {
            StartCoroutine("CloseProximityMovement");
        }

        

    }

    
    IEnumerator NormalMovement()
    {

        int random = Random.Range(1, 3);//1 being up 2 being down

        switch (faceDirection)
        {
            case 1:
                if (random == 1)
                    targetRotation = new Vector2(1, nonTrackingRotation).normalized;
                else
                    targetRotation = new Vector2(1, -nonTrackingRotation).normalized;
                break;
            case -1:
                if (random == 1)
                    targetRotation = new Vector2(-1, nonTrackingRotation).normalized;
                else
                    targetRotation = new Vector2(-1, -nonTrackingRotation).normalized;
                break;
        }

        yield return new WaitForSeconds(5);

        rotateTowardsPlayer = true;

        yield return new WaitForSeconds(5);

        rotateTowardsPlayer = false;

        tryUnitRotate = true;

        tickAI = true;
    }

    IEnumerator CloseProximityMovement()
    {
        if (tryUnitRotate)
        {

            rotateTowardsPlayer = false;

            int startRot = (int)transform.rotation.eulerAngles.z;
            int rand = Random.Range(1, 4);

            for (int i = startRot; i < startRot + 360 * rand; i += 5)
            {
                targetRotation = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad)).normalized;
                yield return new WaitForSeconds(unitCircleInterp);

            }
            tryUnitRotate = false;
        }

        tickAI = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            bulletBehavior bulletScript = collision.GetComponent<bulletBehavior>();

            healthPoints -= bulletScript.GetDamage();

            print(healthPoints);
            

            Vector2 relativeVect = (collision.transform.position - transform.position).normalized;
            
            if (relativeVect.y > 0)
            {
                if (faceDirection == 1)
                    Rb.MoveRotation(transform.rotation.eulerAngles.z - hitRotationAmount);
                else
                    Rb.MoveRotation(transform.rotation.eulerAngles.z + hitRotationAmount);

            }
            else
            {
                if (faceDirection == 1)
                    Rb.MoveRotation(transform.rotation.eulerAngles.z + hitRotationAmount);
                else
                    Rb.MoveRotation(transform.rotation.eulerAngles.z - hitRotationAmount);
            }

            if (healthPoints <= 0)
            {

                spriteRenderer.enabled = false;
                planeCollider.enabled = false;

                particleSmoke.Stop();

                particleExplode.Play();

                Destroy(this.gameObject, 3);
            }
        }
    }


    private void FixedUpdate()
    {
        if (targetRotation != Vector2.zero)
        {
            Vector2 interpRotation = Vector2.Lerp(transform.right, targetRotation, rotationSpeed * Time.deltaTime);
            Rb.MoveRotation(Mathf.Atan2(interpRotation.y, interpRotation.x) * Mathf.Rad2Deg);
        }


        Rb.velocity = transform.right * velocity;

        if (Time.time > currentShotCooldown)
        {
            //Detect player and shoot
            //6 temp layer for player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, detectionDistance, detectionLayer.value);

            //FIXME this is also broken wtf asjdljasldjla
            //FIXME need a cooldown for shoot lmao
            if (hit.collider != null)
            {
                Shoot();
                currentShotCooldown = shotCooldown + Time.time;

            }
            else
            {
                currentShotCooldown = detectionInterval + Time.time;

            }


        }





    }


    private void Shoot()
    {
        Instantiate(enemyBullet, transform);
    }

}
