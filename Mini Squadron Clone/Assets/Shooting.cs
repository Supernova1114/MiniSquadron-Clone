using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletObj;
    public Transform gunPos;

    private float fire1;

    float waitTime = 5.0f;
    float timeStamp = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Fire1") == 1)
        {
            if (flag)
            {
                flag = false;
                StartCoroutine("Fire");
            }
            
        }




    }


    bool flag = true;
    IEnumerator Fire()
    {
        Instantiate(bulletObj, gunPos.position, transform.rotation);

        yield return new WaitForSeconds(0.3f);

        flag = true;
    }

}
