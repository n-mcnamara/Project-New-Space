using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isGrounded = false;
    public GameObject planet;
    public Camera mainCamera;

    public float jumpForce;
    public Rigidbody2D playerRB;

    public Vector3 surfaceNormal;

    public float Speed;
    public int interactRadius;
    public LayerMask rockets;

    private bool a = true;

    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "atmosphere")
        {
            planet = collision.transform.parent.gameObject;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown("m") && a)
        {
            Minimap.MinimapWindow.Show();
            a = false;
        }
        else if (Input.GetKeyDown("m") && !a)
        {
            Minimap.MinimapWindow.Hide();
            a = true;
        }

        if (planet != null)
        {
            Interact();
            RaycastHit2D hit = Physics2D.Raycast(transform.position, planet.transform.position - transform.position);

            Debug.DrawRay(transform.position, planet.transform.position - transform.position);
            surfaceNormal = hit.normal;
            Debug.DrawRay(transform.position, surfaceNormal * 10);

            Jump(surfaceNormal);

            tangentMovement();
            //float inputH = Input.GetAxisRaw("Horizontal");
            //playerRB.AddForce(inputH * transform.right * Speed);
        }
        else
        {
            return;
        }

    }

    void tangentMovement()
    {
        float inputH = Input.GetAxis("Horizontal");
        Vector3 movement = inputH * transform.right;
  
        Vector3 temp = Vector3.Cross(surfaceNormal, movement);
        movement = Vector3.Cross(temp, surfaceNormal);
        movement.Normalize();

        Debug.DrawRay(transform.position, movement);
        

        if (Input.GetKey("a") || Input.GetKey("d") /*&& mainCamera.GetComponent<CameraFollow>().zoomDone*/)
        {
            playerRB.AddForce(movement * Speed);
        }
    }

    void Interact()
    {
        if(Input.GetKeyUp("e"))
        {
            Collider2D[] circle = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), interactRadius, rockets);
            for (int i = 0; i < circle.Length; i++)
            {
                if (circle[i].tag == "Rocket")
                {
                    //get in rocket
                    circle[i].GetComponent<RocketController>().player = gameObject;
                    planet.GetComponent<CorePlanet>().rocket = circle[i].gameObject;
                    planet.GetComponentInChildren<EnterExitAtmosphere>().rocket = circle[i].gameObject;
                    planet.GetComponentInChildren<EnterExitAtmosphere>().PlayerExit(gameObject);
                    //currently no reason to do this
                    transform.parent = circle[i].transform;
                    mainCamera.GetComponent<CameraFollow>().player = circle[i].gameObject;
                    mainCamera.GetComponent<CameraFollow>().zoomOut = true;
                    mainCamera.GetComponent<CameraFollow>().zoomDone = false;

                    gameObject.SetActive(false);
                }
            }
        }
    }

    void Jump(Vector3 surfaceNormal)
    {
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(surfaceNormal.x * jumpForce, surfaceNormal.y * jumpForce), ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown("s"))
        {
            playerRB.AddForce(new Vector2(surfaceNormal.x * -jumpForce, surfaceNormal.y * -jumpForce), ForceMode2D.Impulse);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}







































/*Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
Vector3 localMovement = transform.TransformDirection(movement.normalized);

Vector3 localMoveRightAxis = Vector3.Cross(localMovement, -surfaceNormal);
Vector3 newDir = Vector3.Cross(localMoveRightAxis, surfaceNormal);*/

/*if (surfaceNormal.y <= 0.01)
{
    movement = -movement;
}*/
//transform.InverseTransformDirection(movement);
//b.AddForce(movement * Time.deltaTime * Speed);
//transform.Translate(movement * Time.deltaTime * Speed, Space.Self);
/*Vector3 temp = Vector3.Cross(surfaceNormal, transform.right);
Vector3 temp1 = Vector3.Cross(temp, surfaceNormal);
movement = new Vector3(temp1.x * movement.x, temp1.y * movement.y, temp1.z);
movement.Normalize();
Debug.Log(movement);
Debug.DrawRay(transform.position, movement);*/
//b.AddForce(movement * Time.deltaTime * Speed);

//transform.Translate(newDir * Time.deltaTime * Speed);


//camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, camera.transform.eulerAngles.y, movement.z);