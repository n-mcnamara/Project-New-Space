using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterExitAtmosphere : MonoBehaviour
{
    public bool GravityOn;
    public GameObject player;
    public GameObject rocket;

    public Color atmosphereColor;

    public float maxGravityDist;
    public float planetRadius;

    private Bounds atmosphereBounds;
    private Bounds planetBounds;
    // Start is called before the first frame update
    void Start()
    {
        atmosphereBounds = transform.GetComponent<CircleCollider2D>().bounds;
        planetBounds = transform.parent.GetComponent<CircleCollider2D>().bounds;

        maxGravityDist = atmosphereBounds.extents.x;
        planetRadius = planetBounds.extents.x;

        atmosphereColor = gameObject.GetComponent<SpriteRenderer>().color;
        atmosphereColor.a = 1;
        //retrieves variables from attached planet to use for colour change
        //doesnt work as always becomes zero. have to repeat code form coreplanet which is a shame. [Bug]
        //maxGravityDist = transform.parent.GetComponent<CorePlanet>().maxGravityDist;
        //planetRadius = transform.parent.GetComponent<CorePlanet>().planetRadius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GravityOn && rocket != null)
        {
            float dist = Vector2.Distance(transform.parent.position, rocket.GetComponent<RocketController>().transform.position);
            //the number 50 is the distance from the player and planet when they are on the ground. shouldnt be set as 50 as it will differ for each planet
            //use the 50 in equation to make opacity only change when they are higher than the ground rather than centre of planet as then its partially dark as soon as game starts.
            gameObject.GetComponent<SpriteRenderer>().color = new Color(atmosphereColor.r, atmosphereColor.g, atmosphereColor.b, atmosphereColor.a * (1.0f - (dist - planetRadius) / (maxGravityDist - planetRadius)));
        }
        else
        {
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //fix: remove first 2 if statements
        if(collision.tag == "Player")
        {
            GravityOn = true;
            player = collision.gameObject;
            //GetComponentInParent<CorePlanet>().player = player;
            GetComponentInParent<CorePlanet>().objectsOnPlanet.Add(collision.gameObject);
            player.GetComponent<Rigidbody2D>().drag = 1;
        }
        else if (collision.tag == "Rocket" /*&& collision.transform.childCount > 4*/)
        {
            GravityOn = true;
            rocket = collision.gameObject;
            GetComponentInParent<CorePlanet>().objectsOnPlanet.Add(collision.gameObject);
            rocket.GetComponent<Rigidbody2D>().drag = 0;
        }
        else if(collision.tag != "Ground" && collision.gameObject.GetComponent<Rigidbody2D>() && collision.tag != "FriendlyShip" && collision.tag != "PoliceShip" && collision.tag != "EnemyShip")
        {
            GravityOn = true;
            GetComponentInParent<CorePlanet>().objectsOnPlanet.Add(collision.gameObject);
            collision.GetComponent<Rigidbody2D>().drag = 1;
        }
        else
        {
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //onyl occurs if planet jumps out of atmoshpere which doesnt happen
        if (collision.tag == "Player")
        {
            //PlayerExit(collision.gameObject);
            //GetComponentInParent<CorePlanet>().player = null;4
        }
        else if (collision.tag == "Rocket")
        {
            GravityOn = false;
            rocket.GetComponent<Rigidbody2D>().drag = 0;
            rocket = null;
            //GetComponentInParent<CorePlanet>().rocket = null;
            GetComponentInParent<CorePlanet>().objectsOnPlanet.Remove(collision.gameObject);

            //player.GetComponent<Rigidbody2D>().drag = 0;
            //player.GetComponent<PlayerController>().planet = null;
            GetComponentInParent<CorePlanet>().objectsOnPlanet.Remove(player);
            player = null;
            GetComponentInParent<CorePlanet>().player = null;
        }
        else if (collision.tag != "Ground" && collision.gameObject.GetComponent<Rigidbody2D>() && collision.tag != "FriendlyShip" && collision.tag != "PoliceShip" && collision.tag != "EnemyShip")
        {
            GetComponentInParent<CorePlanet>().objectsOnPlanet.Remove(collision.gameObject);
            collision.GetComponent<Rigidbody2D>().drag = 0;
        }
        else
        {
            return;
        }
    }

    public void PlayerExit(GameObject gameobject)
    {
        player.GetComponent<Rigidbody2D>().drag = 0;
        player = null;
        GetComponentInParent<CorePlanet>().objectsOnPlanet.Remove(gameobject);
    }

    public void GetBounds()
    {
        atmosphereBounds = transform.GetComponent<CircleCollider2D>().bounds;
        planetBounds = transform.parent.GetComponent<CircleCollider2D>().bounds;

        maxGravityDist = atmosphereBounds.extents.x;
        planetRadius = planetBounds.extents.x;
    }
}
