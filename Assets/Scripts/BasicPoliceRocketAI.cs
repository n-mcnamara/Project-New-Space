using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPoliceRocketAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChaseTarget,
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


    private Quaternion _lookRotation;
    private Vector3 _direction;
    private ParticleSystem engineParticleSystem;

    public GameObject bulletPrefab;
    private Vector2 GunRotation;
    public float delayInSeconds;
    public float bulletSpeed;
    private bool canShoot = true;

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
        if (health < 1)
        {
            Destroy(gameObject);
        }
        switch (state)
        {
            default:
            case State.Roaming:

                //while((transform.up.normalized.x - roamPosition.normalized.x) > 0.1f || (transform.up.normalized.x - roamPosition.normalized.x) < -0.1f || (transform.up.normalized.y - roamPosition.normalized.y) < -0.1f || (transform.up.normalized.y - roamPosition.normalized.y) > 0.1f)
                _direction = (roamPosition - transform.position).normalized;

                //create the rotation we need to be in to look at the target
                //_lookRotation = Quaternion.LookRotation(_direction);

                transform.up = Vector3.MoveTowards(transform.up, _direction, rotateSpeed * Time.deltaTime);
                //transform.Rotate(Vector3.forward * 1 * -1, Space.Self);

                //transform.rotation = Quaternion.Lerp(transform.rotation, _lookRotation, Time.deltaTime * 1);

                if (transform.up == _direction)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.up.x * rocketForce, transform.up.y * rocketForce), ForceMode2D.Impulse);
                }
                else
                {
                    engineParticleSystem.Play();
                }

                //transform.position = Vector3.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, roamPosition) < 5f)
                {
                    roamPosition = GetRoamingPosition();
                }
                break;

            case State.ChaseTarget:

                _direction = (player.transform.position - transform.position).normalized;
                transform.up = Vector3.MoveTowards(transform.up, _direction, rotateSpeed * Time.deltaTime);

                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.up.x * rocketForce, transform.up.y * rocketForce), ForceMode2D.Impulse);

                if (transform.up == _direction)
                {
                    Shoot(1);
                }

                break;
        }



    }

    public void ChangeToChase()
    {
        SpriteRenderer[] d = gameObject.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < d.Length; i++)
        {
            if (d[i].gameObject.name == "MinimapIcon")
            {
                d[i].color = Color.red;
            }
        }
        state = State.ChaseTarget;
        rocketForce = 1.2f;
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
            Vector3 roamPosition = startingPosition + (randomDirection * Random.Range(30f, 150f));
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

    private void Shoot(int num)
    {
        if (canShoot)
        {
            //Bounds atmosphereBounds = transform.GetComponent<EdgeCollider2D>().bounds;
            GameObject bullet = Instantiate(bulletPrefab, transform.position + (transform.right * 2), Quaternion.identity);
            GameObject bullet2 = Instantiate(bulletPrefab, transform.position + (transform.right * 2 * -1), Quaternion.identity);

            BasicLaser bulletScript = bullet.GetComponent<BasicLaser>();
            BasicLaser bulletScript2 = bullet2.GetComponent<BasicLaser>();

            GunRotation = new Vector2(transform.up.x, transform.up.y);
            bulletScript.velocity = GunRotation * bulletSpeed;
            bulletScript2.velocity = GunRotation * bulletSpeed;

            bulletScript.AttackingShip = gameObject;
            bulletScript2.AttackingShip = gameObject;

            // Player1.transform.Translate(new Vector3(Player1.transform.position.x - (GunRotation.x ), Player1.transform.position.y - (GunRotation.y), Player1.transform.position.z));
            canShoot = false;
            bullet.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(GunRotation.y, GunRotation.x) * Mathf.Rad2Deg * 90);
            bullet2.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(GunRotation.y, GunRotation.x) * Mathf.Rad2Deg * 90 );
            //bullet.transform.Rotate(0.0f, 0.0f, transform.rotation.z, Space.World);
            //bullet.transform.Rotate(0.0f, 0.0f, transform.rotation.z, Space.World);


            StartCoroutine(ShootDelay());

            //audioManager.Play("Shoot");S
            //bullet.transform.Rotate(0.0f, 0.0f, transform.rotation.z, Space.World);

            Destroy(bullet, 3f);
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(roamPosition, 2);

    }
}
