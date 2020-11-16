using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathParticleSpawn : MonoBehaviour
{
    ParticleSystem particles;
    [HideInInspector] public bool bIsPlaying = false;
    public bool bCanSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    public void SpawnParticles()
    {
       if(!particles.isPlaying && bCanSpawn) particles.Play();
        bCanSpawn = false;
    }

   
}
