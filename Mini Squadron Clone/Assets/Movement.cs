using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D Rb;

    private float horizontal;
    private float vertical;

    private Vector2 joystickVector;

    public float rotationSpeed = 1;
    //public float rotationSpeedFactor = 1;
    public float velocity = 1;

    public float controllerDeadzone = 0;

    bool flipSprite = false;

    public Animator animator;

    private int faceDirection = 0;//left -1 or right 1

    public float hitRotationAmount = 0;

    public GameObject pointerRotator;
    public GameObject pointer;

    public Quaternion targetQuat;

    private bool giveUserControl = true;



    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }


    Vector2 velo = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        joystickVector = new Vector2(horizontal, vertical).normalized;

        //pointerRotator.transform.localPosition = targetRotation * 0.3f;


        pointerRotator.transform.position = transform.position;

        if (giveUserControl)
        targetQuat = Quaternion.Euler(0, 0, Mathf.Atan2(joystickVector.y, joystickVector.x) * Mathf.Rad2Deg);

        print(targetQuat.eulerAngles);

        if (joystickVector != Vector2.zero)
        pointerRotator.transform.rotation = Quaternion.Slerp(pointerRotator.transform.rotation, targetQuat, 0.1f);



        Vector2 pointerDamp = Vector2.SmoothDamp(pointer.transform.localPosition, new Vector2(joystickVector.magnitude * 0.3f, 0), ref velo, 0.05f);

        pointer.transform.localPosition = pointerDamp; //new Vector2(targetRotation.magnitude * 0.3f, 0);


        //print(horizontal + " " + vertical);

        //print(transform.rotation.eulerAngles);

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






    }

    bool flag = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
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


        }

        if (collision.CompareTag("StageBoundary"))
        {
            if (flag)
            {
                flag = false;
                Vector2 wallClosestPt = collision.ClosestPoint(transform.position);

                Vector2 temp = -(wallClosestPt - (Vector2)transform.position).normalized;

                StartCoroutine("tempCourou", temp);
            }
            

        }
        
    }

    IEnumerator tempCourou(Vector2 temp)
    {

        giveUserControl = false;

        yield return new WaitForEndOfFrame();

        targetQuat = Quaternion.Euler(0, 0, Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg);

        yield return new WaitForSeconds(0.3f);

        giveUserControl = true;

        flag = true;
    }


    Vector2 currentVelocity = Vector2.zero;
    public float smoothTime = 0.1f;
    public float maxSpeed = 1;

    private void FixedUpdate()
    {
        if (joystickVector.magnitude > controllerDeadzone)//deadzone not effect cause normalized
        {

            //Vector2 veloz = Rb.velocity;
            //Vector2 interpRotation = Vector2.Lerp(transform.right, targetRotation, rotationSpeed * Time.deltaTime).normalized;

            //float temp = 1 / (interpRotation - targetRotation).magnitude;
            //rotationSpeed = temp * rotationSpeedFactor;

            //Vector2 interpRotation = Vector2.SmoothDamp(transform.right, targetRotation, ref currentVelocity, 0.3f);
            //transform.right = interpRotation;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetQuat, 0.06f);


            

            //Rb.MoveRotation( Mathf.Atan2(interpRotation.y, interpRotation.x) * Mathf.Rad2Deg);




            /*Vector2 slopeVect = (targetRotation - (Vector2)transform.right);

            slopeVect = Vector2.SmoothDamp(transform.right, slopeVect, ref currentVelocity, smoothTime, maxSpeed);


            float rotation = Mathf.Rad2Deg * Mathf.Atan2(slopeVect.y, slopeVect.x);

            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
*/



        }


        Rb.velocity = transform.right * velocity;
        
        

    }



}
