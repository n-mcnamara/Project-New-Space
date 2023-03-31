using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLaser : MonoBehaviour
{
    public Vector2 velocity = new Vector2(0.0f, 0.0f);
    public GameObject AttackingShip;

    void Update()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;

        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition, newPosition);

        foreach (RaycastHit2D hit in hits)
        {
            GameObject other = hit.collider.gameObject;
            if (other != AttackingShip)
            {
                if (other.CompareTag("EnemyShip") && AttackingShip.tag != "EnemyShip")
                {
                    Destroy(gameObject);
                    Destroy(other);
                    //player1Stats.TakeDamage();
                    break;
                }
                else if(other.CompareTag("Rocket"))
                {
                    Destroy(gameObject);
                    other.transform.Find("ExplosionParticleSystem").GetComponent<ParticleSystem>().Play();
                    //player1Stats.TakeDamage();
                    break;
                }
                else if(other.CompareTag("FriendlyShip"))
                {
                    Destroy(gameObject);
                    other.GetComponent<BasicCivRocketAI>().ChangeToFlee();
                    other.GetComponent<BasicCivRocketAI>().health--;

                    break;
                }
                else if (other.CompareTag("PoliceShip") && AttackingShip.tag != "PoliceShip")
                {
                    Destroy(gameObject);
                    other.GetComponent<BasicPoliceRocketAI>().ChangeToChase();
                    other.GetComponent<BasicPoliceRocketAI>().health--;
                    break;
                }
            }

            if (other.CompareTag("Ground"))
            {
                Destroy(gameObject);
                break;
            }
        }
        transform.position = newPosition;
    }
}
