using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //if i use collision then the ground check automatically flips the object but also makes it easier to land as theres a flat collision on bottom of ship
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && Player.GetComponent<PlayerController>())
        {
            Player.GetComponent<PlayerController>().isGrounded = true;
        }
        else if (collision.collider.tag == "Ground" && Player.GetComponent<RocketController>())
        {
            Player.GetComponent<RocketController>().isGrounded = true;
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && Player.GetComponent<PlayerController>())
        {
            Player.GetComponent<PlayerController>().isGrounded = true;
        }
        else if (collision.tag == "Ground" && Player.GetComponent<RocketController>())
        {
            Player.GetComponent<RocketController>().isGrounded = true;
        }
    }

    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && Player.GetComponent<PlayerController>())
        {
            Player.GetComponent<PlayerController>().isGrounded = false;
        }
        else if (collision.collider.tag == "Ground" && Player.GetComponent<RocketController>())
        {
            Player.GetComponent<RocketController>().isGrounded = false;
        }
    }
    */

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" && Player.GetComponent<PlayerController>())
        {
            Player.GetComponent<PlayerController>().isGrounded = false;
        }
        else if (collision.tag == "Ground" && Player.GetComponent<RocketController>())
        {
            Player.GetComponent<RocketController>().isGrounded = false;
        }
    }
}
