using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rocketRB;
    public Camera mainCamera;

    public float rotateSpeed;
    public float rocketForce;
    public float rocketTopSpeed;
    public bool isGrounded;
    public float rocketFuel = 100;
    public float rocketFuelMax;
    public float rocketFuelConsumption = 1;
    public float breakDrag;

    public GameObject bulletPrefab;
    private Vector2 GunRotation;
    public float delayInSeconds;
    public float bulletSpeed;
    private bool canShoot = true;

    private bool a = true;
    public bool autoLand = false;
    private bool landDelay = true;
    public ParticleSystem explosionParticleSystem;

    public Canvas canvas;
    public Text autoLandText;
    public Text fuelIndicator;
    private CameraFollow cm;
    private ParticleSystem engineParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        cm = mainCamera.GetComponent<CameraFollow>();
        engineParticleSystem = transform.Find("EngineParticleSystem").GetComponent<ParticleSystem>();

        rocketFuel = rocketFuelMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
        if (player != null)
        {
            autoLandText.gameObject.SetActive(true);
            if (Input.GetKeyDown("r"))
            {
                //player leaves rocket
                LeaveRocket();
            }
            if (Input.GetKeyDown("p"))
            {
                //player leaves rocket
                Shoot();
            }
            RocketMovement();
            AutoLand();
            //limits speed
            rocketRB.velocity = Vector2.ClampMagnitude(gameObject.GetComponent<Rigidbody2D>().velocity, rocketTopSpeed);

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
        }
        else
        {
            return;
        }
    }

    void LeaveRocket()
    {
        autoLandText.gameObject.SetActive(false);
        player.transform.position = transform.position + (transform.right * 3);
           
        cm.player = player;
        cm.zoomIn = true;
        cm.zoomDone = false;

        player.transform.parent = null;
        player.SetActive(true);
        player = null;
    }

    void RocketMovement()
    {
        if (isGrounded == false)
        {
            float rotateInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.forward * rotateSpeed * -rotateInput, Space.Self);


            if(Input.GetButtonDown("Jump") && cm.zoomDone && rocketFuel > 0)
            {
                engineParticleSystem.Play();
            }

            if(Input.GetButtonUp("Jump"))
            { 
                engineParticleSystem.Stop();
            }
        }

        if(isGrounded == true && rocketFuel < rocketFuelMax)
        {
            rocketFuel += (1 * Time.deltaTime);
            fuelIndicator.text = "Fuel: " + Mathf.Round(rocketFuel).ToString();
        }
            
        if (Input.GetButton("Jump") && cm.zoomDone && rocketFuel > 0)
        {
            rocketRB.AddForce(new Vector2(transform.up.x * rocketForce, transform.up.y * rocketForce), ForceMode2D.Impulse);
            rocketFuel -= (1 * Time.deltaTime * rocketFuelConsumption);
            fuelIndicator.text = "Fuel: " + Mathf.Round(rocketFuel).ToString();
        }

        if (Input.GetButton("left shift") && cm.zoomDone)
        {
            //rocketRB.AddForce(new Vector2(-transform.up.x * rocketForce, -transform.up.y * rocketForce), ForceMode2D.Impulse);

            rocketRB.velocity = new Vector2(Mathf.Lerp(rocketRB.velocity.x, 0, 1.5f * Time.deltaTime), Mathf.Lerp(rocketRB.velocity.y, 0, 1.5f * Time.deltaTime));
        }
        else
        {
            //rocketRB.drag = 1;
        }

    }

    void AutoLand()
    {
        if(Input.GetKeyDown("q") && autoLand == false && landDelay)
        {
            autoLand = true;
            autoLandText.text = "AUTO LANDING ENGAGED";
            autoLandText.gameObject.SetActive(true);
            StartCoroutine(hideText());
        }
        else if(Input.GetKeyDown("q") && autoLand == true && landDelay)
        {
            autoLand = false; 
            autoLandText.text = "AUTO LANDING DISENGAGED";
            autoLandText.gameObject.SetActive(true);
            StartCoroutine(hideText());
        }
    }

    private void Shoot()
    {
        if (canShoot)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<SpriteRenderer>().color = Color.white;
            BasicLaser bulletScript = bullet.GetComponent<BasicLaser>();
            GunRotation = new Vector2(transform.up.x, transform.up.y);
            bulletScript.velocity = GunRotation * bulletSpeed;

            bulletScript.AttackingShip = gameObject;
            // Player1.transform.Translate(new Vector3(Player1.transform.position.x - (GunRotation.x ), Player1.transform.position.y - (GunRotation.y), Player1.transform.position.z));
            canShoot = false;
            StartCoroutine(ShootDelay());

            //audioManager.Play("Shoot");
            bullet.transform.Rotate(0.0f, 0.0f, transform.rotation.z, Space.World);
            Destroy(bullet, 3f);
        }
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" && player != null && (rocketRB.velocity.magnitude > 7 || isGrounded == false))
        {
            transform.Find("ExplosionParticleSystem").GetComponent<ParticleSystem>().Play();
            LeaveRocket();
        }

        if (collision.gameObject.tag == "Explosive")
        {
            Debug.Log("HIT");
            transform.Find("ExplosionParticleSystem").GetComponent<ParticleSystem>().Play();
            Destroy(collision.gameObject);
        }
    }

    IEnumerator hideText()
    {
        landDelay = false;
        yield return new WaitForSeconds(2);
        autoLandText.gameObject.SetActive(false);
        landDelay = true;
    }
}
