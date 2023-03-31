using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorePlanet : MonoBehaviour
{

    public GameObject player;
    public GameObject rocket;

    public int gravity;
    public float maxGravityDist;
    public float planetRadius;

    public List<GameObject> objectsOnPlanet;

    private Bounds atmosphereBounds;
    private Bounds planetBounds;

    // Start is called before the first frame update
    void Start()
    {
        atmosphereBounds = transform.Find("Atmosphere").GetComponent<CircleCollider2D>().bounds;
        planetBounds = transform.GetComponent<CircleCollider2D>().bounds;

        maxGravityDist = atmosphereBounds.extents.x;
        planetRadius = planetBounds.extents.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       /* if (transform.GetComponentInChildren<EnterExitAtmosphere>().GravityOn && player != null && rocket != null)
        {
            Gravity(player);
            Gravity(rocket);
        }*/
        //should remove gravity on entirely?? if theres an object on the planet shoudlnt it always have gravity? however disabling gravity would help performance
        if(transform.GetComponentInChildren<EnterExitAtmosphere>().GravityOn && objectsOnPlanet.Count > 0)
        {
            for (int i = 0; i < objectsOnPlanet.Count; i++)
            {
                Gravity(objectsOnPlanet[i]);
            }
        }
        else
        {
            return;
        }
    }


    void Gravity(GameObject mass)
    {
        float dist = Vector2.Distance(transform.position, mass.transform.position);
        //Debug.Log(mass + ": " + dist);
        Vector3 v = transform.position - mass.transform.position;
        if (dist < maxGravityDist)
        {
            mass.GetComponent<Rigidbody2D>().AddForce(v.normalized * (1.0f - dist / maxGravityDist) * gravity);
        }

        //lookAngle = Mathf.Atan2(-surfaceNormal.x, surfaceNormal.y) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);

        //code from super marios glaxy planet traversal video which keeps player rotation relative to planet
        if(mass.tag != "Rocket" || mass.GetComponent<RocketController>().autoLand)
        {
            mass.transform.up = Vector3.MoveTowards(mass.transform.up, -v * gravity, gravity * Time.deltaTime);
        }
    }

    public void GetBounds()
    {
        atmosphereBounds = transform.Find("Atmosphere").GetComponent<CircleCollider2D>().bounds;
        planetBounds = transform.GetComponent<CircleCollider2D>().bounds;

        maxGravityDist = atmosphereBounds.extents.x;
        planetRadius = planetBounds.extents.x;
    }
}
