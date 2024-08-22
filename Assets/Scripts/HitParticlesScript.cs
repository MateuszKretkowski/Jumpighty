using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class HitParticlesScript : MonoBehaviour
{
    public ParticleSystem hitPs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ParticleSystem particles = Instantiate(hitPs, transform.position, Quaternion.identity);
        particles.Play();

        // Zniszcz system cz¹steczek po zakoñczeniu jego dzia³ania
        Destroy(particles.gameObject, particles.main.duration + particles.main.startLifetime.constantMax);
    }
}
