using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBasic : MonoBehaviour
{
    public GameObject planet;
    public float Speed;
    public Vector3 surfaceNormal;

    float direction;
    float distance = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, planet.transform.position - transform.position);
        surfaceNormal = hit.normal;
        if(distance > 0.1)
        {
            tangentMovement();
        }
    }

    void tangentMovement()
    {
        direction = Random.Range(-1, 1.1f);
        distance =  

        Mathf.Round(direction);
        Debug.Log(Mathf.Round(-0.75f));
        Debug.Log(Mathf.Round(-0.005f));

        while (distance > 0.1 && direction != 0)
        {
            Vector3 movement = direction * transform.right;

            Vector3 temp = Vector3.Cross(surfaceNormal, movement);
            movement = Vector3.Cross(temp, surfaceNormal);
            movement.Normalize();

            //Debug.DrawRay(transform.position, movement);

            gameObject.GetComponent<Rigidbody2D>().AddForce(movement * Speed);
            distance -= 1 * Time.deltaTime;
            Debug.Log(distance);
        }
        distance = 1;
    }

    void tangentMovement1()
    {
        direction = Random.Range(-1, 1.1f);
        distance = Random.Range(1, 10);

        Mathf.Round(direction);
        Debug.Log(Mathf.Round(-0.75f));
        Debug.Log(Mathf.Round(-0.005f));

        while (distance > 0.1 && direction != 0)
        {
            Vector3 movement = direction * transform.right;

            Vector3 temp = Vector3.Cross(surfaceNormal, movement);
            movement = Vector3.Cross(temp, surfaceNormal);
            movement.Normalize();

            //Debug.DrawRay(transform.position, movement);

            gameObject.GetComponent<Rigidbody2D>().AddForce(movement * Speed);
            distance -= 1 * Time.deltaTime;
            Debug.Log(distance);
        }
        distance = 1;
    }
}
