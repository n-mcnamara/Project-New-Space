using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCivRocketAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Flee,
        Evade,
        SeekPolice,
    }

    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private Vector3 randomDirection;
    public float targetRange;

    public int health;

    private State state;
    public GameObject player;
    public float rocketForce;
    public float rotateSpeed;

    float rnd;

    private Quaternion _lookRotation;
    private Vector3 _direction;
    private ParticleSystem engineParticleSystem;

    public GameObject minePrefab;

    private GameObject nearPoliceCar;


    public GameObject bulletPrefab;
    private Vector2 GunRotation;
    public float delayInSeconds;
    public float bulletSpeed;
    private bool canDrop = true;

    private void Awake()
    {
        state = State.Roaming;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        roamPosition = GetRoamingPosition();
        engineParticleSystem = transform.Find("EngineParticleSystem").GetComponent<ParticleSystem>();

        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if(health < 1)
        {
            Destroy(gameObject);
        }

        switch (state)
        {
            default:
            case State.Roaming:

                _direction = (roamPosition - transform.position).normalized;


                transform.up = Vector3.MoveTowards(transform.up, _direction, rotateSpeed * Time.deltaTime);

                if (transform.up == _direction)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.up.x * rocketForce, transform.up.y * rocketForce), ForceMode2D.Impulse);
                }
                else
                {
                    engineParticleSystem.Play();
                }

                if (Vector3.Distance(transform.position, roamPosition) < 20f)
                {
                    roamPosition = GetRoamingPosition();
                    if(roamPosition == new Vector3(0, 0, roamPosition.z))
                    {
                        Debug.LogError("Spaceship couldnt find locaiton to go");
                    }
                }
                break;

            case State.Flee:
                _direction = (transform.position - player.transform.position).normalized;
                transform.up = Vector3.MoveTowards(transform.up, _direction, rotateSpeed * Time.deltaTime);

                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.up.x * rocketForce, transform.up.y * rocketForce), ForceMode2D.Impulse);

                if (transform.up == _direction)
                {
                    // state = State.Evade;
                    DropMine();
                    rnd = Random.Range(-90f, 90f);
                    Collider2D[] circle = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 222);
                    for (int i = 0; i < circle.Length; i++)
                    {
                        if (circle[i].tag == "PoliceShip")
                        {
                            circle[i].GetComponent<BasicPoliceRocketAI>().ChangeToChase();
                        }
                    }
                }

                break;
            case State.SeekPolice:
              
                _direction = (nearPoliceCar.transform.position - transform.position).normalized;
                transform.up = Vector3.MoveTowards(transform.up, _direction, rotateSpeed * Time.deltaTime);

                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.up.x * rocketForce, transform.up.y * rocketForce), ForceMode2D.Impulse);

                if (Vector3.Distance(transform.position, nearPoliceCar.transform.position) < 90f)
                {
                    state = State.Flee;
                    nearPoliceCar.GetComponent<BasicPoliceRocketAI>().ChangeToChase();
                }

                break;
        }
    }

    public void ChangeToFlee()
    {
        bool checkForFarPolice = true;
        state = State.Flee;
        //rocketForce = 1.75f;
        Collider2D[] circle = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 222);
        for (int i = 0; i < circle.Length; i++)
        {
            if (circle[i].tag == "PoliceShip")
            {
                circle[i].GetComponent<BasicPoliceRocketAI>().ChangeToChase();
                //break from method
                checkForFarPolice = false;
                break;
            }
        }

        if(checkForFarPolice)
        {
            Collider2D[] circle2 = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 500);
            for (int i = 0; i < circle2.Length; i++)
            {
                if (circle2[i].tag == "PoliceShip")
                {
                    nearPoliceCar = circle2[i].gameObject;
                    state = State.SeekPolice;
                    break;
                }
            }
        }
    }

    Vector3 GetRoamingPosition()
    {
        int numberOfTrys = 50;
        int j = 0;
        bool foundSpot;

        while (j < numberOfTrys)
        {
            foundSpot = false;
            randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 roamPosition = transform.position + (randomDirection * Random.Range(30f, 800f));
            //to prevent flying into atmoshpere, doesnt work as they still do
            //Collider2D[] circle = Physics2D.OverlapCircleAll(new Vector2(roamPosition.x, roamPosition.y), 2);

            RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, roamPosition);

            foreach (RaycastHit2D hit in hits)
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.tag == "atmosphere" || hitObject.tag == "Ground")
                {
                    foundSpot = true;
                }
            }

            if (!foundSpot)
            {
                return roamPosition;
            }
            j++;
        }
        return new Vector2(0, 0);
    }

    void DropMine()
    {
        if(canDrop)
        {
            Vector3 spawnPos = new Vector3(transform.position.x - 5, transform.position.y - 5, transform.position.z);
            GameObject mine = Instantiate(minePrefab, spawnPos, Quaternion.identity);
            canDrop = false;
            GunRotation = new Vector2(-transform.up.x, -transform.up.y);
            mine.GetComponent<Rigidbody2D>().velocity = GunRotation * bulletSpeed;

            StartCoroutine(MineDelay());

            
            //bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(GunRotation.y, GunRotation.x) * Mathf.Rad2Deg);

            Destroy(mine, 4f);
        }


    }

    IEnumerator MineDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canDrop = true;
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, 500);

     
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(roamPosition, 2);
    }
}
