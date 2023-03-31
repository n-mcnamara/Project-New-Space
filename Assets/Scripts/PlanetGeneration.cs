using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGeneration : MonoBehaviour
{
    private Vector3 startingPosition;
    public GameObject planetPrefab;
    public GameObject[] shipsPrefab;

    public int numberOfPlanets;
    public int numberOfShips;

    public GameObject[] planetsGenerated;
    public GameObject[] shipsGenerated;

    public Sprite enemySHip;
    public Sprite policeShip2;
    public Sprite[] civShips;


    public float maxHeight;
    public float maxWidth;

    // Start is called before the first frame update
    void Start()
    {
        planetsGenerated = new GameObject[numberOfPlanets];
        shipsGenerated = new GameObject[numberOfShips];

        startingPosition = transform.position;
        GeneratePlanets();
        GenerateNPC();
    }

    void GeneratePlanets()
    {
        for (int i = 0; i < numberOfPlanets; i++)
        {
            Vector2 spawnPos = FindSpawnLocation();
            if(spawnPos == new Vector2(0, 0))
            {
                Debug.LogError("No space to generate planet");
                break;
            }
            else
            {
                planetsGenerated[i] = Instantiate(planetPrefab, spawnPos, Quaternion.identity);
                //planetsGenerated[i].GetComponent<SpriteRenderer>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255), 255);
                float radius = Random.Range(50, 200);
                planetsGenerated[i].transform.localScale = new Vector2(radius, radius);
                planetsGenerated[i].GetComponent<CorePlanet>().GetBounds();
               // planetsGenerated[i].GetComponent<EnterExitAtmosphere>().GetBounds();
            }
        }

    }

    Vector2 FindSpawnLocation()
    {
        int numberOfTrys = 50;
        int i = 0;
        bool planetInWay;
        while(i < numberOfTrys)
        {
            planetInWay = false;
            float x = Random.Range(startingPosition.x, -maxWidth);
            float y = Random.Range(-maxHeight, maxHeight);

            Collider2D[] circle = Physics2D.OverlapCircleAll(new Vector2(x, y), 400);
            for (int j = 0; j < circle.Length; j++)
            {
                if (circle[j].tag == "Ground")
                {
                    i++;
                    planetInWay = true;
                }
            }
            if (!planetInWay)
            {
                return new Vector2(x, y);
            }
        }
        return new Vector2(0, 0);
    }

    void GenerateNPC()
    {
        for (int i = 0; i < numberOfShips; i++)
        {
            Vector2 spawnPos = FindSpawnLocation();
            if (spawnPos == new Vector2(0, 0))
            {
                Debug.LogError("No space to generate ship");
                break;
            }
            else
            {
                int shipType = Random.Range(0, 3);
                shipsGenerated[i] = Instantiate(shipsPrefab[shipType], spawnPos, Quaternion.identity);
                if(shipType == 1)
                {
                    int d = Random.Range(0, 2);

                    if (d == 1)
                    {
                        shipsGenerated[i].GetComponent<SpriteRenderer>().sprite = enemySHip;
                    }
                }
                else if(shipType == 0)
                {
                    int d = Random.Range(0, 4);
                    shipsGenerated[i].GetComponent<SpriteRenderer>().sprite = civShips[d];
                    
                }
                else if (shipType == 2)
                {
                    int d = Random.Range(0, 2);

                    if (d == 1)
                    {
                        shipsGenerated[i].GetComponent<SpriteRenderer>().sprite = policeShip2;
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 400);

    }
}
