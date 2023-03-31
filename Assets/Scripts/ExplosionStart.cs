using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartExplosion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartExplosion()
    {
        transform.GetComponent<ParticleSystem>().Play();
    }
}
